using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScannedTileData {
    public int x;
    public int y;
    //Actual positions
    public int ax;
    public int ay;
    public TileBase tile;
}

public class PlotManager : MonoBehaviour, IInteractable
{
    public static PlotManager inst;
    public Tilemap cropTilemap;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float plantActionCooldown;
    [SerializeField] private float audioPitchVariance;
    [SerializeField] private AudioClip plantAudio;
    [SerializeField] private AudioClip harvestAudio;

    public string actionString { get; set; }
    private Grid<Plot> plots;
    private Plot currentInteractablePlot;
    private bool builtInteraction = false;
    private bool canInteract = true;

    public void Interact()
    {
        if (!canInteract) return;
        if (currentInteractablePlot.crop != null && currentInteractablePlot.crop.currentStage == currentInteractablePlot.crop.data.stages.Count - 1)
        {
            HarvestCrop();
            StartCoroutine(InteractionTimer());
            return;
        }
        TryPlantSelectedCrop(currentInteractablePlot.tileData.x, currentInteractablePlot.tileData.y);
        StartCoroutine(InteractionTimer());
    }

    private IEnumerator InteractionTimer() {
        canInteract = false;
        Timer timer = new Timer(plantActionCooldown);
        while (!timer.ExecuteTimer()) {
            yield return null;
        }
        canInteract = true;
    }
    
    //Absolute shit
    private void UpdateCurrentCrop() {
        Plot plot = LocateNearestPlotToWorld(Player.inst.transform.position);
        Player.inst.interactor.RemoveInteraction(this);
        if (plot == null)
        {
            if (builtInteraction)
            {
                builtInteraction = false;
                Player.inst.interactor.RemoveInteraction(this);
            }
            return;
        }
        Player.inst.interactor.AddInteraction(this);

        currentInteractablePlot = plot;
        if (currentInteractablePlot.crop == null)
        {
            if (Player.inst.inventory.GetSelectedItem() != null && Player.inst.inventory.GetSelectedItem().itemData.GetType() == typeof(CropItemData))
            {
                actionString = "Plant";
            }
            else {
                actionString = "";
            }
            
        }
        else
        {
            actionString = currentInteractablePlot.crop.data.stages[currentInteractablePlot.crop.currentStage].actionDescription;
        }
        if (!builtInteraction)
        {
            Player.inst.interactor.AddInteraction(this);
            builtInteraction = true;
        }

    }

    private Plot LocateNearestPlotToWorld(Vector2 position) {
        Vector2Int pos = plots.GetCellAtWorld(position.x, position.y) - new Vector2Int(tilemap.cellBounds.xMin, tilemap.cellBounds.yMin);
        return LocateNearestPlot(pos.x, pos.y);
    }

    private void HarvestCrop() {
        Plot plot = currentInteractablePlot;
        SoundManager.instance.PlaySound(harvestAudio, audioPitchVariance);
        Item item = new Item(plot.crop.data.item)
        {
            count = Random.Range(1, 2 + 1) //TBF
        };
        Player.inst.inventory.TryAddItem(item);
        plot.crop.KillCrop();
        plot.crop = null;
        
    }

    private Plot LocateNearestPlot(int x, int y) {
        List<Plot> nearestPlots = plots.Get8Neighbours(new Vector2Int(x,y));
        nearestPlots.Add(plots.GetValueAt(x, y));
        //Filter
        nearestPlots = nearestPlots.Where(x => x != null).ToList();
        if (nearestPlots.Count == 0) return null;
        //Find closest
        Plot closestPlot = nearestPlots[0];
        float minDistance = float.MaxValue;
        for (int i = 0; i < nearestPlots.Count; i++) {
            float currentDistance = Vector2.SqrMagnitude(Player.inst.transform.position - new Vector3(nearestPlots[i].tileData.ax + 0.5f, nearestPlots[i].tileData.ay + 0.5f));
            if (currentDistance < minDistance) {
                closestPlot = nearestPlots[i];
                minDistance = currentDistance;
            }
        }
        return closestPlot;
    }

    private void TryPlantSelectedCrop(int x, int y) {
        if (Player.inst.inventory.GetSelectedItem() == null) return;

        ItemData itemData = Player.inst.inventory.GetSelectedItem().itemData;
        if (itemData.GetType() != typeof(CropItemData)) return;

        CropData data = ((CropItemData)itemData).CropData;
        if (plots.GetValueAt(x, y).crop != null) return;

        PlantCrop(x,y,data);
        Player.inst.inventory.RemoveItemFromSelected();
    }

    private void PlantCrop(int x, int y, CropData cropData) {
        SoundManager.instance.PlaySound(plantAudio, audioPitchVariance);
        Plot plot = plots.GetValueAt(x, y);
        if (plot.crop != null) return;
        plot.crop = new Crop(cropData) { myPlot = plot };
        plot.crop.InitializeLife();
    }

    private List<ScannedTileData> GetAllTiles() {
        List<ScannedTileData> tiles = new List<ScannedTileData>();
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++) {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile == null) continue;
                ScannedTileData tileData = new ScannedTileData() {
                    x = x - tilemap.cellBounds.xMin,
                    y = y - tilemap.cellBounds.yMin,
                    ax = x,
                    ay = y,
                    tile = tile
                };
                tiles.Add(tileData);
            }
        }
        return tiles;
    }

    private void Start()
    {
        plots = new Grid<Plot>(tilemap.cellBounds.xMax - tilemap.cellBounds.xMin, tilemap.cellBounds.yMax - tilemap.cellBounds.yMin, Vector2.one);
        List<ScannedTileData> scannedTiles = GetAllTiles();
        for (int i = 0; i < scannedTiles.Count; i++)
        {
            Plot plot = new Plot
            {
                tileData = scannedTiles[i]
            };
            plots.SetValueAt(scannedTiles[i].x, scannedTiles[i].y, plot);
        }
    }

    private void Update()
    {
        UpdateCurrentCrop();
    }

    private void Awake()
    {

        if (inst != null && inst != this)
        {
            Destroy(this);
        }
        else
        {
            inst = this;
        }
    }
}
