using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpponentFarm : MonoBehaviour
{
    public List<EnemyData> Animals;
    public string FarmerName;
    public string Species;
    public bool Defeated;
    public Canvas InfoCanvas;
    public TextMeshProUGUI Info;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Info.text = this.ToString();
        InfoCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InfoCanvas.enabled = false;
        }
    }

    void OnMouseDown()
    {
        if (!Defeated)
        {
            InfoCanvas.enabled = true;
        }
    }

    public override string ToString()
    {
        return FarmerName + "\n" + Species + " farmer";
    }
}
