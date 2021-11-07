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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Ajusted = mousePos;
        Ajusted.y -= map.YDrawOffset;
        //ints from mouse
        Vector3Int gridInts = map.mountainMap.WorldToCell(Ajusted);
        //Debug.Log("CLICK:  " + InBounds(gridInts.x, gridInts.y));
        if (InBounds(gridInts.x, gridInts.y) == false)
        {
            return;
        }
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

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (SelectDropPoint == true && hit != null)
        {
            if (DS.DropPoints.Contains(new int2(gridInts.x, gridInts.y)))
            {
                //Debug.Log("contains " + gridInts.x + " " + gridInts.y);
                //PeopleSpawner.instance.SpawnFriendly(hit.transform);
                StartOrganiser.SelectedCastle = true;
                DS.ClearDropList();
                map.TM.SpawnPlayerCastle(gridInts.x, gridInts.y);
                SelectDropPoint = false;
            }
        }
        if (IsPointerOverUIObject() == false)
        {
            UIManager.instance.BarracksNull();
        }

        if (CurrentSelected != null)
        {
            CurrentSelected.GetComponent<Unit>().SetActive(false);
            UIManager.instance.SetStatManager(false);
        }

        CurrentSelected = null;
        if (hit.transform != null)
        {
            if (IsPointerOverUIObject() == false)
            {
                if (hit.transform.GetComponent<Unit>())
                {
                    //hit.transform.SendMessage("SetActive", true, SendMessageOptions.DontRequireReceiver);
                    hit.transform.GetComponent<Unit>().SetActive(true);
                    CurrentSelected = hit.transform.gameObject;
                    UIManager.instance.SetStatManager(true);
                }
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
        //int TileType = map.CheckTile(gridInts.x, gridInts.y);
        //Debug.Log("pt1");
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
                    map.PM.FindPath(start, end, CurrentSelected, Faction, 20, allegiances.instance.Lists[Faction].Castles[0], false);
                    //Debug.Log("pt5");
                }
                if (hit2.transform.tag == "Enemy" && CurrentSelected.transform.tag == "Infantry")
                {
                    CurrentSelected.GetComponent<Infantry>().Attack(hit2.transform);
                }
                else if (hit2.transform.tag == "Enemy" && CurrentSelected.transform.tag == "Mech")
                {
                    CurrentSelected.GetComponent<Mech>().Attack(hit2.transform);
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit3 = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit3 != null)
        {
            if (UIManager.instance.ChangeTerrain == true)
            {
                if (hit3.transform.tag == "Ground")
                {
                    //GameObject.activeSelf;
                    int instaniateNum;
                    if (UIManager.instance.FarmChosen == true)
                    {
                        instaniateNum = 0;
                    }
                    else
                    {
                        instaniateNum = 1;
                    }

                    hit3.collider.transform.GetComponent<ClickableTile>().Destroy(instaniateNum);
                }
            }

        }
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
        if (InBoundsBattle(gridInts.x, gridInts.y) == true)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit != null && hit.transform.GetComponent<BattleUnit>())
        {
            hit.transform.GetComponent<BattleUnit>().SetActive(true);
            CurrentSelected = hit.transform.gameObject;
            UIManager.instance.SetStatManager(true);
        }
        
    }
    public void ClickTouchRightBattle()
    {

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
        
        //left Touch
        /*
                if (hit.transform.tag == "Worker" && UIManager.instance.ChangeTerrain == false)
                {
                    if (AllTroops == false && hit.transform.gameObject.GetComponent<Worker>().unit.FactionNum != 0)
                    {
                        return;
                    }
                    Worker worker = hit.transform.gameObject.GetComponent<Worker>();
                    worker.ChangeBool();
                    CurrentSelected = hit.transform.gameObject;
                }

                if (hit.transform.tag == "Infantry" && UIManager.instance.ChangeTerrain == false)
                {
                    if (AllTroops == false && hit.transform.gameObject.GetComponent<Infantry>().unit.FactionNum != 0)
                    {
                        return;
                    }
                    Infantry infentryScript = hit.transform.gameObject.GetComponent<Infantry>();
                    infentryScript.ChangeBool();
                    //infentryScript.Shoot();
                    //infentryScript.Target = TargetProvider;
                    CurrentSelected = hit.transform.gameObject;
                }
                if (hit.transform.tag == "Mech")
                {
                    if (AllTroops == false && hit.transform.gameObject.GetComponent<Worker>().unit.FactionNum != 0)
                    {
                        return;
                    }
                    hit.transform.gameObject.GetComponent<Mech>().ChangeBool();
                    CurrentSelected = hit.transform.gameObject;
                }
                else if (hit.transform.tag == "Farm")
                {
                    UIManager.instance.ChangeBarracks();
                    hit.transform.gameObject.GetComponent<BarracksSpawn>().IsSelected = true;

                }
                else if (hit.transform.tag == "Castle")
                {
                    CurrentSelected = hit.transform.gameObject;
                }
                else if (hit.transform.tag == "Person")
                {
                    if (AllTroops == false && hit.transform.gameObject.GetComponent<WalkingPerson>().unit.FactionNum != 0)
                    {
                        return;
                    }
                    CurrentSelected = hit.transform.gameObject;
                    hit.transform.gameObject.GetComponent<WalkingPerson>().IsSelected = true;
                }
                */
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
        if (x > map.RealHeight || x < 0 || y > map.RealWidth || y < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
