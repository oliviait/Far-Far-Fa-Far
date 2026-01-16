using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public static Board Instance;

    public AudioClipGroup SheepMusic;
    public AudioClipGroup PigMusic;
    public AudioClipGroup CowMusic;

    // Tile
    public Tile TilePrefab;
    public float TileScaleY;
    public float TileScaleX;
    public float TileSpacing;   // space between tiles
    private float TileWidth;

    // Board
    public GameObject BoardGameObject;  // parent gameobject for tiles
    public int BoardSizeX;    // how many tiles across
    public int BoardSizeY;    // how many tiles in a column
    private float ySpacing;   // used for tile placement in column
    private Tile[,] BoardTiles;  // 2D matrix for holding board tiles

    // Pieces
    public Piece PiecePrefab;

    private OpponentFarmData opponentFarmData;


    private void Awake()
    {
        Instance = this;

        TileWidth = TilePrefab.GetComponent<Renderer>().bounds.size.x;
        ySpacing = Mathf.Sqrt(3f) * TileWidth / 2;

        TileWidth *= TileScaleX;
        ySpacing *= TileScaleY;

        LoadOpponentFarmData(Player.Instance.enteringLevel);

        BoardTiles = new Tile[BoardSizeY, BoardSizeX];
        
        GenerateBoard();
        PlacePieces();
    }
    public Tile[,] GetBoard() => BoardTiles;

    public void LoadOpponentFarmData(OpponentFarmData ofd)
    {
        if (ofd == null)
        {
            Debug.Log("BATTLECONTROLLER GOT NULL DATA");
            return;
        }
        if (ofd.Species == "Sheep") AudioManager.Instance.BattleMusic = SheepMusic;
        else if (ofd.Species == "Cow") AudioManager.Instance.BattleMusic = CowMusic;
        else if (ofd.Species == "Pig") AudioManager.Instance.BattleMusic = PigMusic;
        opponentFarmData = ofd;
        BoardSizeX = ofd.BoardSizeX;
        BoardSizeY = ofd.BoardSizeY;

        BoardTiles = new Tile[BoardSizeY, BoardSizeX];
    }

    // Calculates the coordinates of top left tile, so that the board is centered.
    public Vector3 CalculateBoardStartPos()
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
        Vector3 startPos = CalculateBoardStartPos();

        float X = startPos.x;

        for (int column = 0; column < BoardSizeX; column++) // how many columns
        {
            float Y = startPos.y;

            bool right = false;
            for (int row = 0; row < BoardSizeY; row++)  // creating column
            {
                Tile tile = GameObject.Instantiate<Tile>(TilePrefab, BoardGameObject.transform);

                // Tile pos
                if (right) tile.transform.position = new Vector3(X + (TileWidth + TileSpacing) / 2, Y, 0.0f);
                else tile.transform.position = new Vector3(X, Y, 0.0f);

                // Tile scale and coords
                tile.transform.localScale = new Vector3(TileScaleX, TileScaleY, 1);
                tile.GridPos = new Vector2Int(row, column);

                // Add tile to board
                BoardTiles[row, column] = tile;

                // Moving to next row
                Y -= ySpacing + TileSpacing;
                right = !right; // column rows alternate between left and right
            }
            X += TileWidth + TileSpacing;
        }
    }

    private void PlacePieces()
    {
        Piece.NumberOfEnemyPieces = 0;
        Piece.NumberOfPlayerPieces = 0;

        // Place enemy pieces
        int counter = 0;
        foreach (Vector2Int enemieSpawnLoc in opponentFarmData.EnemySpawnLocations)
        {
            Piece piece = GameObject.Instantiate<Piece>(PiecePrefab);
            Tile tile = BoardTiles[enemieSpawnLoc.y, enemieSpawnLoc.x];

            tile.GetComponent<Tile>().SetOccupant(piece);
            piece.transform.position = tile.transform.position;
            piece.SetData(opponentFarmData.Animals[counter]);
            Piece.NumberOfEnemyPieces++;
            piece.SetTilePlacedOn(tile);    // Link piece to tile it's placed on
            counter++;
        }

        // Player pieces
        counter = 0;
        foreach (Vector2Int pos in opponentFarmData.PlayerSpawnLocations)
        {
            // If player doesn't have enough sheep in inv
            if (counter >= Player.Instance.InventorySheepList.Count(s => s != null)) return;  

            Piece piece = GameObject.Instantiate<Piece>(PiecePrefab);
            Tile tile = BoardTiles[pos.y, pos.x];

            tile.SetOccupant(piece);
            piece.transform.position = tile.transform.position;
            piece.SetData(Player.Instance.InventorySheepList[counter]);
            Piece.NumberOfPlayerPieces++;
            piece.SetTilePlacedOn(tile);  // Link piece to tile it's placed on
            counter++;
        }
    }

    // BFS using world-distance neighbors to build reachable set
    public HashSet<Tile> GetReachableTiles(Tile start, int range)
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
                    if (occ == null) Debug.Log("occ is null");
                    else if (start == null) Debug.Log("start is null");
                    else if (start.GetOccupant() == null) Debug.Log("start.occupant is null");
                    if (occ.Owner == start.GetOccupant().Owner) continue; // block friendly tile
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
        if (tile == null) return null;
        var res = new List<Tile>();

        int row = tile.GridPos.x; // Row
        int col = tile.GridPos.y; // Column

        //  O X X     O X X
        //  X S X -->  X S X
        //  O X X     O X X
        if (row % 2 == 1)
        {
            if (col > 0) res.Add(BoardTiles[row, col - 1]);    // Left
            if (row > 0) res.Add(BoardTiles[row - 1, col]);    // Up Left
            if (row < BoardSizeY - 1) res.Add(BoardTiles[row + 1, col]);   // Down Left
            if (row > 0 && col < BoardSizeX - 1) res.Add(BoardTiles[row - 1, col + 1]);  // Up Right
            if (col < BoardSizeX - 1) res.Add(BoardTiles[row, col + 1]);   // Right
            if (row < BoardSizeY - 1 && col < BoardSizeX - 1) res.Add(BoardTiles[row + 1, col + 1]);    // Down Right
        }
        else
        //  X X O      X X O
        //  X S X --> X S X
        //  X X O      X X O
        {
            if (col > 0) res.Add(BoardTiles[row, col - 1]);    // Left
            if (col > 0 && row > 0) res.Add(BoardTiles[row - 1, col - 1]);    // Up Left
            if (col > 0 && row < BoardSizeY - 1) res.Add(BoardTiles[row + 1, col - 1]);    // Down Left
            if (row > 0) res.Add(BoardTiles[row - 1, col]);    // Up Right
            if (col < BoardSizeX - 1) res.Add(BoardTiles[row, col + 1]);    // Right
            if (row < BoardSizeY - 1) res.Add(BoardTiles[row + 1, col]);    // Down Right
        }

        return res;
    }
}
