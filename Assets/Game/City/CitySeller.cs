using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySeller : MonoBehaviour, IInteractable
{
    public string actionString { get; set; }
    [SerializeField] private List<string> formats = new List<string>();
    [SerializeField] private AudioClip buyAudio;

    public void Interact()
    {
        Item item = Player.inst.inventory.GetSelectedItem();
        if (item == null || item.itemData.ItemCost == 0) { return; }
        Player.inst.inventory.RemoveItemFromSelected();
        Player.inst.inventory.AddMoney(item.itemData.ItemCost);
        SoundManager.instance.PlaySound(buyAudio, 0.1f);
    }

    private void Start()
    {
        Player.inst.inventory.itemChangedEvent += UpdateString;
    }

    private void UpdateString() {
        Item item = Player.inst.inventory.GetSelectedItem();
        if (item == null) { actionString = ""; Player.inst.interactor.UpdateInteractionText();  return; }
        actionString = string.Format(formats[Random.Range(0,formats.Count)], item.itemData.ItemCost);
        Player.inst.interactor.UpdateInteractionText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player.inst.inventory.GetSelectedItem() != null)
        {
            UpdateString();
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
