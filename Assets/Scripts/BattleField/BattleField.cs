using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

public class BattleField : MonoBehaviour
{
    #region Singleton

    public static BattleField instance;

    void Awake()
    {
        instance = this;
    }

    #endregion


    //zoom into lower field
    //stop time during battle?
    //horses move faster
    //include weapon triangle \
    //horses > archer > knights, but knights > hourses

    //when reinforcements arrive, drop them onto the board in cool fashion
    //needs to be good, (a key event in movie and tv history and is a great drama)

    private HexGenerator map;
    public Tilemap FieldMap;
    private List<BattleFieldTile> AroundTiles = new List<BattleFieldTile>();
    private List<BattleFieldTile> BaseTiles = new List<BattleFieldTile>();
    private int MainChange = 1;
    private int SmallChange = 1;
    public int2 MapSize;

    public List<GameObject> BattleUnits = new List<GameObject>();
    public List<int> EachMaxGroup = new List<int>();
    //public int KnightGroup, ArcherGroup, CalvalryGroup;

    public int[,] tilesTypes;

    public List<int2> PlayerSpawns = new List<int2>();

    //it could generate it right off the bat, and just access and change
    // Start is called before the first frame update
    public bool WalkAble(int x, int y)
    {
        if (tilesTypes[x,y] == 1 || tilesTypes[x, y] == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int CheckTile(int x, int y)
    {
        return (tilesTypes[x, y]);
    }
    public Vector2 GetPosition(int x, int y)
    {
        Vector3 Maxplace = FieldMap.CellToWorld(new Vector3Int(x, y, 0));
        return Maxplace;
    }

    public void SpawnPeople(List<int> FriendCounts, List<int> EnemyCounts)
    {
        //10x10 so player spawns
        //check if odd first
        int PlayerStacks = 0;
        int EnemyStacks = 0;
        for (int i = 0; i < FriendCounts.Count; i++)
        {
            while (FriendCounts[i] > EachMaxGroup[i])
            {
                PlayerStacks += 1;
                FriendCounts[i] -= EachMaxGroup[i];
            }

            for (int j = 0; j < PlayerStacks; j++)
            {
                //spawn at playerspawnsi
                Spawn(EachMaxGroup[i], i, PlayerSpawns[j], true);
            }

            if (FriendCounts[i] != 0)
            {
                Spawn(FriendCounts[i], i, PlayerSpawns[PlayerStacks + 1], true);
            }
        }

        
    }

    public void Spawn(int Number, int Type, int2 Pos, bool Friendly)
    {
        Vector2 position = GetPosition(Pos.x, Pos.y);
        
        GameObject Spawned = (GameObject)Instantiate(BattleUnits[Number], position, Quaternion.identity);
        Spawned.GetComponent<BattleUnit>().World = position;
        Spawned.GetComponent<BattleUnit>().Grid = Pos;
        Spawned.GetComponent<BattleUnit>().Friendly = Friendly;
        //Spawned.GetComponent<BattleUnit>().FactionNum = Faction;
        if (Number == 0)
        {
            
        }
        else if (Number == 1)
        {
            //Archer
        }
        else if (Number == 2)
        {
            //Calvalry
        }
    }

    //where to spawn void
    //pathfinding for this type
    //deviiding into biggest size for troops
    //speed constent reguardless of size of pack
    //damage is based on the amount of people inside
    //player can decide a lower number and its automatically named "delta quad" or "alpha quad" for stratagy


    void Start()
    {
        map = HexGenerator.instance;

        tilesTypes = new int[MapSize.x, MapSize.y];
    }

    public void UnfunctionalTileSpawn()
    {
        for (int i = 0; i < MapSize.x; i++)
        {
            for (int j = 0; j < MapSize.y; j++)
            {
                int Type = 1;
                tilesTypes[i, j] = Type;
                FieldMap.SetTile(new Vector3Int(i, j, 0), map.tileTypes[Type].tile);
            }
        }
    }

    public void RecieveTileInfo(List<int2> Spots)
    {
        //Debug.Log("generate1");
        //set list size
        AroundTiles.Clear();
        BaseTiles.Clear();
        for (int i = 0; i < Spots.Count; i++)
        {
            BaseTiles.Add(new BattleFieldTile());
            for (int j = 0; j < 6; j++)
            {
                AroundTiles.Add(new BattleFieldTile());
                AroundTiles[AroundTiles.Count - 1].Parent = i;
            }
        }

        //BaseTiles = new List<BattleFieldTile>(Spots.Count);
        //AroundTiles = new List<BattleFieldTile>(Spots.Count * 6);

        BaseTiles[0].LocalGrid = new int2(0, 0);

       // Debug.Log("generate1.1");

        int neighbourIndex = 0;
        BaseTiles[0].WorldGrid = new int2(Spots[0].x, Spots[0].y);
        BaseTiles[1].WorldGrid = new int2(Spots[1].x, Spots[1].y);

        BaseTiles[0].TileType = map.CheckTile(Spots[0].x, Spots[0].y);
        BaseTiles[1].TileType = map.CheckTile(Spots[1].x, Spots[1].y);

        BaseTiles[0].LocalGrid = new int2(0, 0);
        

        int2 LocalStart = new int2(Spots[1].x - Spots[0].x, Spots[1].y - Spots[0].y);

        List<int2> AroundSecond = GetSurrounding(LocalStart);
        //Debug.Log("generate1.2");
        for (int i = 0; i < AroundSecond.Count; i++)
        {
            if (AroundSecond[i].x == LocalStart.x && AroundSecond[i].y == LocalStart.y)
            {
                neighbourIndex = i;
            }
        }
        //Debug.Log("generate2");
        //local 2 offset
        for (int i = 0; i < MainChange; i++)
        {
            if (LocalStart.y % 2 == 1)
            {
                LocalStart.x += map.neighbourOffsetArrayEven[neighbourIndex].x;
                LocalStart.y += map.neighbourOffsetArrayEven[neighbourIndex].y;
            }
            else
            {
                LocalStart.x += map.neighbourOffsetArrayOdd[neighbourIndex].x;
                LocalStart.y += map.neighbourOffsetArrayOdd[neighbourIndex].y;
            }
        }
        BaseTiles[1].LocalGrid = LocalStart;
        //Debug.Log("generate3");

        //set tiletipes
        for (int i = 0; i < BaseTiles.Count; i++)
        {
            //Debug.Log("pt1");
            List<int2> Around = GetSurrounding(BaseTiles[i].WorldGrid);
            //Debug.Log("pt2");
            for (int j = 0; j < Around.Count; j++)
            {
                //Debug.Log("pt3");
                int Index = (i * 6) + j;
                //Debug.Log(i + " " + j);
                AroundTiles[Index].WorldGrid = Around[j];
                //Debug.Log("pt4");
                AroundTiles[Index].TileType = map.CheckTile(Around[j].x, Around[j].y);
            }
        }
        //Debug.Log("generate4");

        //get grid
        for (int i = 0; i < BaseTiles.Count; i++)
        {
            List<int2> Around = GetSurrounding(BaseTiles[i].LocalGrid);
            for (int j = 0; j < Around.Count; j++)
            {
                int Index = (i * 6) + j;
                AroundTiles[Index].LocalGrid = Around[j];
            }
        }
        // Debug.Log("generate5");


        //extend surrounding outwards on local
        for (int i = 0; i < AroundTiles.Count; i++)
        {
            //Debug.Log("pt1");
            List<int2> Around = GetSurrounding(AroundTiles[i].LocalGrid);
            
            int neighbourI = 0;
           // Debug.Log("pt2");
            for (int j = 0; j < Around.Count; j++)
            {
                int2 Current = new int2(Around[j].x - BaseTiles[AroundTiles[i].Parent].LocalGrid.x, Around[j].y - BaseTiles[AroundTiles[i].Parent].LocalGrid.y);
                if (Around[j].x == Current.x && Around[j].y == Current.y)
                {
                    neighbourI = i;
                }
            }
           // Debug.Log("pt3");
            int2 NeighbourFound = new int2(Around[neighbourI].x, Around[neighbourI].y);
            for (int j = 0; j < SmallChange; j++)
            {
                if (NeighbourFound.y % 2 == 1)
                {
                    NeighbourFound.x += map.neighbourOffsetArrayEven[neighbourI].x;
                    NeighbourFound.y += map.neighbourOffsetArrayEven[neighbourI].y;
                }
                else
                {
                    NeighbourFound.x += map.neighbourOffsetArrayOdd[neighbourI].x;
                    NeighbourFound.y += map.neighbourOffsetArrayOdd[neighbourI].y;
                }
            }
            AroundTiles[i].LocalGrid = new int2(NeighbourFound.x, NeighbourFound.y);
            //Debug.Log("pt4");
        }

        List<BattleFieldTile> AllTiles = new List<BattleFieldTile>();
        for (int i = 0; i < AroundTiles.Count; i++)
        {
            //Debug.Log(AroundTiles[i].LocalGrid);
            AllTiles.Add(AroundTiles[i]);
        }

        for (int i = 0; i < BaseTiles.Count; i++)
        {
            //Debug.Log(BaseTiles[i].LocalGrid);
            AllTiles.Add(BaseTiles[i]);
        }

        //BaseTiles
        //Debug.Log("generate4");
        GenerateBattleField(AllTiles);
        //pass in randomised tiles, that are already extended
       
    }

    public void GenerateBattleField(List<BattleFieldTile> Spots)
    {
        ///try other with extended tiles
        for (int i = 0; i < Spots.Count; i++)
        {
            //get ajusted spot, then get neighbours and create tiles
            List<int2> Around = GetSurrounding(Spots[i].LocalGrid);
            for (int j = 0; j < Around.Count + 1; j++)
            {
                if (j == 6)
                {
                    int Type = Spots[i].TileType;
                    FieldMap.SetTile(new Vector3Int(Spots[i].LocalGrid.x, Spots[i].LocalGrid.y, 0), map.tileTypes[Type].tile);
                }
                else
                {
                    int Type = Spots[i].TileType;
                    FieldMap.SetTile(new Vector3Int(Around[j].x, Around[j].y, 0), map.tileTypes[Type].tile);
                }
                
                
            }
        }
    }

    public List<int2> GetSurrounding(int2 Middle)
    {
        List<int2> CurrentOffset = new List<int2>();
       // Debug.Log("Surround1");
        if (Middle.y % 2 == 1)
        {
            CurrentOffset = map.neighbourOffsetArrayEven;
        }
        else
        {
            CurrentOffset = map.neighbourOffsetArrayOdd;
        }
        //Debug.Log("Surround2");
        List<int2> Tiles = new List<int2>();
        Debug.Log(CurrentOffset.Count);
        for (int i = 0; i < CurrentOffset.Count; i++)
        {
            int2 Neighbour = new int2(CurrentOffset[i].x + Middle.x, CurrentOffset[i].y + Middle.y);
            Tiles.Add(Neighbour);
        }
        //Debug.Log("Surround3");
        return Tiles;
    }
}
