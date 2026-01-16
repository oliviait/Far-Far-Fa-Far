using Unity.VisualScripting;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool Selected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Selected = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        if (Selected)
        {
            Deselect();
        }
        else if (Breeding.Instance.numSelected < 2)
        {
            Select();
        }
    }

    public void Select()
    {
        Breeding.Instance.Increase(gameObject);
        Selected = true;
        foreach (var sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = new Color(1f, 0.7f, 0.7f, 1f);
        }
    }

    public void Deselect()
    {
        Breeding.Instance.Decrease(gameObject);
        Selected = false;
        foreach (var sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
        }    
    }
}