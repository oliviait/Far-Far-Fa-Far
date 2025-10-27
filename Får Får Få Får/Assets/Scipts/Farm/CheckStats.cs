using TMPro;
using UnityEngine;

public class CheckStats : MonoBehaviour
{
    public string StatsDisplay;
    private TextMeshProUGUI displayStats;
    public Vector3 Offset = new Vector3 (-4, -5, 0);

    void Start()
    {
        displayStats = GameObject.Find(StatsDisplay).GetComponent<TextMeshProUGUI>();
        displayStats.enabled = false;
    }

    private void OnMouseOver()
    {
        displayStats.transform.localPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position + Offset);
        if (gameObject.transform.position.x > 5)
        {
            displayStats.transform.localPosition += new Vector3(-90, 0, 0);
        }
        Stats stats = gameObject.GetComponent<Stats>();
        displayStats.text = stats.ToString();
        displayStats.enabled = true;
    }

    private void OnMouseExit()
    {
        displayStats.enabled = false;
    }
}
