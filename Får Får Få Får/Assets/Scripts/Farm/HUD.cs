using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button mapButton;
    public Button breedButton;

    public List<GameObject> inventorySlots;
    public GameObject inventorySlotsHolder;
    public AudioClipGroup mapCrunch;
    public AudioClipGroup wrongAction;

    public void OnMapButtonClicked()
    {
        if (Player.Instance.InventorySheepList.Count == 0)
        {
            Animator slotAnimator = inventorySlotsHolder.GetComponent<Animator>();
            slotAnimator.SetTrigger("Wiggle");
            foreach (var slot in inventorySlots)
            {
                slotAnimator = slot.GetComponent<Animator>();
                slotAnimator.SetTrigger("Flash");
            }

            wrongAction.Play();
        }
        else
        {
            mapCrunch.Play();
            SceneManager.LoadScene(2);
        }
    }

    public void OnBreedButtonClicked()
    {
        Breeding.Instance.Breed();
    }
}
