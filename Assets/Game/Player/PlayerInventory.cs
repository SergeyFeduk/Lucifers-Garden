using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    private const int inventorySize = 6;

    [Header("Imports")]
    [SerializeField] private Transform itemRendererHolder;
    [SerializeField] private GameObject itemRendererPrefab;
    [SerializeField] private GameObject selectedSlotCaret;
    [SerializeField] private TMP_Text selectedItemName;
    [SerializeField] private TMP_Text moneyAmountText;

    private Item[] items = new Item[inventorySize];
    private Image[] itemRenderer = new Image[inventorySize];
    private TMP_Text[] itemTextRenderer = new TMP_Text[inventorySize];
    private int selectedSlot;
    private int money;

    public UnityAction itemChangedEvent;

    public void AddMoney(int amount) {
        money += amount;
        moneyAmountText.SetText(string.Format("{0} coins", money));
    }

    public int GetMoney() {
        return money;
    }

    public Item FindItem(ItemData data) {
        for (int i = 0; i < inventorySize; i++) {
            if (items[i] != null && items[i].itemData == data) {
                return items[i];
            }
        }
        return null;
    }

    public int FindItemSlot(ItemData data) {
        for (int i = 0; i < inventorySize; i++)
        {
            if (items[i] != null && items[i].itemData == data)
            {
                return i;
            }
        }
        return 0;
    }

    public void SetItemAtSlot(Item item, int slot, int amount) {
        if (slot > items.Length) return;
        item.count = amount;
        items[slot] = item;
        //Manage graphics
        SetSlotGraphics(slot, false, items[slot]);
        if (slot == selectedSlot) UpdateItemNameText(slot);
    }

    public void RemoveItemAtSlot(int slot) {
        if (slot > items.Length) return;
        items[slot] = null;
        SetSlotGraphics(slot, true);
        if (slot == selectedSlot) UpdateItemNameText(slot);
    }

    public void RemoveItemFromSlot(int slot) {
        if (slot > items.Length) return;
        items[slot].count--;
        if (items[slot].count <= 0) {
            RemoveItemAtSlot(slot);
            return;
        }
        SetSlotGraphics(slot, false, items[slot]);
        if (slot == selectedSlot) UpdateItemNameText(slot);
    }

    public void RemoveItemFromSlot(int slot, int amount)
    {
        if (slot > items.Length) return;
        items[slot].count-= amount;
        if (items[slot].count <= 0)
        {
            RemoveItemAtSlot(slot);
            return;
        }
        SetSlotGraphics(slot, false, items[slot]);
        if (slot == selectedSlot) UpdateItemNameText(slot);
    }

    public void RemoveItemFromSelected() {
        RemoveItemFromSlot(selectedSlot);
    }

    public void RemoveItemAtSelectedSlot() {
        RemoveItemAtSlot(selectedSlot);
    }

    public bool TryAddItem(Item item) {
        for (int i = 0; i < inventorySize; i++) {
            if (items[i] != null && items[i].itemData.ItemName == item.itemData.ItemName) {
                SetItemAtSlot(item, i, items[i].count + item.count);
                return true;
            }
            if (items[i] != null) continue;
            SetItemAtSlot(item,i, item.count);
            return true;
        }
        return false;
    }

    public Item GetItemAtSlot(int slot) {
        if (slot > items.Length) return null;
        return items[slot];
    }

    public Item GetSelectedItem() {
        return items[selectedSlot];
    }

    private void SetSelectedSlot(int index) {
        selectedSlot = index;
        selectedSlotCaret.GetComponent<RectTransform>().position = itemRenderer[index].transform.position;
        itemChangedEvent.Invoke();
        UpdateItemNameText(index);
    }

    private void UpdateItemNameText(int index) {
        if (items[index] == null)
        {
            selectedItemName.SetText("");
            return;
        }
        selectedItemName.SetText(items[index].itemData.ItemName);
    }

    private void SetSlotGraphics(int slot, bool clear, Item item = null) {
        if (clear) {
            itemRenderer[slot].sprite = null;
            itemRenderer[slot].color = Color.clear;
            itemTextRenderer[slot].SetText("");
            return;
        }
        itemRenderer[slot].sprite = item.itemData.ItemIcon;
        itemRenderer[slot].color = Color.white;
        itemTextRenderer[slot].SetText(items[slot].count.ToString());
    }

    private void Start()
    {
        for (int i = 0; i < inventorySize; i++) {
            GameObject obj = Instantiate(itemRendererPrefab, itemRendererHolder);
            itemRenderer[i] = obj.GetComponent<Image>();
            itemTextRenderer[i] = obj.GetComponentInChildren<TMP_Text>();
            SetSlotGraphics(i, true);
        }
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y >= 1) {
            selectedSlot = Math.mod((selectedSlot + 1), inventorySize);
            SetSelectedSlot(selectedSlot);
        }
        if (Input.mouseScrollDelta.y <= -1)
        {
            selectedSlot = Math.mod((selectedSlot - 1), inventorySize);
            SetSelectedSlot(selectedSlot);
        }

        for (int i = 0; i <= inventorySize - 1; i++) {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
                SetSelectedSlot(i);
            }
        }
    }
}
