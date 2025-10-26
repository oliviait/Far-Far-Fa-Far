using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    // HARD CODED FOR NOW
    public OpponentFarmData AJUTINEFARMDATA;

    public static BattleController Instance;

    // Tile
    public GameObject TilePrefab;
    public float TileScaleY;
    public float TileScaleX;
    public float TileSpacing;   // space between tiles
    private float TileWidth;
    

    // Board
    public GameObject BoardGameObject;  // parent gameobject for tiles
    public int BoardSizeX;    // how many tiles across
    public int BoardSizeY;    // how many tiles in a column
    private float ySpacing;   // used for tile placement in column
    private Tile[,] Board; // 2D matrix for holding board tiles


    // Battle
    public GameObject PiecePrefab;
    private OpponentFarmData opponentFarmData;


    // Turns
    public List<Piece> turnOrder = new List<Piece>(); 
    private Piece movingPiece;  // Piece who's turn currently is
    private Tile fromTile;  // Tile where the moving piece starts from
    private HashSet<Tile> TilesInRange = new HashSet<Tile>();

    private void Awake()
    {
        Instance = this;

        TileWidth = TilePrefab.GetComponent<Renderer>().bounds.size.x;
        ySpacing = Mathf.Sqrt(3f) * TileWidth / 2;

        TileWidth *= TileScaleX;
        ySpacing *= TileScaleY;

        Board = new Tile[BoardSizeY, BoardSizeX];
    }

    private void Start()
    {
        LoadOpponentFarmData(AJUTINEFARMDATA);

        GenerateBoard();
        PlacePieces();

        BuildTurnOrder();
        NextTurn();
    }

    private void PlacePieces()
    {
        // Enemy pieces
        int counter = 0;
        foreach (Vector2Int pos in opponentFarmData.EnemySpawnLocations)
        {
            GameObject pieceGameObject = Instantiate(PiecePrefab);
            Tile tile = Board[pos.y, pos.x];
            Piece piece = pieceGameObject.GetComponent<Piece>();

            tile.GetComponent<Tile>().SetOccupant(piece);
            pieceGameObject.transform.position = tile.transform.position;
            piece.SetData(
                opponentFarmData.
                Animals[counter]
            );
            counter++;
        }

        // Player pieces
        counter = 0;
        foreach (Vector2Int pos in opponentFarmData.PlayerSpawnLocations)
        {
            if (counter >= Player.Instance.Sheep.Count) return;  // If player doesn't have enough sheep in inv

            GameObject pieceGameObject = Instantiate(PiecePrefab);
            Tile tile = Board[pos.y, pos.x];
            Piece piece = pieceGameObject.GetComponent<Piece>();

            tile.SetOccupant(piece);
            pieceGameObject.transform.position = tile.transform.position;
            piece.SetData(Player.Instance.Sheep[counter]);
            counter++;
        }
    }

    public void LoadOpponentFarmData(OpponentFarmData ofd)
    {
        if (ofd == null) return;

        opponentFarmData = ofd;
        BoardSizeX = ofd.BoardSizeX;
        BoardSizeY = ofd.BoardSizeY;

        Board = new Tile[BoardSizeY, BoardSizeX];
    }

    public Tile[,] GetBoard() => Board;

    // Calculates the coordinates of top left tile, so that the board is centered.
    public Vector3 CalcStartPos()
    {
        float coordX;
        float coordY;

        // X coordinate
        if (BoardSizeX % 2 == 0) coordX = (-BoardSizeX / 2 + 0.5f) * TileWidth;
        else coordX = -Mathf.Floor(BoardSizeX / 2) * TileWidth;

        // Y coordinate
        if (BoardSizeY % 2 == 0) coordY = (BoardSizeY / 2 - 0.5f) * ySpacing;
        else coordY = Mathf.Floor(BoardSizeY / 2) * ySpacing;

        Vector3 pos = new Vector3(coordX, coordY, 0.0f);

        return pos;
    }
    public void GenerateBoard()
    {
        Vector3 startPos = CalcStartPos();

        float X = startPos.x;

        for (int column = 0; column < BoardSizeX; column++) // how many columns
        {
            float Y = startPos.y;

            bool right = false;
            for (int row = 0; row < BoardSizeY; row++)  // creating column
            {
                Tile tile = Instantiate(TilePrefab, BoardGameObject.transform).GetComponent<Tile>();

                // Tile pos
                if (right) tile.transform.position = new Vector3(X + (TileWidth + TileSpacing) / 2, Y, 0.0f);
                else tile.transform.position = new Vector3(X, Y, 0.0f);

                // Tile Scale
                tile.transform.localScale = new Vector3(TileScaleX, TileScaleY, 1);
                // Tile coords
                tile.GetComponent<Tile>().GridPos = new Vector2Int(row, column);

                // Tile to board
                Board[row, column] = tile;

                // Moving to next row
                Y -= ySpacing + TileSpacing;
                right = !right; // column rows alternate between left and right
            }
            X += TileWidth + TileSpacing;
        }
    }

    private void BuildTurnOrder()
    {
        // Get all pieces
        turnOrder.Clear();
        foreach (Tile tile in Board)
        {
            Piece occupant = tile.GetOccupant();
            if (occupant != null) turnOrder.Add(occupant);
        }

        // Sort pieces by next turn time asc
        turnOrder = turnOrder.OrderBy(piece => piece.GetNextMoveTime()).ToList();
    }

    private void NextTurn()
    {
        ClearHighlightsAndState();

        // turnOrder = turnOrder.Where(p => p != null).ToList();
        if (turnOrder.Count == 0) return;   // There are no pieces to move

        movingPiece = turnOrder[0]; // Always take the first piece
        movingPiece.IncrementNextMoveTime();    // Increment move time
        turnOrder = turnOrder.OrderBy(piece => piece.GetNextMoveTime()).ToList();   // Reorder

        // Find tile where movingPiece sits
        fromTile = FindTileOccupideByPiece(movingPiece);
        if (fromTile != null) fromTile.SetAsFromTile(true);

        // Check who's turn
        if (movingPiece.GetOwner() == Piece.Team.Opponent) OpponentPlay(movingPiece);
        else ShowMovableAndHighlights(movingPiece);
    }

    private Tile FindTileOccupideByPiece(Piece piece)
    {
        foreach (Tile tile in Board)
        {
            if (tile.IsOccupied())
            {
                Piece occ = tile.GetOccupant();
                if (occ == piece) return tile;
            }
        }
        return null;
    }

    private void ShowMovableAndHighlights(Piece piece)
    {
        int range = piece.GetRange();
        TilesInRange = GetReachableTiles(fromTile, range);

        foreach (Tile tile in TilesInRange)
        {
            if (tile.IsOccupied())
            {
                // Is enemy on tile
                Piece occPiece = tile.GetOccupant();
                if (occPiece.GetOwner() == Piece.Team.Opponent)
                    tile.SetHighlight(true, Tile.HighlightType.Attack);
                else
                    tile.SetHighlight(false, Tile.HighlightType.None); // Cannot land on another sheep
            }
            else
            {
                tile.SetMovable(true);
                tile.SetHighlight(true, Tile.HighlightType.Move);
            }
        }
    }

    // BFS using world-distance neighbors to build reachable set
    private HashSet<Tile> GetReachableTiles(Tile start, int range)
    {
        var visited = new HashSet<Tile>();
        var q = new Queue<(Tile tile, int dist)>();
        q.Enqueue((start, 0));
        visited.Add(start);

        while (q.Count > 0)
        {
            var (cur, dist) = q.Dequeue();
            if (dist >= 1) visited.Add(cur);

            if (dist == range) continue;

            List<Tile> neighbors = GetNeighbors(cur);

            foreach (Tile tile in neighbors)
            {
                if (visited.Contains(tile)) continue;

                // allow entering empty tile or attacking enemy tile (occupied by enemy) but not friendly-occupied
                if (tile.IsOccupied())
                {
                    Piece occ = tile.GetOccupant();
                    if (occ.Owner == Piece.Team.Player) continue; // block friendly tile
                    // enemy tile is allowable as reachable (for highlight as attack) but we do not enqueue further beyond it
                    visited.Add(tile);
                    continue;
                }

                // empty tile -> enqueue
                visited.Add(tile);
                q.Enqueue((tile, dist + 1));
            }
        }

        // remove the start tile itself from returned moves (can't "move" to where you already are)
        visited.Remove(start);
        return visited;
    }

    // Gets neighbouring tiles
    private List<Tile> GetNeighbors(Tile tile)
    {
        var res = new List<Tile>();

        int row = tile.GridPos.x; // Row
        int col = tile.GridPos.y; // Column

        //  O X X     O X X
        //  X S X -->  X S X
        //  O X X     O X X
        Debug.Log("X = " + row + "; Y = " + col);
        if (row % 2 == 1)
        {
            if (col > 0) res.Add(Board[row, col-1]);    // Left
            if (row > 0) res.Add(Board[row - 1, col]);    // Up Left
            if (row < BoardSizeY - 1) res.Add(Board[row + 1, col]);   // Down Left
            if (row > 0 && col < BoardSizeX - 1) res.Add(Board[row - 1, col + 1]);  // Up Right
            if (col < BoardSizeX - 1) res.Add(Board[row, col + 1]);   // Right
            if (row < BoardSizeY - 1 && col < BoardSizeX - 1) res.Add(Board[row + 1, col + 1]);    // Down Right
        }
        else
        //  X X O      X X O
        //  X S X --> X S X
        //  X X O      X X O
        {
            if (col > 0) res.Add(Board[row, col - 1]);    // Left
            if (col > 0 && row > 0) res.Add(Board[row - 1, col - 1]);    // Up Left
            if (col > 0 && row < BoardSizeY - 1) res.Add(Board[row + 1, col - 1]);    // Down Left
            if (row > 0) res.Add(Board[row - 1, col]);    // Up Right
            if (col < BoardSizeX - 1) res.Add(Board[row, col + 1]);    // Right
            if (row < BoardSizeY - 1) res.Add(Board[row + 1, col]);    // Down Right
        }

        return res;
    }

    // Called by Tile when player clicks a tile
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
                if (target == null) ClearTileOccupant(clicked);
                EndTurn();
                return;
            }
            else return;  // friendly occupied - do nothing
        }
        // Click on free tile
        else
        {
            // move
            fromTile.SetOccupant(null);

            clicked.SetOccupant(movingPiece);
            movingPiece.transform.position = clicked.transform.position;
            EndTurn();
            return;
        }
    }

    public void ClearTileOccupant(Tile t)
    {
        t.SetOccupant(null);
    }

    private void OpponentPlay(Piece aiPiece)
    {
        Tile start = FindTileOccupideByPiece(aiPiece);
        if (start == null) { EndTurn(); return; }

        // try attack adjacent reachable enemy
        HashSet<Tile> reachable = GetReachableTiles(start, aiPiece.Range);
        Tile attackTile = reachable.Where(tile => tile.IsOccupied() && tile.GetOccupant().Owner == Piece.Team.Player).FirstOrDefault();
        if (attackTile != null)
        {
            Piece target = attackTile.GetOccupant();
            aiPiece.Attack(target);
            if (target == null) ClearTileOccupant(attackTile);
            EndTurn();
            return;
        }

        // otherwise move to first empty reachable tile (simple)
        Tile moveTo = reachable.Where(tile => !tile.IsOccupied()).FirstOrDefault();
        if (moveTo != null)
        {
            start.SetOccupant(null);
            moveTo.SetOccupant(aiPiece);
            aiPiece.transform.position = moveTo.transform.position;
        }

        EndTurn();
    }

    public void SkipTurn()
    {
        // if player chooses skip, just end turn
        EndTurn();
    }

    private void EndTurn()
    {
        // cleanup then next
        ClearHighlightsAndState();

        movingPiece = null;
        fromTile = null;
        TilesInRange.Clear();

        NextTurn();
    }

    private void ClearHighlightsAndState()
    {
        foreach (Tile tile in Board)
        {
            tile.SetMovable(false);
            tile.SetHighlight(false, Tile.HighlightType.None);
            tile.SetAsFromTile(false);
        }
        TilesInRange.Clear();
    }
}
