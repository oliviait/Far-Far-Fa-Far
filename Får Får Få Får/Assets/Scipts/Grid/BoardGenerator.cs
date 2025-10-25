using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject TilePrefab;
    public float TileScaleY;
    public float TileScaleX;
    public float TileSpacing;

    private float TileWidth;

    public GameObject BoardGameObject;

    public int BoardSizeX;    // how many tiles across
    public int BoardSizeY;    // how many tiles in a column

    private float ySpacing;   // used for tile placement in column

    private GameObject[,] Board; // 2D matrix for holding board tiles

    private void Awake()
    {
        TileWidth = TilePrefab.GetComponent<Renderer>().bounds.size.x;
        ySpacing = Mathf.Sqrt(3f) * TileWidth / 2;

        TileWidth *= TileScaleX;
        ySpacing *= TileScaleY;

        Board = new GameObject[BoardSizeY, BoardSizeX];
    }

    private void Start()
    {
        // Generate board
        GenerateBoard(BoardSizeX, BoardSizeY);
    }

    public GameObject[,] GetBoard() => Board;
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
    public void GenerateBoard(int BoardX, int BoardY)
    {
        Vector3 startPos = CalcStartPos();

        float X = startPos.x;

        for (int column = 0; column < BoardSizeX; column++) // how many columns
        {
            float Y = startPos.y;

            bool right = false;
            for (int row = 0; row < BoardSizeY; row++)  // creating column
            {
                GameObject tile = Instantiate(TilePrefab, BoardGameObject.transform);

                // Tile pos
                if (right)
                    tile.transform.position = new Vector3(X + (TileWidth + TileSpacing) / 2, Y, 0.0f);
                else
                    tile.transform.position = new Vector3(X, Y, 0.0f);

                // Tile Scale
                tile.transform.localScale = new Vector3(TileScaleX, TileScaleY, 1);
                // Tile coords
                tile.GetComponent<Tile>().GridPos = new Vector2(row, column);

                // Tile to board
                Board[row, column] = tile;

                // Moving to next row
                Y -= ySpacing + TileSpacing;
                right = !right; // column rows alternate between left and right
            }
            X += TileWidth + TileSpacing;
        }
    }
}
