using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public static StatsDisplay Instance;
    public TextMeshProUGUI statsText;
    public Vector2 screenOffset = new(50f, 0f);


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        gameObject.SetActive(false);
    }


    public void Show(Stats stats, Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        transform.position = screenPos + (Vector3)screenOffset;
        if (screenPos.x > 600) transform.position -= new Vector3(250.0f, 0.0f, 0.0f);
        
        statsText.text = stats.ToString();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}