using TMPro;
using UnityEngine;

public class OpponentFarm : MonoBehaviour
{
    public OpponentFarmData data;
    public Canvas FarmInfoCanvas;
    public TextMeshProUGUI InfoText;

    private Collider2D col;
    private SpriteRenderer sr;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.position = data.Location;
        ApplyDefeatedState();
    }

    private void ApplyDefeatedState()
    {
        if (data == null) return;

        // Disable clicks
        if (col != null) col.enabled = !data.Defeated;

        // Optional: dim (remove if you already have a better defeated look)
        if (sr != null)
        {
            var c = sr.color;
            c.a = data.Defeated ? 0.4f : 1f;
            sr.color = c;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            FarmInfoCanvas.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (data == null) return;

        // If defeated, do nothing (collider should already block this)
        if (data.Defeated) return;

        FarmInfoCanvas.gameObject.SetActive(true);
        InfoText.text = ToString();
        Player.Instance.enteringLevel = data;
    }

    public override string ToString()
    {
        return data.FarmerName + "\n" + data.Species + " farmer";
    }
}