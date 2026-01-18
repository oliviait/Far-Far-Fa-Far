using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Region : MonoBehaviour
{
    [Header("Stages (0..N-1)")]
    public RegionData[] StageData;            
    public Sprite[] StageSprites;             
    public SpriteRenderer MapSpriteRenderer;  

    [Header("Spawning")]
    public GameObject OpponentFarmPrefab;
    public Canvas FarmInfoCanvas;
    public TextMeshProUGUI InfoText;

    [Header("UI")]
    public GameObject NextStageButton; // "More opponents"
    public GameObject PrevStageButton; // "Back"

    [Header("Audio")]
    public AudioClipGroup SwordClash;

    void Start()
    {
        if (FarmInfoCanvas != null) FarmInfoCanvas.gameObject.SetActive(false);

        LoadCurrentStage();
        UpdateStageButtons();
    }

    private int GetStageIndex()
    {
        int stage = 0;
        if (StageProgress.Instance != null) stage = StageProgress.Instance.stageIndex;

        if (StageData == null || StageData.Length == 0) return 0;
        return Mathf.Clamp(stage, 0, StageData.Length - 1);
    }

    private void LoadCurrentStage()
    {
        int stage = GetStageIndex();

        if (MapSpriteRenderer != null && StageSprites != null && stage < StageSprites.Length)
            MapSpriteRenderer.sprite = StageSprites[stage];

        RegionData data = StageData[stage];
        if (data == null || data.Farms == null) return;

        foreach (OpponentFarmData farm in data.Farms)
        {
            GameObject opponentFarm = Instantiate(OpponentFarmPrefab);
            opponentFarm.transform.position = farm.Location;

            OpponentFarm opf = opponentFarm.GetComponent<OpponentFarm>();
            opf.data = farm;
            opf.FarmInfoCanvas = FarmInfoCanvas;
            opf.InfoText = InfoText;
        }
    }

    private void UpdateStageButtons()
    {
        int stage = GetStageIndex();

        // Prev is available if we're not at the first stage
        if (PrevStageButton != null)
            PrevStageButton.SetActive(stage > 0);

        // Next is available only if all farms in current stage are defeated AND there's a next stage
        bool hasNextStage = StageData != null && stage < StageData.Length - 1;
        bool allDefeated = AllFarmsDefeatedInStage(stage);

        if (NextStageButton != null)
            NextStageButton.SetActive(hasNextStage && allDefeated);
    }

    private bool AllFarmsDefeatedInStage(int stage)
    {
        if (StageData == null || StageData.Length == 0) return false;

        RegionData data = StageData[stage];
        if (data == null || data.Farms == null) return false;

        foreach (var farm in data.Farms)
        {
            if (!farm.Defeated) return false;
        }
        return true;
    }

    public void OnNextStageClicked()
    {
        if (StageProgress.Instance == null) return;

        StageProgress.Instance.stageIndex++;
        StageProgress.Instance.stageIndex = Mathf.Clamp(StageProgress.Instance.stageIndex, 0, StageData.Length - 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPrevStageClicked()
    {
        if (StageProgress.Instance == null) return;

        StageProgress.Instance.stageIndex--;
        StageProgress.Instance.stageIndex = Mathf.Clamp(StageProgress.Instance.stageIndex, 0, StageData.Length - 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onBattleButtonClicked()
    {
        if (SwordClash != null) SwordClash.Play();
        SceneManager.LoadScene(3);
    }

    public void onBackButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}
