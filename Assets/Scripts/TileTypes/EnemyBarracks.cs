using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class EnemyBarracks : MonoBehaviour
{
    [Header("TickInfo")]
    public float TickCount;
    public int Seconds;

    public GameObject Skeleton;

    public int SkeletonNum;

    public List<int2> neighbours = new List<int2>();

    public ClickableTile CT;

    public enum Factions { FactionA, FactionB, FactionC, FactionD, FactionE, FactionF };

    public Factions factions;

    public int FactionNum;

    public bool FullyClogged;

    public int SpawnsLeft;

    void Start()
    {
        SpawnEnemy(0, 100);
        //CT.map.TileTwoArray[CT.tileX, CT.tileY] = gameObject;
        //int ValueInArray = (CT.tileX * CT.map.RealHeight) + CT.tileY;
        //CT.map.KingdomSave.Insert(ValueInArray, FactionNum);
    }


    void Update()
    {
        CheckSurroundings();
        TickCount += Time.deltaTime * SpeedScript.instance.CurrentSpeed;
        if (TickCount >= Seconds)
        {
            TickCount = 0;
            SpawnEnemy(0, 100);
        }
    }

    public void CheckSurroundings()
    {
        SpawnsLeft = 0;
        for (int i = 0; i < neighbours.Count; i++)
        {
            bool Either = false;
            if (PeopleManager.instance.IntArraySingle.Contains(((CT.tileX + neighbours[i].x) * CT.map.RealHeight) + CT.tileY + neighbours[i].y))
            {
                FullyClogged = true;
                Either = true;
                
            }
            else if (HexGenerator.instance.tiles[CT.tileX + neighbours[i].x, CT.tileY + neighbours[i].y] == 3)
            {
                Either = true;
            }
            else if (HexGenerator.instance.tiles[CT.tileX + neighbours[i].x, CT.tileY + neighbours[i].y] == 0)
            {

            }
            else
            {
                //FullyClogged = false;
                SpawnsLeft += 1;
            }
        }
    }

    public void SpawnEnemy(int TriedCount, int CurrentSpawn)
    {
        //Debug.Log("I'M: " + FactionNum + "  AT: " + CurrentSpawn);
        int CurrentInVoid = CurrentSpawn;
        if (TriedCount > 20)
        {
            return;
        }
        
        if(CurrentInVoid == 100)
        {
            CurrentInVoid = 0;
        }
        int SpawnNum = CurrentInVoid;
        //neighbours[SpawnNum]
        int x = neighbours[SpawnNum].x + CT.tileX;
        int y = neighbours[SpawnNum].y + CT.tileY;
        //DO LATER

        SpawnsLeft = 0;
        
        if (PeopleManager.instance.IntArraySingle.Contains((x * CT.map.RealHeight) + y))
        {
            TriedCount += 1;
            CurrentInVoid += 1;
            SpawnEnemy(TriedCount, CurrentInVoid);
            return;
            //Debug.Log("CONTAINS");
        }

        //&& CanSpawnThisTile == true
        if (HexGenerator.instance.tiles[x, y] == 2 || HexGenerator.instance.tiles[x, y] == 1)
        {
            GameObject TileOBJ = GameObject.Find(x + " " + y);
            ClickableTile tile = TileOBJ.GetComponent<ClickableTile>();

            Vector3 Spawn = new Vector3(tile.WorldX, tile.WorldY + 0, 0);

            GameObject SkeletonOBJ = Instantiate(Skeleton, Spawn, Quaternion.identity);
            

            Unit unit = SkeletonOBJ.GetComponent<Unit>();

            unit.GridX = neighbours[SpawnNum].x;
            unit.GridY = neighbours[SpawnNum].y;

            unit.WorldX = tile.WorldX;
            unit.WorldY = tile.WorldY;

            PeopleManager.instance.All.Add(SkeletonOBJ.transform);
            Unit unitScript = SkeletonOBJ.GetComponent<Unit>();

            if (factions == Factions.FactionB)
            {
                unitScript.SetFaction(2);
                allegiances.instance.Lists[1].ArmyTransforms.Add(SkeletonOBJ.transform);
                //PeopleSpawner.instance.Army3.Add(SkeletonOBJ.transform);
                string Name = "Skeleton: " + SkeletonNum + " FactionB";
                SkeletonOBJ.name = Name;
            }
            else if (factions == Factions.FactionC)
            {
                unitScript.SetFaction(3);
                allegiances.instance.Lists[2].ArmyTransforms.Add(SkeletonOBJ.transform);
                //PeopleSpawner.instance.Army3.Add(SkeletonOBJ.transform);
                string Name = "Skeleton: " + SkeletonNum + " FactionC";
                SkeletonOBJ.name = Name;
            }
            else if (factions == Factions.FactionD)
            {
                unitScript.SetFaction(4);
                allegiances.instance.Lists[3].ArmyTransforms.Add(SkeletonOBJ.transform);
                //PeopleSpawner.instance.Army3.Add(SkeletonOBJ.transform);
                string Name = "Skeleton: " + SkeletonNum + " FactionD";
                SkeletonOBJ.name = Name;
            }
            else if (factions == Factions.FactionE)
            {
                unitScript.SetFaction(5);
                allegiances.instance.Lists[4].ArmyTransforms.Add(SkeletonOBJ.transform);
                //PeopleSpawner.instance.Army3.Add(SkeletonOBJ.transform);
                string Name = "Skeleton: " + SkeletonNum + " FactionE";
                SkeletonOBJ.name = Name;
            }
            else if (factions == Factions.FactionF)
            {
                unitScript.SetFaction(6);
                allegiances.instance.Lists[5].ArmyTransforms.Add(SkeletonOBJ.transform);
                //PeopleSpawner.instance.Army3.Add(SkeletonOBJ.transform);
                string Name = "Skeleton: " + SkeletonNum + " FactionF";
                SkeletonOBJ.name = Name;
            }
            SkeletonNum += 1;
        }
        else
        {
            TriedCount += 1;
            CurrentInVoid += 1;
            SpawnEnemy(TriedCount , CurrentInVoid);
        }     
    }

    public void SetFaction(int Faction)
    {
        if (Faction == 2)
        {
            factions = Factions.FactionB;
        }
        else if (Faction == 3)
        {
            factions = Factions.FactionC;
        }
        else if (Faction == 4)
        {
            factions = Factions.FactionD;
        }
        else if (Faction == 5)
        {
            factions = Factions.FactionE;
        }
        else if (Faction == 6)
        {
            factions = Factions.FactionF;
        }
        FactionNum = Faction;
    }
}
