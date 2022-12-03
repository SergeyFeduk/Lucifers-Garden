using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelSeller : MonoBehaviour, IInteractable
{
    [SerializeField] private string str;
    [SerializeField] private ItemData steel;
    [SerializeField] private AudioClip buyAudio;
    public string actionString { get; set; }

    private void Start()
    {
        actionString = str;
    }

    public void Interact()
    {
        if (Player.inst.inventory.GetMoney() >= 30) {
            Player.inst.inventory.AddMoney(-30);
            Player.inst.inventory.TryAddItem(new Item(steel));
            SoundManager.instance.PlaySound(buyAudio,0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.inst.interactor.UpdateInteractionText();
            Player.inst.interactor.AddInteraction(this);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.inst.interactor.RemoveInteraction(this);
        }
    }
}
