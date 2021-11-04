using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProgress : MonoBehaviour
{
    public float SecondsNeeded;
    public float TickCount;
    public int Current;
    public int PerWorker;
    public ClickableTile tile;
    public int Workers;
    public float PerTick;
    public bool Destoryed;
    void Start()
    {
        Destoryed = false;
        PerTick = 1 / TurnGenerator.instance.FPS;
        TurnGenerator.OnTick += delegate (object sender, TurnGenerator.OnTickEventArgs e)
        {
            TickUpdate();
        };
    }
    public void TickUpdate()
    {
        TickCount += PerTick * Workers;
        if (TickCount >= SecondsNeeded && Destoryed == false)
        {
            Destoryed = true;
            tile.CompleteBuilding();
            Destroy(gameObject);

        }
            
    }
    public void ChangeWorker(bool Add)
    {
        if (Add == true)
        {
            Workers += 1;
        }
        else
        {
            Workers -= 1;
        }
    }
}
