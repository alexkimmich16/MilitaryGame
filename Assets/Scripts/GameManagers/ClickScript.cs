using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Mathematics;

public class ClickScript : MonoBehaviour
{
    #region Singleton

    public static ClickScript instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    //click people on left
    public Camera cam;
    public GameObject CurrentSelected;
    public bool ChangeTerrain;
    public HexGenerator map;
    public DropScript DS;
    public bool hasUnit;
    public bool SelectDropPoint;
    public bool AllTroops;

    public Transform TargetProvider;

    //public int CastleKingdom = 7;
    public Castle castle;

    //public Castle selectedCastle;
    #region Non-Battle
    public void ClickTouchLeft()
    {
        //get mouse
        //Debug.Log("pt1");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Ajusted = mousePos;
        Ajusted.y -= map.YDrawOffset;
        //ints from mouse
        Vector3Int gridInts = map.mountainMap.WorldToCell(Ajusted);
        Debug.Log(map.GetTerrainValue(gridInts.x, gridInts.y));
        if (InBounds(gridInts.x, gridInts.y) == false || IsPointerOverUIObject() == true)
        {
            return;
        }
        //Debug.Log("pt2");
        UIManager.instance.SetBarracks(false);

        int TileType = map.CheckTile(gridInts.x, gridInts.y);
        
        if (TileType == 6)
        {
            int Kingdom = map.GetTileFaction(gridInts.x, gridInts.y);
            //Debug.Log(gridInts.x + " " + gridInts.y);
            //CastleKingdom = Kingdom;
            castle = allegiances.instance.Lists[Kingdom].Castles[0];
            //CastleDisplay.instance.Castle
        }
        else
        {
            castle = null;
        }
       //Debug.Log("pt3");
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (SelectDropPoint == true && hit != null)
        {
            if (DS.DropPoints.Contains(new int2(gridInts.x, gridInts.y)))
            {
                //PeopleSpawner.instance.SpawnFriendly(hit.transform);
                StartOrganiser.SelectedCastle = true;
                DS.ClearDropList();
                map.TM.SpawnPlayerCastle(gridInts.x, gridInts.y);
                SelectDropPoint = false;
            }
        }
        //Debug.Log("pt4");
        if (CurrentSelected != null)
        {
            CurrentSelected.GetComponent<Unit>().SetActive(false);
            UIManager.instance.SetStatManager(false);
        }

        CurrentSelected = null;
        //Debug.Log("pt5");
        if (hit.transform != null)
        {
           // Debug.Log("pt6");
            if (hit.transform.GetComponent<Unit>())
            {
                //Debug.Log("pt7");
                hit.transform.SendMessage("SetActive", true, SendMessageOptions.DontRequireReceiver);
                hit.transform.GetComponent<Unit>().SetActive(true);
                CurrentSelected = hit.transform.gameObject;
                UIManager.instance.SetStatManager(true);
            }
        }
        
    }
    public void ClickTouchRight()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Adjusted = mousePos;
        Adjusted.y -= map.YDrawOffset;
        RaycastHit2D hit2 = Physics2D.Raycast(mousePos, Vector2.zero);
        Vector3Int gridInts = map.mountainMap.WorldToCell(Adjusted);
        
