using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarracksManager : MonoBehaviour
{
    //1 = worker
    //2 = Infantry

    #region Singleton

    public static BarracksManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public List<ItemSlot> Items = new List<ItemSlot>();
    public Sprite Infantry;
    public Sprite Worker;
    public Sprite Mech;

    public List<Text> text = new List<Text>();

    public List<int> Quoe = new List<int>(4);
    public List<int> TurnsLeft = new List<int>();

    public int SpawnNumber;

    public int X;
    public int Y;

    public BarracksSpawn CurrentBarracks;

    public float PerTick;
    public float TickCount;
    public int Seconds;

    

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
        if(CurrentBarracks != null && TurnsLeft[0] != 0)
        {
            TickCount += PerTick;
            if (TickCount >= Seconds)
            {
                TickCount = 0;
                TurnsLeft[0] -= 1;
                if (TurnsLeft[0] == 0)
                {
                    SpawnNumber = Quoe[0];
                    Quoe[0] = 0;
                    TurnsLeft.RemoveAt(0);
                    TurnsLeft.Insert(3, 0);
                    CurrentBarracks.Spawn(SpawnNumber);
                }
            }
        }
    }

    void InventoryChange()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Quoe[i] == 0)
            {
                Items[i].UIImage.sprite = null;
            }
            else if(Quoe[i] == 1)
            {
                Items[i].UIImage.sprite = Worker;
            }
            else if (Quoe[i] == 2)
            {
                Items[i].UIImage.sprite = Infantry;
            }
            else if (Quoe[i] == 3)
            {
                Items[i].UIImage.sprite = Mech;

            }
        }
    }

    void UpdateIndividual()
    {
        InventoryChange();
        for (int i = 0; i < 4; i++)
        {
            CurrentBarracks.Quoe[i] = Quoe[i];
            CurrentBarracks.TurnsLeft[i] = TurnsLeft[i];
            //CurrentBarracks.PerTick = PerTick;
            //CurrentBarracks.TickCount = TickCount;
            //CurrentBarracks.Seconds = Seconds;
        }
            
    }

    void Update()
    {
        UpdateIndividual();
        for (int i = 0; i < 4; i++)
        {
            text[i].text = TurnsLeft[i] + "";
        }

        if (Quoe[0] == 0)
        {
            Quoe.RemoveAt(0);
            Quoe.Insert(3, 0);
            InventoryChange();
        }
        if (Quoe[1] == 0)
        {
            Quoe.RemoveAt(1);
            Quoe.Insert(3, 0);
            InventoryChange();
        }
        if (Quoe[2] == 0)
        {
            Quoe.RemoveAt(2);
            Quoe.Insert(3, 0);
            InventoryChange();
        }
        
    }

    public void InfantryAdd()
    {
        if (Currency.instance.Money > 3)
        {
            Currency.instance.ChangeCurrecy(-4);
            bool Got = false;
            for (int i = 0; i < 4; i++)
            {
                if (Quoe[i] == 0 && Got == false)
                {
                    //Items[i].AddSprite(Infantry);
                    CurrentBarracks.Quoe[i] = 2;
                    Quoe[i] = 2;
                    CurrentBarracks.TurnsLeft[i] = 3;
                    TurnsLeft[i] = 3;
                    Got = true;
                }
            }
            InventoryChange();
        }
    }

    public void WorkerAdd()
    {
        if (Currency.instance.Money > 2)
        {
            Currency.instance.ChangeCurrecy(-3);
            bool Got = false;
            for (int i = 0; i < 4; i++)
            {
                if (Quoe[i] == 0 && Got == false)
                {
                    //Items[i].AddSprite(Worker);
                    CurrentBarracks.Quoe[i] = 2;
                    CurrentBarracks.TurnsLeft[i] = 3;
                    TurnsLeft[i] = 2;
                    Quoe[i] = 2;
                    Got = true;
                }
            }
            InventoryChange();
        }
        
    }

    public void MechAdd()
    {
        if (Currency.instance.Money > 7)
        {
            Currency.instance.ChangeCurrecy(-8);
            bool Got = false;
            for (int i = 0; i < 4; i++)
            {
                if (Quoe[i] == 0 && Got == false)
                {
                    //Items[i].AddSprite(Worker);
                    CurrentBarracks.Quoe[i] = 3;
                    CurrentBarracks.TurnsLeft[i] = 7;
                    TurnsLeft[i] = 7;
                    Quoe[i] = 3;
                    Got = true;
                }
            }
            InventoryChange();
        }
    }
}
