using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class CropStage {
    public TileBase tile;
    public string actionDescription;
    public float timeToGrow;
}

[CreateAssetMenu(fileName = "Crop")]
public class CropData : ScriptableObject    
{
    [field: SerializeField] public List<CropStage> stages = new List<CropStage>();
    [field: SerializeField] public ItemData item;
}
