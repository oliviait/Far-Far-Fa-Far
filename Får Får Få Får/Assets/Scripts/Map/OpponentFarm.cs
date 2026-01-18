using TMPro;
using UnityEngine;

public class OpponentFarm : MonoBehaviour
{
    public OpponentFarmData data;
    public Canvas FarmInfoCanvas;
    public TextMeshProUGUI InfoText;

    private GameObject FarmInfoPanel;
    private Collider2D col;
    private SpriteRenderer sr;
    
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        FarmInfoPanel = FarmInfoCanvas.transform.GetChild(0).gameObject;
        transform.position = data.Location;
        ApplyDefeatedState();
    }

    private void ApplyDefeatedState()
    {
        if (data == null) return;

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
        // if click off panel
        if (Input.GetMouseButtonDown(0) && FarmInfoCanvas.isActiveAndEnabled)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    FarmInfoPanel.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    null))
            {
                FarmInfoCanvas.gameObject.SetActive(false);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
            FarmInfoCanvas.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (data == null) return;

        StartCoroutine(OpenPanelNextFrame());
    }

    // DON'T TOUCH THIS
    // SCARY I KNOW BUT IT WORKS
    private System.Collections.IEnumerator OpenPanelNextFrame()
    {
        yield return null; // wait one frame, necessary for clicking off to close panel
        FarmInfoCanvas.gameObject.SetActive(true);
        InfoText.text = ToString();
        Player.Instance.enteringLevel = data;
    }

    public override string ToString()
    {
        return data.FarmerName + "\n" + data.Species + " farmer";
    }
}