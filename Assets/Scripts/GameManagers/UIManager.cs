using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject StatsManagerOBJ;

    public bool ChangeTerrain;
    public GameObject wall;
    public GameObject building;
    public bool FarmChosen;
    public bool BuildingChosen;
    Image FarmSR;
    Image BuildingSR;
    public bool ShowBarracks;
    public GameObject Barracks;
    public GameObject BattleFieldOBJ;

    //public GameObject TileParent;
    public GameObject FamilyTree;
    public GameObject Castle;

    public GameObject SendMenu;
    public GameObject PeopleInspector;
    public GameObject AllPeople;

    public ClickableTile Tile;

    public GameObject FactionsMenu;

    public List<TypeOfBanner> BannerBuffer = new List<TypeOfBanner>();

    public GameObject Banner;
    public GameObject YesNo;
    public GameObject Closebutton;

    public SendMenu sendScript;

    public int Accept;
    
    public void TurnMenusOff()
    {
        SendMenuOpen(false);
        AllPeopleOpen(false);
        SetStatManager(false);
        ClickScript.instance.castle = null;
    }

    public void AcceptCurrent()
    {
        //allegiances.instance.StartAlliance(Accept, 0);
        TacticsManager.instance.TacticsControllers[Accept].PlayerReaction(true);
        Accept = 0;
        //Debug.Log("do");
        NextBanner();
        
    }
    public void DenyCurrent()
    {
        TacticsManager.instance.TacticsControllers[Accept].PlayerReaction(false);
        Accept = 0;
        NextBanner();
    }

    public void SendMenuRecieve(int2 Pos)
    {
        sendScript.RecieveObjective(Pos);
    }

    public void SendMenuOpen(bool Open)
    {
        SendMenu.SetActive(Open);
    }

    public void BattleFieldOpen(bool Open)
    {
        BattleFieldOBJ.SetActive(Open);
    }

    public void AllPeopleOpen(bool Open)
    {
        AllPeople.SetActive(Open);
    }
    
    public void PeopleMenuOpen(bool Open)
    {
        PeopleInspector.SetActive(Open);
    }
    
    public void RecieveTile(Transform tile)
    {
        Tile = tile.GetComponent<ClickableTile>();
    }

    public void ChangeFactionsMenu()
    {
        //TileParent.SetActive(!TileParent.activeInHierarchy);
        FactionsMenu.SetActive(!FactionsMenu.activeInHierarchy);
    }

    public void ChangeTree()
    {
        //TileParent.SetActive(!TileParent.activeInHierarchy);
        FamilyTree.SetActive(!FamilyTree.activeInHierarchy);
    }

    public void ShowBanner(TypeOfBanner Buffer)
    {
        if(Buffer.IsYesNo == true)
        {
            YesNo.SetActive(true);
            Closebutton.SetActive(false);
            Accept = Buffer.Faction;
        }
        else
        {
            YesNo.SetActive(false);
            Closebutton.SetActive(true);

        }
        Banner.SetActive(true);
        BannerScript.instance.SetBanner(Buffer.Text);

        SpeedScript.instance.CurrentSpeed = 0;
    }

    public void NextBanner()
    {
        Banner.SetActive(false);
        //SpeedScript.instance.CurrentSpeed = SpeedScript.instance.BackupSpeed;
    }

    void Update()
    {
        if(BannerBuffer.Count > 0 && Banner.activeSelf == false)
        {
            ShowBanner(BannerBuffer[0]);
            BannerBuffer.RemoveAt(0);
        }

        if (ClickScript.instance.castle == null)
        {
            Castle.SetActive(false);
        }
        else
        {
            Castle.SetActive(true);
            CastleDisplay.instance.CurrentCastle = ClickScript.instance.castle;
        }
        if (ClickScript.instance.CurrentSelected != null)
        {
            
        }
        if (StatsManagerOBJ.GetComponent<StatsManager>().Selected == null)
        {
            SetStatManager(false);
        }
        else
        {
            SetStatManager(true);
        }
        /*
        if (ShowBarracks == true)
        {
            ChangeTerrain = false;
            Barracks.SetActive(true);
        }
        else
        {
            Barracks.GetComponent<BarracksManager>().CurrentBarracks = null;
            Barracks.SetActive(false);
        }
        */
        if (ChangeTerrain == true)
        {
            wall.SetActive(true);
            building.SetActive(true);
            if (FarmChosen == true)
            {
                
                FarmSR.color = Color.white;
                BuildingSR.color = Color.blue;
            }
            else
            {
                FarmSR.color = Color.blue;
                BuildingSR.color = Color.white;
            }
        }
        else
        {
            wall.SetActive(false);
            building.SetActive(false);
        }
    }

    public void SetStatManager(bool Active)
    {
        StatsManagerOBJ.SetActive(Active);
    }

    public void SetBarracks(bool Active)
    {
        Barracks.SetActive(Active);
    }
    public void ChangeBarracks()
    {
        ShowBarracks = !ShowBarracks;
    }

    public void SetTerrain(bool Active)
    {
        //ChangeTerrain.SetActive(Active);
    }
    public void Building()
    {
        FarmChosen = false;
        BuildingChosen = true;
    }
    public void Farm()
    {
        FarmChosen = true;
        BuildingChosen = false;
    }
    // Update is called once per frame
    void Start()
    {
        FarmSR = wall.GetComponent<Image>();
        BuildingSR = building.GetComponent<Image>();

        //TileParent.SetActive(true);
        FamilyTree.SetActive(false);
    }
}
