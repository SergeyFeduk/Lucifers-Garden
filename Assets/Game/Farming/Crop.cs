using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crop
{
    public CropData data;
    public Plot myPlot;
    private Timer stageTimer;
    public int currentStage = 0;
    public bool satisfiedConditions = true;

    public Crop(CropData data)
    {
        this.data = data;
    }

    public void InitializeLife() {
        PlotManager.inst.cropTilemap.SetTile(new Vector3Int(myPlot.tileData.ax, myPlot.tileData.ay, 0), data.stages[currentStage].tile);
        StageRoutine();
    }

    public void KillCrop() {
        PlotManager.inst.cropTilemap.SetTile(new Vector3Int(myPlot.tileData.ax, myPlot.tileData.ay, 0), null);
    }

    private void GoNextStage() {
        if (currentStage < data.stages.Count - 1)
        {
            currentStage++;
            PlotManager.inst.cropTilemap.SetTile(new Vector3Int(myPlot.tileData.ax, myPlot.tileData.ay, 0), data.stages[currentStage].tile);
            CheckSatisfiedCondition();
        }
        
    }

    public void CheckSatisfiedCondition() {
        if (satisfiedConditions)
        {
            StageRoutine();
        }
    }

    private async void StageRoutine() {
        stageTimer = new Timer(data.stages[currentStage].timeToGrow);
        while (!stageTimer.ExecuteTimer()) {
            await Task.Yield();
        }
        GoNextStage();
    }
}
