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
        else if (Breeding.Instance.NumSelected < 2)
        {
            Select();
        }
    }

    public void Select()
    {
        Breeding.Instance.Increase(gameObject);
        Selected = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f, 1f);
    }

    public void Deselect()
    {
        Breeding.Instance.Decrease(gameObject);
        Selected = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
