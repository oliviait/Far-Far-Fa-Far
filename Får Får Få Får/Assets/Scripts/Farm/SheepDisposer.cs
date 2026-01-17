using System;
using UnityEngine;

public class SheepDisposer : MonoBehaviour
{
    public static SheepDisposer Instance;
    public GameObject sheep;
    public AudioClipGroup warningSound;
    public AudioClipGroup cartWroom;
    
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartRetire()
    {
        Animator animator = Instance.GetComponent<Animator>();
        if (sheep == null) animator.SetTrigger("Warning"); 
        else animator.SetTrigger("Retire");
    }

    public void RetireSheep()
    {
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
