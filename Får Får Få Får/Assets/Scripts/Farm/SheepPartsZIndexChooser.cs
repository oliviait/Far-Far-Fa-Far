using System;
using UnityEngine;

public class SheepPartsZIndexChooser : MonoBehaviour
{
    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer legs;

    private void Update()
    {
        int baseOrder = Mathf.RoundToInt(-transform.position.y * 1000); // lower y = in front

        legs.sortingOrder = baseOrder;
        body.sortingOrder = baseOrder + 1;
        head.sortingOrder = baseOrder + 2;
    }
}
