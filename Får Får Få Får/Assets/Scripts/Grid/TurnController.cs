using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static TurnController Instance;
    // Board
    private Tile[,] BoardTiles;  // 2D matrix for holding board tiles

    // Turns
    public List<Piece> turnOrder = new List<Piece>();
    private Piece movingPiece;  // Piece who's turn currently is
    private Tile movingFromTile;  // Tile where the moving piece starts from
    private HashSet<Tile> TilesInRange = new HashSet<Tile>();

    // Win/Lose con
    public TextMeshProUGUI WinLoseText;
    public GameObject WinLosePanel;

    // AI
    public float TimeBetweenAIAttacks;
    private float nextAIAttackTime = float.MaxValue;

    private void Awake()
    {
        Instance = this;
        nextAIAttackTime = Time.time;

        WinLosePanel.gameObject.SetActive(false);

        Events.onBattleLost += GameLost;
        Events.onBattleWon += GameWon;
    }

    private void OnDestroy()
    {
        Events.onBattleLost -= GameLost;
        Events.onBattleWon -= GameWon;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BoardTiles = Board.Instance.GetBoard();
        BuildTurnOrder();
        NextTurn();
    }
    //-------------------------------------
    //      Win/Lose cons
    //-------------------------------------
    private void GameLost()
    {
        WinLosePanel.gameObject.SetActive(true);
        WinLoseText.text = "You Lost!";
        // Add restart level button later
        gameObject.SetActive(false);
        Player.Instance.freshStart = false;
    }

    private void GameWon()
    {
        WinLosePanel.gameObject.SetActive(true);
        WinLoseText.text = "You Won!";

        // Mark the actual farm data as defeated (this matches your existing map logic)
        if (Player.Instance != null && Player.Instance.enteringLevel != null)
        {
            Player.Instance.enteringLevel.Defeated = true;
        }

        gameObject.SetActive(false);
        Player.Instance.freshStart = false;
    }



    //-------------------------------------
    //      Turn 
    //-------------------------------------

    private void BuildTurnOrder()
    {
        // Get all pieces
        turnOrder.Clear();
        foreach (Tile tile in BoardTiles)
        {
            Piece occupant = tile.GetOccupant();
            if (occupant != null) turnOrder.Add(occupant);
        }

        // Sort pieces by next turn time asc
        turnOrder = turnOrder.OrderBy(piece => piece.GetNextMoveTime()).ToList();
    }

    private void NextTurn()
    {
        if (turnOrder.Count == 0) return;   // There are no pieces to move
        
        BuildTurnOrder();
        movingPiece = turnOrder[0]; // Always take the first piece
        movingPiece.IncrementNextMoveTime();    // Increment move time

        // Find tile where movingPiece sits
        movingFromTile = movingPiece.GetTilePlacedOn();
        if (movingFromTile != null) movingFromTile.SetTileType(Tile.TileType.MovingFrom);

        // Check who's turn
        if (movingPiece.GetOwner() == Piece.Team.Opponent) OpponentPlay(movingPiece);
        else ShowMovableAndHighlights(movingPiece);
    }
    public void SkipTurn()  // Skipping a turn means that no action, so just end turn
    {
        EndTurn();
    }

    private void EndTurn()
    {
        // cleanup then next
        ClearHighlightsAndState();

        movingPiece = null;
        movingFromTile = null;
        TilesInRange.Clear();

        NextTurn();
    }

    private void ClearHighlightsAndState()
    {
        foreach (Tile tile in BoardTiles)
        {
            tile.SetTileType(Tile.TileType.Default);
            tile.SetHighlight(false, Tile.HighlightType.None);
        }
        TilesInRange.Clear();
    }

    //-------------------------------------
    //      Player's Turn
    //-------------------------------------

    // Color tiles, so player can see available moves
    private void ShowMovableAndHighlights(Piece piece)
    {
        int range = piece.GetRange();
        TilesInRange = Board.Instance.GetReachableTiles(piece.GetTilePlacedOn(), range);

        // Set type and highlight for all tiles in moving range
        foreach (Tile tile in TilesInRange)
        {
            if (tile.IsOccupied())
            {
                if (tile.GetOccupant().GetOwner() == Piece.Team.Opponent)
                {
                    // Tile with enemy piece on it
                    tile.SetHighlight(true, Tile.HighlightType.Attack); 
                    tile.SetTileType(Tile.TileType.Attackable);
                }
                else
                {
                    // Tile with player's piece on it
                    tile.SetHighlight(false, Tile.HighlightType.None);
                    tile.SetTileType(Tile.TileType.Default);
                }
            }
            else
            {
                // Free tile
                tile.SetHighlight(true, Tile.HighlightType.Free);
                tile.SetTileType(Tile.TileType.Free);
            }
        }
    }

    // Called by Tile when player clicks it
    public void OnTileClicked(Tile clicked)
    {
        if (movingPiece == null) return;
        if (!TilesInRange.Contains(clicked)) return;

        // Click on occupied tile
        if (clicked.IsOccupied())
        {
            Piece target = clicked.GetOccupant();
            if (target.Owner == Piece.Team.Opponent)
            {
                // attack (do not move onto tile)
                movingPiece.Attack(target);
                if (target == null) movePiece(movingFromTile, clicked); // if enemy died, move there
                EndTurn();
                return;
            }
            else return;  // friendly occupied - do nothing
        }
        // Click on free tile
        else
        {
            // move
            movePiece(movingFromTile, clicked);
            EndTurn();
            return;
        }
    }

    //-------------------------------------
    //      AI's Turn
    //-------------------------------------

    private void OpponentPlay(Piece aiPiece)
    {
        Tile start = aiPiece.GetTilePlacedOn();
        if (start == null) { 
            EndTurn(); 
            return; 
        }

        // Try attack reachable enemy
        HashSet<Tile> reachable = Board.Instance.GetReachableTiles(start, aiPiece.Range);
        Tile attackTile = reachable.Where(tile => tile.IsOccupied() && tile.GetOccupant().Owner == Piece.Team.Player).FirstOrDefault();
        if (attackTile != null)
        {
            Piece target = attackTile.GetOccupant();
            aiPiece.Attack(target);
            if (Piece.NumberOfPlayerPieces == 0) return;
            EndTurn();
            return;
        }

        // Otherwise move to first empty reachable tile
        Tile moveTo = reachable.Where(tile => !tile.IsOccupied()).FirstOrDefault();
        if (moveTo != null) movePiece(start, moveTo);
        EndTurn();
    }

    private void movePiece(Tile startTile, Tile endTile)
    {
        Piece piece = startTile.GetOccupant();
        if (piece == null) return;
        startTile.SetOccupant(null);
        endTile.SetOccupant(piece);
        piece.SetTilePlacedOn(endTile);
        movingPiece.transform.position = endTile.transform.position;
    }
}
