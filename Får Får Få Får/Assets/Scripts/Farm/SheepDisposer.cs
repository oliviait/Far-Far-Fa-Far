using System;
using UnityEngine;

public class SheepDisposer : MonoBehaviour
{
    public static SheepDisposer Instance;
    public GameObject sheep;
    public AudioClipGroup warningSound;
    public AudioClipGroup cartWroom;
    public PolygonCollider2D bounds;
    
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartRetire()
    {
        Animator animator = Instance.GetComponent<Animator>();
        if (sheep == null) animator.SetTrigger("Warning");
        else
        {
            bounds.gameObject.SetActive(false);
            animator.SetTrigger("Retire");
        }
    }

    public void FinishRetire()
    {
        bounds.gameObject.SetActive(true);
    }

    public void RetireSheep()
    {
        if (sheep == null) return;
        SheepData data = sheep.GetComponent<Stats>().Data;
        if (Player.Instance.farmSheepList.Contains(data))
            Player.Instance.farmSheepList.Remove(data);
        Destroy(sheep.gameObject);    
    }

    public void PlayWarningSound()
    {
        warningSound.Play();
    }

    public void PlayCartWroomSound()
    {
        cartWroom.Play();
    }
}
