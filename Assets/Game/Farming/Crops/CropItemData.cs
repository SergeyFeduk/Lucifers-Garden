using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropItem")]
public class CropItemData : ItemData
{
    [field: SerializeField] public CropData CropData { get; private set; }

}
