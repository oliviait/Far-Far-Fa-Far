using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Region : MonoBehaviour
{
    [Header("Stages (0..N-1)")]
    public RegionData[] StageData;            // Orebro, Svealand, Sverige, Norra_Europa (in this order)
    public Sprite[] StageSprites;             // matching order
    public SpriteRenderer MapSpriteRenderer;  // drag the Map object's SpriteRenderer here

    [Header("Spawning")]
    public GameObject OpponentFarmPrefab;
    public Canvas FarmInfoCanvas;
    public TextMeshProUGUI InfoText;

    [Header("UI")]
    public GameObject NextStageButton;

    [Header("Audio")]
    public AudioClipGroup SwordClash;

    void Start()
    {
        if (FarmInfoCanvas != null) FarmInfoCanvas.gameObject.SetActive(false);
        if (NextStageButton != null) NextStageButton.SetActive(false);

        LoadCurrentStage();
        CheckAllDefeatedForCurrentStage();
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

        // Swap map sprite
        if (MapSpriteRenderer != null && StageSprites != null && stage < StageSprites.Length)
            MapSpriteRenderer.sprite = StageSprites[stage];

        // Spawn farms for this stage (INCLUDING defeated ones)
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

    private void CheckAllDefeatedForCurrentStage()
    {
        if (NextStageButton == null) return;

        int stage = GetStageIndex();
        RegionData data = StageData[stage];
        if (data == null || data.Farms == null)
        {
            NextStageButton.SetActive(false);
            return;
        }

        foreach (var farm in data.Farms)
        {
            if (!farm.Defeated)
            {
                NextStageButton.SetActive(false);
                return;
            }
        }

        bool hasNext = stage < StageData.Length - 1;
        NextStageButton.SetActive(hasNext);
    }

    // Hook this up to NextStageButton OnClick
    public void OnNextStageClicked()
    {
        if (StageProgress.Instance == null) return;

        StageProgress.Instance.stageIndex++;
        StageProgress.Instance.stageIndex = Mathf.Clamp(StageProgress.Instance.stageIndex, 0, StageData.Length - 1);

        // Reload map scene to clear old spawned farm GameObjects
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
