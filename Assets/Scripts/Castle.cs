using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Castle : MonoBehaviour
{
    public string FortName;

    public List<Person> Inside = new List<Person>();
    public int Peasants;
    public int food;
    public int PeopleFoodDecay;

    public int2 MyPos;
    public List<int2> neighbours = new List<int2>();

    public List<int2> SpawnSpots = new List<int2>();
    public List<int2> CastleSpawns = new List<int2>();
    public List<Farm> Farms = new List<Farm>();

    public enum Factions { 
        FactionA = 0, 
        FactionB = 1, 
        FactionC = 2, 
        FactionD = 3, 
        FactionE = 4, 
        FactionF = 5};

    public Factions factions;

    public int FactionNum;

    public float Timer;
    public float PeasantSpawnTime;
    public int CurrentHealth;
    public int MaxHealth;

    public bool PlayerControlled;

    public int Knights;
    public int Archers;
    public int Calvalry;

    public bool First = true;

    public List<Unit> Armies = new List<Unit>();

    public void Start()
    {
        SendPeasantsNoDesinitation(FactionNum);
    }
    public void UpdateStats()
    {
        //add people
        float ToAdd = Peasants * 0.025f;
        Peasants += Mathf.RoundToInt(ToAdd);

        //convert people to infantry
        //float ToAdd2 = Peasants * 0.01f;
        //infantry += Mathf.RoundToInt(ToAdd2);
        //Peasants -= Mathf.RoundToInt(ToAdd2);

        //food -= Peasants / PeopleFoodDecay;
    }

    public void SendArmy(int Knight, int Archer, int Calval, bool SendOnSpawn, int2 Objective)
    {
        Knights -= Knight;
        Archers -= Archer;
        Calvalry -= Calval;
        //Debug.Log("PT1");
        int2 side = FindNeighbour();
        //Debug.Log("PT2.2");
        //get leader
        Person bestFit = Inside[0];
       // Debug.Log("PT2.3");
        for (int i = 0; i < Inside.Count; i++)
        {
            if (Inside[i].Command > bestFit.Command)
            {
                bestFit = Inside[i];
            }
        }
        //Debug.Log("PT2");
        PeopleSpawner.instance.SpawnArmy(Knight, Archer, Calval, bestFit, FactionNum, 0, side, this, SendOnSpawn, Objective);
       // Debug.Log("PT3");
    }

    public void AddPeasantsToCastle(int ToAdd)
    {
        Peasants += ToAdd;
    }

    public int2 FindNeighbour()
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            bool Skip = false;

            int x = neighbours[i].x + MyPos.x;
            int y = neighbours[i].y + MyPos.y;

            if (x < 0 || y < 0)
            {
                Skip = true;
            }
            //Debug.Log(x + "  " + y);
            int2 ToAdd = new int2(x, y);
            if (SpawnSpots.Count > 0 && Skip == false)
            {
                //if not occupied
                //Debug.Log("try1");
                if (PeopleManager.instance.BothInt.Contains(ToAdd))
                {
                    //Debug.Log("try2.1");
                }
                else
                {
                    //Debug.Log("try2");
                    if (SpawnSpots.Contains(ToAdd))
                    {
                        //Debug.Log("contains");
                        return ToAdd;
                    }
                    else
                    {
                        //Debug.Log("add2");
                        //Debug.Log("try3");
                        if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2 || HexGenerator.instance.tiles[x, y] == 5)
                        {
                            //Debug.Log("add  " + HexGenerator.instance.tiles[x, y]);
                            return ToAdd;
                        }
                    }
                }



            }
            else if(Skip == false)
            {
                //Debug.Log("no-spawns");
                //Debug.Log("add1");
                if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2 || HexGenerator.instance.tiles[x, y] == 5)
                {
                    //Debug.Log("add  " + HexGenerator.instance.tiles[x, y]);
                    return ToAdd;
                }
            }
            
        }
        Debug.LogError("Castle.FindNeighbour Returned null");
        return new int2(0, 0);
    }

    public void SendPeasantsNoDesinitation(int Number)
    {
        int2 Spawn = FindNeighbour();
        int2 End = new int2(0, 0);
        PeopleSpawner.instance.SpawnPeasants(Number, Spawn, End, this, false);
    }

    public void SendPeople(int Number, int ToX, int ToY)
    {
        int2 Spawn = FindNeighbour();
        int2 End = new int2(ToX, ToY);
        int x = Spawn.x;
        int y = Spawn.y;
        //DO LATER
        PeopleSpawner.instance.SpawnPeasants(Number, Spawn, End, this, true);
    }

    void Update()
    {
        if(First == true)
        {
            AdjustSpawns();
            StartFarmSequence();
            //StartFarmSequence();
                
            First = false;
        }

        Timer += Time.deltaTime * SpeedScript.instance.CurrentSpeed;
        if (Timer > PeasantSpawnTime)
        {
            UpdateStats();
            Timer = 0;
        }
        if (Peasants > 110)
        {
            Peasants -= 20;
            //StartFarmSequence();
            if (PlayerControlled == false)
            {
                //SendPeople(10, SpawnSpots[0].x, SpawnSpots[0].y);
            }
        }
    }

    public void RemoveFromCastleWithDestination(Person PersonToAdd, int XTile, int YTile, int ObjectiveFaction)
    {
        int2 Spawn = FindNeighbour();
        int IntX = Spawn.x;
        int IntY = Spawn.y;

        PeopleSpawner.instance.SpawnPerson(MyPos.x, MyPos.y, PersonToAdd, true, XTile, YTile, this, ObjectiveFaction);

        CastleDisplay.instance.RemovePanel(PersonToAdd);
        CastleDisplay.instance.ChangeLists();
    }

    public void RemoveFromCastle(Person PersonToAdd)
    {
        int2 Spawn = FindNeighbour();
        int IntX = Spawn.x;
        int IntY = Spawn.y;

        PeopleSpawner.instance.SpawnPerson(MyPos.x, MyPos.y, PersonToAdd, false, 0 ,0, null, 0);

        CastleDisplay.instance.RemovePanel(PersonToAdd);
        CastleDisplay.instance.ChangeLists();
    }

    public void AddToCastle(Person PersonToAdd, bool Lists)
    {
        Inside.Add(PersonToAdd);
        if (Lists == true)
        {
            CastleDisplay.instance.ChangeLists();
        }
    }

    public void SetStartStats()
    {
        //Debug.Log("start");
        Peasants = 100;
        CurrentHealth = MaxHealth;
        float infantryFloat = allegiances.instance.Lists[FactionNum].Land / 5;
        //infantry = (int)infantryFloat;
    }

    public void AdjustSpawns()
    {
        //for castle tiles
        SpawnSpots.Clear();
        CastleSpawns.Clear();
        for (int i = 0; i < neighbours.Count; i++)
        {
            bool Skip = false;
            
            int x = neighbours[i].x + MyPos.x;
            int y = neighbours[i].y + MyPos.y;

            if (x < 0 && y < 0)
            {
                Skip = true;
            }
            int2 ToAdd = new int2(x, y);
            if (x < HexGenerator.instance.RealWidth && x > 0 && y < HexGenerator.instance.RealHeight && y > 0)
            {
                if (SpawnSpots.Count > 0 && Skip == false)
                {
                    if (SpawnSpots.Contains(ToAdd))
                    {

                    }
                    else
                    {
                        if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2 && Skip == false)
                        {
                            SpawnSpots.Add(ToAdd);
                        }
                        if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2 || HexGenerator.instance.tiles[x, y] == 5 && Skip == false)
                        {
                            CastleSpawns.Add(ToAdd);
                        }
                    }
                }
                else
                {
                    //Debug.Log("add1");
                    if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2 && Skip == false)
                    {
                        //Debug.Log("add2");
                        SpawnSpots.Add(ToAdd);
                    }
                    if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2 || HexGenerator.instance.tiles[x, y] == 6 || HexGenerator.instance.tiles[x, y] == 5 && Skip == false)
                    {
                        CastleSpawns.Add(ToAdd);
                    }
                }
            }
            
            
        }
        for (int i = 0; i < Farms.Count; i++)
        {
            for (int j = 0; j < Farms[i].SpawnSpots.Count; j++)
            {
                bool Skip = false;
                int x = Farms[i].SpawnSpots[j].x + Farms[i].CT.tileX;
                int y = Farms[i].SpawnSpots[j].y + Farms[i].CT.tileY;

                if (x < 0 && y < 0)
                {
                    Skip = true;
                }

                int2 ToAdd = new int2(x, y);
                //Debug.Log(ToAdd);
                if (SpawnSpots.Count > 0 && Skip == false)
                {
                    if (SpawnSpots.Contains(ToAdd))
                    {
                        //Debug.Log("fail");
                    }
                    else
                    {
                        //Debug.Log(HexGenerator.instance.tiles[x, y] + "  try");
                        if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2)
                        {
                            SpawnSpots.Add(ToAdd);
                            //Debug.Log("add");
                        }
                    }
                }
                else
                {
                    //Debug.Log(x + "  " + y);
                    if (HexGenerator.instance.tiles[x, y] == 1 || HexGenerator.instance.tiles[x, y] == 2 && Skip == false)
                    {
                        //Debug.Log("add");
                        SpawnSpots.Add(ToAdd);
                    }
                }
            }
        }
    }

    public void SetFaction(int Faction)
    {
        //int val = (int)someEnumValue
        factions = (Factions)Faction;
        FactionNum = Faction;
    }

    public void StartFarmSequence()
    {
        /*
        int Choosen = Random.Range(0, SpawnSpots.Count);
        int x = SpawnSpots[Choosen].x;
        int y = SpawnSpots[Choosen].y;
        AddFarm(x, y);
        AdjustSpawns();
        */
    }

    public void AddFarm(int x, int y)
    { 
        HexGenerator.instance.TM.ReplaceWithFarm(x, y, FactionNum, this);
    }

    public void AddFood(int Food)
    {
        food += Food;
    }
}
