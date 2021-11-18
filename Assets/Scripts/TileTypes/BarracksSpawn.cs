using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarracksSpawn : MonoBehaviour
{
    //1 = worker
    //2 = Infantry

    public List<int> Quoe = new List<int>(4);
    public List<int> TurnsLeft = new List<int>();

    public int SpawnNumber;

    public int X;
    public int Y;

    public bool IsSelected;
    public bool StartBool;

    public BarracksManager Barracks;

    public float PerTick;
    public float TickCount;
    public int Seconds;

    void Start()
    {
        Barracks = BarracksManager.instance;
        StartBool = true;
        PerTick = 1 / TurnGenerator.instance.FPS;
        TurnGenerator.OnTick += delegate (object sender, TurnGenerator.OnTickEventArgs e)
        {
            TickUpdate();
        };
    }

    void TickUpdate()
    {
        if(IsSelected == false)
        {
            TickCount += PerTick;
            if (TickCount >= Seconds)
            {
                TurnsLeft[0] -= 1;
                TickCount = 0;
                if (TurnsLeft[0] == 0)
                {
                    SpawnNumber = Quoe[0];
                    Quoe[0] = 0;
                    TurnsLeft.RemoveAt(0);
                    TurnsLeft.Insert(3, 0);
                    Spawn(SpawnNumber);
                }
            }
        }
        
        
    }

    void Update()
    {
        if (IsSelected == true && StartBool == true)
        {
            StartBool = false;
            Barracks.CurrentBarracks = this;
            for (int i = 0; i < 4; i++)
            {
                Barracks.Quoe[i] = Quoe[i];
                Barracks.TurnsLeft[i] = TurnsLeft[i];
                //Barracks.PerTick = PerTick;
                //Barracks.TickCount = TickCount;
                //Barracks.Seconds = Seconds;
            }
        }
        if (Barracks.CurrentBarracks != this)
        {
            IsSelected = false;
        }
        if (IsSelected == false)
        {
            StartBool = true;
        }



        if (Quoe[0] == 0)
        {
            Quoe.RemoveAt(0);
            Quoe.Insert(3, 0);
        }
        if (Quoe[1] == 0)
        {
            Quoe.RemoveAt(1);
            Quoe.Insert(3, 0);
        }
        if (Quoe[2] == 0)
        {
            Quoe.RemoveAt(2);
            Quoe.Insert(3, 0);
        }

    }

    public void Spawn(int SpawnNum)
    {
        /*
        Debug.Log(SpawnNum);
        string IdentifyNumber = X + " " + Y;
        GameObject foundObject = GameObject.Find(IdentifyNumber);
        //float x = foundObject.GetComponent<ClickableTile>().WorldX;
        //float y = foundObject.GetComponent<ClickableTile>().WorldY;
        if(SpawnNum == 3)
        {
            GameObject SpawnedInfantry = Instantiate(PeopleSpawner.instance.Mech, new Vector3(x, y, 0), Quaternion.identity);
            SpawnedInfantry.GetComponent<Unit>().WorldX = foundObject.transform.position.x;
            SpawnedInfantry.GetComponent<Unit>().WorldY = foundObject.transform.position.y;

            SpawnedInfantry.GetComponent<Unit>().GridX = X;
            SpawnedInfantry.GetComponent<Unit>().GridY = Y;
        }
        else if (SpawnNum == 2)
        {
            GameObject SpawnedInfantry = Instantiate(PeopleSpawner.instance.Infantry, new Vector3(x,y,0), Quaternion.identity);
            SpawnedInfantry.GetComponent<Unit>().WorldX = foundObject.transform.position.x;
            SpawnedInfantry.GetComponent<Unit>().WorldY = foundObject.transform.position.y;

            SpawnedInfantry.GetComponent<Unit>().GridX = X;
            SpawnedInfantry.GetComponent<Unit>().GridY = Y;
        }
        else if (SpawnNum == 1)
        {
            GameObject SpawnedInfantry = Instantiate(PeopleSpawner.instance.Worker, new Vector3(x, y, 0), Quaternion.identity);
            SpawnedInfantry.GetComponent<Unit>().WorldX = foundObject.transform.position.x;
            SpawnedInfantry.GetComponent<Unit>().WorldY = foundObject.transform.position.y;

            SpawnedInfantry.GetComponent<Unit>().GridX = X;
            SpawnedInfantry.GetComponent<Unit>().GridY = Y;
        }
        */
    }

    
    public void NextTurn()
    {
        TurnsLeft[0] -= 1;
        if (TurnsLeft[0] == 0)
        {
            SpawnNumber = Quoe[0];
            Quoe[0] = 0;
            TurnsLeft.RemoveAt(0);
            TurnsLeft.Insert(3, 0);
            CreatePerson(SpawnNumber);
        }
    }

    public void CreatePerson(int SpawnNumber)
    {
        if (SpawnNumber == 1)
        {
            //GameObject SpawnedInfantry = (GameObject)Instantiate(Infantry, trans.position, Quaternion.identity);
            //SpawnedInfantry.GetComponent<Unit>().WorldX = trans.position.x;
            //SpawnedInfantry.GetComponent<Unit>().WorldY = trans.position.y;
        }
        else if (SpawnNumber == 2)
        {

        }
    }

    public void InfantryAdd()
    {
        bool Got = false;
        for (int i = 0; i < 4; i++)
        {
            if (Quoe[i] == 0 && Got == false)
            {
                //Items[i].AddSprite(Infantry);
                Quoe[i] = 2;
                TurnsLeft[i] = 3;
                Got = true;
            }
        }


    }
    public void WorkerAdd()
    {
        bool Got = false;
        for (int i = 0; i < 4; i++)
        {
            if (Quoe[i] == 0 && Got == false)
            {
                //Items[i].AddSprite(Worker);
                TurnsLeft[i] = 2;
                Quoe[i] = 1;
                Got = true;
            }
        }
    }
}