        if (gridInts.x > map.RealWidth || gridInts.x < 0 || gridInts.y > map.RealHeight || gridInts.y < 0)
        {
            return;
        }
        //Debug.Log("pt2");
        if (CurrentSelected != null)
        {
            //Debug.Log("pt2");
            if (UIManager.instance.ChangeTerrain == false)
            {
                // Debug.Log("pt3");
                if (CurrentSelected.GetComponent<Unit>())
                {
                    //Debug.Log("pt4");
                    int Faction = CurrentSelected.GetComponent<Unit>().FactionNum;
                    int2 start = new int2(CurrentSelected.GetComponent<Unit>().GridX, CurrentSelected.GetComponent<Unit>().GridY);
                    int2 end = new int2(gridInts.x, gridInts.y);
                    List<int> Friendly = new List<int>();
                    for (int i = 0; i < allegiances.instance.Lists.Count; i++)
                    {
                        if (allegiances.instance.Lists[Faction].State[i] == -1 || allegiances.instance.Lists[Faction].State[i] == 0)
                        {
                            Friendly.Add(i);
                        }
                    }
                    List<int2> Path = map.PM.FindPath(start, end, Faction, 20, false);
                    CurrentSelected.GetComponent<Unit>().AddToPath(Path);
                    //Debug.Log("pt5");
                }
                if (hit2.transform.tag == "Enemy" && CurrentSelected.transform.tag == "Infantry")
                {
                    //CurrentSelected.GetComponent<Infantry>().Attack(hit2.transform);
                }
                else if (hit2.transform.tag == "Enemy" && CurrentSelected.transform.tag == "Mech")
                {
                    //CurrentSelected.GetComponent<Mech>().Attack(hit2.transform);
                }
            }
            if (CurrentSelected.GetComponent<Castle>() && hit2.transform.tag == "Ground")
            {
                
            }
        }
        if (castle != null)
        {
            if (InBounds(gridInts.x, gridInts.y) == true)
            {
                UIManager.instance.SendMenuRecieve(new int2(gridInts.x, gridInts.y));
                UIManager.instance.SendMenuOpen(true);
            }
            
        }
    }
    public void RightHold()
    {
    }
    #endregion

    #region Battle
    public void ClickTouchLeftBattle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Ajusted = mousePos;
        Ajusted.y -= map.YDrawOffset;
        //ints from mouse
        Vector3Int gridInts = BattleField.instance.FieldMap.WorldToCell(Ajusted);

        CurrentSelected = null;
        if (InBoundsBattle(gridInts.x, gridInts.y) == false)
        {
            return;
        }
        Debug.Log("pt1");
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.transform != null)
        {
            Debug.Log("pt2");
            if (IsPointerOverUIObject() == false)
            {
                Debug.Log("pt3");
                if (hit.transform.GetComponent<BattleUnit>())
                {
                    Debug.Log("pt4");
                    //hit.transform.SendMessage("SetActive", true, SendMessageOptions.DontRequireReceiver);
                    hit.transform.GetComponent<BattleUnit>().SetActive(true);
                    CurrentSelected = hit.transform.gameObject;
                    UIManager.instance.SetStatManager(true);
                }
            }
        }
    }
    public void ClickTouchRightBattle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Adjusted = mousePos;
        Adjusted.y -= map.YDrawOffset;
        RaycastHit2D hit2 = Physics2D.Raycast(mousePos, Vector2.zero);
        Vector3Int gridInts = map.mountainMap.WorldToCell(Adjusted);
        
        if (InBoundsBattle(gridInts.x, gridInts.y) == true)
        {
            return;
        }

        if (CurrentSelected != null)
        {
            if (CurrentSelected.GetComponent<BattleUnit>())
            {
                //Spawned.GetComponent<BattleUnit>().World = position;
                BattleUnit unit = CurrentSelected.GetComponent<BattleUnit>();
                int2 start = new int2(unit.Grid.x, unit.Grid.y);
                int2 end = new int2(gridInts.x, gridInts.y);
                //List<int> Friendly = new List<int>();
                //List<int2> Path = map.PM.FindPath(start, end, Faction, 20, false);
                //CurrentSelected.GetComponent<Unit>().AddToPath(Path);
            }
        }
        if (castle != null)
        {
            if (InBounds(gridInts.x, gridInts.y) == true)
            {
                UIManager.instance.SendMenuRecieve(new int2(gridInts.x, gridInts.y));
                UIManager.instance.SendMenuOpen(true);
            }

        }
    }
    public void RightHoldBattle()
    {

    }
    #endregion

    void Update()
    {
        if(map.selectedUnit != null)
        {
            map.selectedUnit = CurrentSelected;
        }
        if (BattleField.instance.Active == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                ClickTouchRight();
            }
            if (Input.GetMouseButtonDown(0))
            {
                ClickTouchLeft();
            }
            if (Input.GetMouseButton(1))
            {
                RightHold();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                ClickTouchRightBattle();
            }
            if (Input.GetMouseButtonDown(0))
            {
                ClickTouchLeftBattle();
            }
            if (Input.GetMouseButton(1))
            {
                RightHoldBattle();
            }
        }
    }

    

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private bool InBoundsBattle(int x, int y)
    {
        BattleField BF = BattleField.instance;
        if (x > BF.MapSize.y || x < 0 || y > BF.MapSize.x || y < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    private bool InBounds(int x, int y)
    {
        //Debug.Log();
        
        if (x > map.RealWidth || x < 0 || y > map.RealHeight || y < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
