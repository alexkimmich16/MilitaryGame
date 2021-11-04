using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Farm : MonoBehaviour
{
    //
    public int AmountAdd;
    public float PerTick;
    public float TickCount;
    public int Alligence;
    public int Seconds;
    public List<int2> SpawnSpots = new List<int2>();
    public ClickableTile CT;
    public Castle castle;

    public float Timer;
    public float MaxTimer;
    void Start()
    {
        PerTick = 1 / TurnGenerator.instance.FPS;
        TurnGenerator.OnTick += delegate (object sender, TurnGenerator.OnTickEventArgs e)
        {
            TickUpdate();
        };
    }

    void TickUpdate()
    {
        Timer += Time.deltaTime * SpeedScript.instance.CurrentSpeed;

        TickCount += PerTick;
        if (Timer > MaxTimer)
        {
            castle.AddFood(AmountAdd);
            //Currency.instance.ChangeCurrecy(AmountAdd);
            //allegiances.instance.Lists[Alligence].Money += AmountAdd;
            Timer = 0;
        }
    }
}
