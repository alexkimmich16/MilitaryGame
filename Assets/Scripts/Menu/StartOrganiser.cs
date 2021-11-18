using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartOrganiser : MonoBehaviour
{
    #region Singleton
    public static StartOrganiser instance;
    void Awake(){instance = this;}
    #endregion

    private HexGenerator Map;
    private NameAssigner NameGen;
    //private NameAssigner NameGen;

    public static bool SelectedCastle = false;

    public bool Started = false;

    public int MinBoarderPerCastle;

    public CameraBounds bounds;

    public Vector2 SurroundingBoarder;

    List<Color> colorList = new List<Color>()
     {
         Color.white,
         Color.green,
         Color.blue,
         Color.red,
         Color.gray,
         Color.cyan,
         Color.magenta,
     };

    void Start()
    {
        MapAndKingdom();
        SetKingdom();
        Barracks();
        CreateNames();
        SetBounds();
        CreateMenus();
    }
    void CreateMenus()
    {
        PeopleInspector.instance.SetMenu();
    }
    void SetBounds()
    {
        Vector3Int coordinate = new Vector3Int(Map.RealWidth, Map.RealHeight, 0);
        Vector3 TilePos = Map.mountainMap.CellToWorld(coordinate);
        //Debug.Log("Pos: " + TilePos);
        //Debug.Log("Pos: " + coordinate);
        
        //Debug.Log("Should be: " + 90);

        bounds.pointa.x = TilePos.x + SurroundingBoarder.x;
        bounds.pointa.y = TilePos.y + SurroundingBoarder.y;

        //Debug.Log("Final: " + bounds.pointa.x);
    }

    void SetKingdom()
    {
        //Debug.Log("kingdom");
        KingdomAssigner.instance.GetSpots();
        for (int x = 0; x < allegiances.instance.Lists.Count; x++)
        {
            allegiances.instance.Lists[x].Land = 0;
        }

        for (int x = 0; x < Map.RealWidth; x++)
        {
            for (int y = 0; y < Map.RealHeight; y++)
            {
                float minDist = Mathf.Infinity;
                int Kingdom = 0;

                for (int i = 0; i < KingdomAssigner.instance.Spots.Count; i++)
                {
                    Vector2 pos = Map.GetPosition(x, y);
                    Vector2 KingdomDot = new Vector2(KingdomAssigner.instance.Spots[i].x, KingdomAssigner.instance.Spots[i].y);
                    float dist = Vector2.Distance(pos, KingdomDot);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        Kingdom = i;
                    }
                    
                    
                }
                int Type = Map.CheckTile(x, y);
                if (Type == 1 || Type == 2)
                {
                    allegiances.instance.Lists[Kingdom].Land += 1;
                }

                if (GameControl.instance.ColorTiles == true)
                {
                    Map.ChangeColor(x, y, colorList[Kingdom]);
                }

                int ValueInArray = (x * Map.RealHeight) + y;
                Map.KingdomSave[ValueInArray] = Kingdom;
            }
        }

        for (int x = 0; x < allegiances.instance.Lists.Count; x++)
        {
            if (allegiances.instance.Lists[x].Land == 0)
            {
                SetKingdom();
            }
        }
    }

    void MapAndKingdom()
    {
        Map = HexGenerator.instance;
        NameGen = NameAssigner.instance;
        
        Map.InitialiseCapacity(Map.Width * Map.Height);
        Map.GenerateMap();
    }
    void Barracks()
    {
        //Debug.Log("pt0.1");
        Map.TM.CreateBarracks(KingdomAssigner.instance.regionAmount - 1, MinBoarderPerCastle);
    }
    void CreateNames()
    {
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            NameGen.FirstNameMaleSave = new List<string>(NameGen.FirstNameMale);
            NameGen.FirstNameFemaleSave = new List<string>(NameGen.FirstNameFemale);
            NameGen.PointsLeft = NameGen.HousePoints;
            NameGen.GenerateHouseName(i);
            string House = allegiances.instance.Lists[i].DominentHouseName;

            while (NameGen.PointsLeft > 0)
            {
                NameGen.GenerateName(i, House);
            }
        }

        NameGen.CreateGenerations();
        NameGen.MakeFamilyTree();
        NameGen.DrawFamilyTree();
    }

    void SetCastlesStats()
    {
        //set castle stats
        for (int i = 0; i < 5; i++)
        {
            allegiances.instance.Lists[i + 1].Castles[0].SetStartStats();
        }
    }

    public void SetPlayerCastleStats()
    {
        allegiances.instance.Lists[0].Castles[0].SetStartStats();
    }

    void Update()
    {
        if (Started == false && SelectedCastle == true)
        {
            SetPlayerCastleStats();
            Started = true;
        }
    }
}
