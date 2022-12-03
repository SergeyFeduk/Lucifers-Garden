using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class ItemData : ScriptableObject
{
    [field:SerializeField] public string ItemName      { get; private set; }
    [field: SerializeField] public Sprite ItemIcon      { get; private set; }
    [field: SerializeField] public int ItemCost         { get; private set; }
    [field: SerializeField] public GameObject WorldItem { get; private set; }
}
