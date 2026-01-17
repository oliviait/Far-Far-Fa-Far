using System;
using UnityEngine;

public class SheepPartsZIndexChooser : MonoBehaviour
{
    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer legs;

    private bool isSortingLocked;

    private void Update()
    {
        if (isSortingLocked) return;    // dont change distance if locked
        
        int baseOrder = Mathf.RoundToInt(-transform.position.y * 1000); // lower y = in front

        legs.sortingOrder = baseOrder;
        body.sortingOrder = baseOrder + 1;
        head.sortingOrder = baseOrder + 2;
    }

    public void PutInDisposer()
    {
        head.sortingOrder = -3;
        body.sortingOrder = -4;
        legs.sortingOrder = -5;

        head.sortingLayerName = "Background";
        body.sortingLayerName = "Background";
        legs.sortingLayerName = "Background";
        
        isSortingLocked = true;
    }

    public void RemoveFromDisposer()
    {
        head.sortingLayerName = "Default";
        body.sortingLayerName = "Default";
        legs.sortingLayerName = "Default";
        
        isSortingLocked = false;
    }
}
