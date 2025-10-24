using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Vector2 GridPos;
    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
