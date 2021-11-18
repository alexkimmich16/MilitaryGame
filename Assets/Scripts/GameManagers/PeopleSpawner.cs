using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class PeopleSpawner : MonoBehaviour
{
    #region Singleton

    public static PeopleSpawner instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject skeleton;
    public GameObject Worker;
    public GameObject Infantry;
    public GameObject Mech;
    public GameObject PersonObj;
    public GameObject PeasantGroup;
    public GameObject Army;

    public int MaxPeasantsGroup;

    [HideInInspector]
    public Transform HitObj;
    public float PerTick;
    public float TickCount;
    public int Seconds;

    [HideInInspector]
    public int SpawnZone;

    public HexGenerator Map;

    public void SpawnArmy(int Knights, int Archers, int Calvalry, Person leader, int Faction, int Food, int2 Spawn, Castle castle, bool Send, int2 Objective)
    {
        //Debug.Log("army");
        
        Vector2 position = Map.GetPosition(Spawn.x, Spawn.y);
        GameObject Spawned = (GameObject)Instantiate(Army, position, Quaternion.identity);
        Spawned.GetComponent<Unit>().WorldX = position.x;
        Spawned.GetComponent<Unit>().WorldY = position.y;
        Spawned.GetComponent<Unit>().GridX = Spawn.x;
        Spawned.GetComponent<Unit>().GridY = Spawn.y;
        Spawned.GetComponent<Unit>().FactionNum = Faction;

        Spawned.GetComponent<ArmyPack>().SetMen(Knights, Archers, Calvalry);
        Spawned.GetComponent<ArmyPack>().Leader = leader;

        PeopleManager.instance.AddToList(Spawned.transform, Spawn.x, Spawn.y);
        allegiances.instance.Lists[Faction].ArmyTransforms.Add(Spawned.transform);

        castle.Armies.Add(Spawned.GetComponent<Unit>());

        if (Faction == 0)
        {
            Spawned.GetComponent<ArmyPack>().Friendly = true;
        }
        else
        {
            Spawned.GetComponent<ArmyPack>().Friendly = false;
        }
        
        if (Send == true)
        {
            List<int2> Path = Map.PM.FindPath(Spawn, Objective, castle.FactionNum, castle.FactionNum, false);
            Spawned.GetComponent<Unit>().AddToPath(Path);
            //Objective
        }
        
    }

    public void SpawnPerson(int StartX, int StartY, Person person, bool HasPath, int XTile, int YTile, Castle castle, int FactionToMoveTo)
    {
        /*
        Vector2 position = Map.GetPosition(StartX, StartY);
        GameObject SpawnedPerson = (GameObject)Instantiate(PersonObj, position, Quaternion.identity);
        
        SpawnedPerson.GetComponent<WalkingPerson>().currentPerson = person;
        SpawnedPerson.GetComponent<Unit>().WorldX = position.x;
        SpawnedPerson.GetComponent<Unit>().WorldY = position.y;

        SpawnedPerson.GetComponent<Unit>().GridX = StartX;
        SpawnedPerson.GetComponent<Unit>().GridY = StartY;
        SpawnedPerson.GetComponent<Unit>().FactionNum = person.Faction;
        PeopleManager.instance.AddToList(SpawnedPerson.transform, StartX, StartY);

        allegiances.instance.Lists[0].ArmyTransforms.Add(SpawnedPerson.transform);

        string Name = person.FullName;
        SpawnedPerson.transform.name = Name;

        if(HasPath == true)
        {
            
            ClickableTile CT = Map.TileTwoArray[XTile, YTile].GetComponent<ClickableTile>();
            //CT.GeneratePath(SpawnedPerson, person.Faction);
            int2 start = new int2(StartX, StartY);
            int2 end = new int2(XTile, YTile);

            List<int> Friendly = new List<int>();

            //type is faction
            for (int i = 0; i < allegiances.instance.Lists.Count; i++)
            {
                if (allegiances.instance.Lists[person.Faction].State[i] == 1)
                {
                    Friendly.Add(i);
                }
            }
            if (Map.PM.PathExists(start, end, true, person.Faction) == true)
            {
                Map.PM.FindPath(start, end, SpawnedPerson, person.Faction, FactionToMoveTo, castle, true);
            }
            else if (Map.PM.AlteredPathExists(start, end, person.Faction) != 7)
            {
                int Num = Map.PM.AlteredPathExists(start, end, person.Faction);
                Friendly.Add(Num);
                Map.PM.FindPath(start, end, SpawnedPerson, person.Faction, FactionToMoveTo, castle, true);
            }
            else
            {
                Debug.Log("didn't find path");
            }
            //20 is objective
            //Debug.Log(Map.PM.AlteredPathExists(start, end, person.Faction) + " found path");
            //Debug.Log(end.x + " " + end.y + " endboth2");
            Map.PM.FindPath(start, end, SpawnedPerson, person.Faction, FactionToMoveTo, castle, true);
            
        }
        */
        //find reason they cannot find a path
    }

    public void SpawnPeasants(int Number, int2 spawn, int2 To, Castle castle, bool UseTo)
    {
        Vector2 Start = Map.GetPosition(spawn.x, spawn.y);
        Vector2 End = Map.GetPosition(To.x, To.y);
        GameObject Spawned = (GameObject)Instantiate(PeasantGroup, Start, Quaternion.identity);
        Spawned.GetComponent<Unit>().WorldX = Start.x;
        Spawned.GetComponent<Unit>().WorldY = Start.y;
        Spawned.GetComponent<Unit>().GridX = spawn.x;
        Spawned.GetComponent<Unit>().GridY = spawn.y;
        Spawned.GetComponent<Unit>().FactionNum = castle.FactionNum;

        Spawned.GetComponent<PeasantGroup>().ObjectiveX = To.x;
        Spawned.GetComponent<PeasantGroup>().ObjectiveY = To.y;
        Spawned.GetComponent<PeasantGroup>().castle = castle;
        PeopleManager.instance.AddToList(Spawned.transform, spawn.x, spawn.y);
        allegiances.instance.Lists[0].ArmyTransforms.Add(Spawned.transform);
        if (UseTo == true)
        {
            List<int2> Path = Map.PM.FindPath(spawn, To, castle.FactionNum, castle.FactionNum, false);
            Spawned.GetComponent<Unit>().AddToPath(Path);
            //Map.PM.GeneratePath(Spawned, 0);
        }
        
    }
    /*
    // do Manned
    public void SpawnInfantry(int XSpawn, int YSpawn, int Faction, int NumberInside)
    {
        ClickableTile tile = HexGenerator.instance.TileTwoArray[XSpawn, YSpawn].GetComponent<ClickableTile>();

        GameObject Spawned = (GameObject)Instantiate(Infantry, new Vector2(tile.WorldX, tile.WorldY), Quaternion.identity);
        Spawned.GetComponent<Unit>().WorldX = tile.WorldX;
        Spawned.GetComponent<Unit>().WorldY = tile.WorldY;
        Spawned.GetComponent<Unit>().GridX = tile.tileX;
        Spawned.GetComponent<Unit>().GridY = tile.tileY;
        Spawned.GetComponent<Unit>().FactionNum = Faction;

        Spawned.GetComponent<Infantry>().AmountInside = NumberInside;
        PeopleManager.instance.AddToList(Spawned.transform, XSpawn, YSpawn);

        allegiances.instance.Lists[Faction].ArmyTransforms.Add(Spawned.transform);
        allegiances.instance.Lists[Faction].InfantryTransforms.Add(Spawned.transform);
    }
    public void SpawnFriendly(Transform trans)
    {
        HitObj = trans;
        for (int b = 0; b < InfantryNum; b++)
        {
            GameObject SpawnedInfantry = (GameObject)Instantiate(Infantry, trans.position, Quaternion.identity);
            SpawnedInfantry.GetComponent<Unit>().WorldX = trans.position.x;
            SpawnedInfantry.GetComponent<Unit>().WorldY = trans.position.y;
            Vector3 friend = new Vector3(SpawnedInfantry.transform.position.x, SpawnedInfantry.transform.position.y, 0);
            PeopleManager.instance.AddToList(SpawnedInfantry.transform, 2, 2);

            allegiances.instance.Lists[0].ArmyTransforms.Add(SpawnedInfantry.transform);
        }
        for (int b = 0; b < WorkersNum; b++)
        {
            GameObject SpawnedWorker = (GameObject)Instantiate(Worker, trans.position, Quaternion.identity);

            SpawnedWorker.GetComponent<Unit>().WorldX = trans.position.x;
            SpawnedWorker.GetComponent<Unit>().WorldY = trans.position.y;
            Vector3 friend = new Vector3(SpawnedWorker.transform.position.x, SpawnedWorker.transform.position.y, 0);
            PeopleManager.instance.AddToList(SpawnedWorker.transform, 2,2);
            allegiances.instance.Lists[0].ArmyTransforms.Add(SpawnedWorker.transform);
        }
        for (int b = 0; b < MechNum; b++)
        {
            GameObject SpawnedWorker = (GameObject)Instantiate(Mech, trans.position, Quaternion.identity);

            SpawnedWorker.GetComponent<Unit>().WorldX = trans.position.x;
            SpawnedWorker.GetComponent<Unit>().WorldY = trans.position.y;
            Vector3 friend = new Vector3(SpawnedWorker.transform.position.x, SpawnedWorker.transform.position.y, 0);
            PeopleManager.instance.AddToList(SpawnedWorker.transform, 2, 2);
            allegiances.instance.Lists[0].ArmyTransforms.Add(SpawnedWorker.transform);
        }
    }
    */
}
