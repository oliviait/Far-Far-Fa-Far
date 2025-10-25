using TMPro;
using UnityEngine;

public class CheckStats : MonoBehaviour
{
    public TextMeshProUGUI DisplayStats;
    public Vector3 Offset = new Vector3 (-4, -5, 0);

    void Start()
    {
        DisplayStats.enabled = false;
    }

    private void OnMouseOver()
    {
        DisplayStats.transform.localPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position + Offset);
        if (gameObject.transform.position.x > 5)
        {
            DisplayStats.transform.localPosition += new Vector3(-90, 0, 0);
        }
        Stats stats = gameObject.GetComponent<Stats>();
        DisplayStats.text = stats.ToString();
        DisplayStats.enabled = true;
    }

    private void OnMouseExit()
    {
        DisplayStats.enabled = false;
    }
}
