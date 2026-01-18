using UnityEngine;

public class CheckStats : MonoBehaviour
{
    public Vector3 offset;
    private Stats stats;

    void Awake()
    {
        stats = GetComponent<Stats>();
    }

    void OnMouseOver()
    {
        StatsDisplay.Instance.Show(stats, transform.position + offset);
    }

    void OnMouseExit()
    {
        StatsDisplay.Instance.Hide();
    }
}