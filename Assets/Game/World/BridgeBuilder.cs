using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBuilder : MonoBehaviour, IInteractable
{
    [SerializeField] private string str;
    [SerializeField] private ItemData steelData;
    public string actionString { get; set; }
    private bool finished = false;

    public void Interact() {
        if (!finished && Player.inst.inventory.FindItem(steelData) != null && Player.inst.inventory.FindItem(steelData).count >= 5) {
            finished = true;
            Player.inst.inventory.RemoveItemFromSlot(Player.inst.inventory.FindItemSlot(steelData),5);
            BridgeController.inst.AddStage();
        }
    }

    private void Start()
    {
        actionString = str;
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
