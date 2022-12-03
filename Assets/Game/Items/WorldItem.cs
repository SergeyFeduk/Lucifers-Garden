using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    [SerializeField] private string actionName;
    public string actionString { get; set; }

    public void Interact() {
        if (!Player.inst.inventory.TryAddItem(new Item(itemData))) return;
        Player.inst.interactor.RemoveInteraction(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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

    private void Start()
    {
        actionString = actionName;
    }
}
