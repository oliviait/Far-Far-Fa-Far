using UnityEngine;

public class StageProgress : MonoBehaviour
{
    public static StageProgress Instance;

    public int stageIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}