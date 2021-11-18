using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class PeopleManager : MonoBehaviour
{
    #region Singleton

    public static PeopleManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    /// <summary>
    /// check for people togehter
    /// </summary>

    public HexGenerator Map;

    //public List<Transform> FriendTransform = new List<Transform>();

    public List<Transform> All = new List<Transform>();
    public List<int2> BothInt = new List<int2>();
    public List<int> IntArraySingle = new List<int>();

    public List<int2> neighbourOffsetArrayEven = new List<int2>();
    public List<int2> neighbourOffsetArrayOdd = new List<int2>();
    public List<Unit> Armies = new List<Unit>();

    public bool SimpleMapGen = true;

    public void AddToList(Transform AddToAll, int x, int y)
    {
        All.Add(AddToAll);
        BothInt.Add(new int2(x, y));
        IntArraySingle.Add(0);
    }

    public void RemoveFromList(Transform toRemove)
    {
        for (int i = 0; i < All.Count; i++)
        {
            if (All[i] == toRemove)
            {
                All.RemoveAt(i);
            }
        }
        BothInt.RemoveAt(BothInt.Count - 1);
        IntArraySingle.RemoveAt(BothInt.Count - 1);
    }
    void ArmyBattleDetect()
    {
        for (var i = 0; i < Armies.Count; i++)
        {
            for (var j = 0; j < Armies.Count; j++)
            {
                if (i != j)
                {
                    Unit UnitI = Armies[i].gameObject.GetComponent<Unit>();
                    Unit UnitJ = Armies[j].gameObject.GetComponent<Unit>();
                    //neighbour
                    List<int2> Offset = new List<int2>();
                    if (UnitI.GridY % 2 == 1)
                    {
                        Offset = Map.neighbourOffsetArrayEven;
                    }
                    else
                    {
                        Offset = Map.neighbourOffsetArrayOdd;
                    }

                    for (var n = 0; n < Offset.Count; n++)
                    {
                        int2 Ajusted = new int2(UnitI.GridX + Offset[n].x, UnitI.GridY + Offset[n].y);
                        if (Ajusted.x == UnitJ.GridX && Ajusted.y == UnitJ.GridY)
                        {
                            ArmyPack ArmyI = Armies[i].gameObject.GetComponent<ArmyPack>();
                            ArmyPack ArmyJ = Armies[j].gameObject.GetComponent<ArmyPack>();
                            if (ArmyI.Fighting == false && ArmyJ.Fighting == false)
                            {
                                //Debug.Log("Battle");
                                ArmyI.Fighting = true;
                                ArmyJ.Fighting = true;
                                List<int2> BattleFieldTiles = new List<int2>();
                                BattleFieldTiles.Add(new int2(UnitI.GridX, UnitI.GridY));
                                BattleFieldTiles.Add(new int2(UnitJ.GridX, UnitJ.GridY));
                                UIManager.instance.BattleFieldOpen(true);
                                BattleField.instance.SetActive(true);
                                if (SimpleMapGen == true)
                                {

                                    ArmyPack MyArmy = Armies[i].gameObject.GetComponent<ArmyPack>();
                                    ArmyPack TheyArmy = Armies[j].gameObject.GetComponent<ArmyPack>();
                                    if (ArmyI.Friendly == true)
                                    {
                                        MyArmy = ArmyI;
                                        TheyArmy = ArmyJ;
                                    }
                                    else
                                    {
                                        MyArmy = ArmyJ;
                                        TheyArmy = ArmyI;
                                    }
                                    List<int> Friendly = new List<int>(3);
                                    List<int> Enemy = new List<int>(3);
                                    Friendly.Add(MyArmy.Knights);
                                    Friendly.Add(MyArmy.Archers);
                                    Friendly.Add(MyArmy.Calvalry);

                                    Enemy.Add(TheyArmy.Knights);
                                    Enemy.Add(TheyArmy.Archers);
                                    Enemy.Add(TheyArmy.Calvalry);

                                    BattleField.instance.UnfunctionalTileSpawn(Friendly, Enemy);
                                }
                                else
                                {
                                    BattleField.instance.RecieveTileInfo(BattleFieldTiles);
                                }


                                
                            }

                        }

                    }

                }
            }
        }
    }
    void StopTileStacking()
    {
        for (var i = 0; i < BothInt.Count; i++)
        {
            for (var j = 0; j < BothInt.Count; j++)
            {
                if (i != j)
                {
                    if (BothInt[i].x == BothInt[j].x && BothInt[i].y == BothInt[j].y)
                    {
                        if (All[i].GetComponent<Unit>().Iswalking == false && All[j].GetComponent<Unit>().Iswalking == false)
                        {
                            int Num = Random.Range(0, 5);
                            List<int2> CurrentOffset = new List<int2>();
                            if (BothInt[i].y % 2 == 1)
                            {
                                CurrentOffset = neighbourOffsetArrayEven;
                            }
                            else
                            {
                                CurrentOffset = neighbourOffsetArrayOdd;
                            }
                            if (BothInt[i].y % 2 == 1)
                            {
                                int BothX = BothInt[i].x + CurrentOffset[Num].x;
                                int BothY = BothInt[i].y + CurrentOffset[Num].y;
                                //if its a place it can't go or is already inhabitied and deson't contain
                                if (Map.CheckTile(BothX, BothY) != 0 && Map.CheckTile(BothX, BothY) != 3 && !IntArraySingle.Contains((BothX * Map.RealHeight) + BothY))
                                {
                                    //Debug.Log("pt3");
                                    Vector2 pos = Map.GetPosition(BothX, BothY);

                                    All[i].GetComponent<Unit>().WorldX = pos.x;
                                    All[i].GetComponent<Unit>().WorldY = pos.y;
                                    All[i].GetComponent<Unit>().GridX += CurrentOffset[Num].x;
                                    All[i].GetComponent<Unit>().GridY += CurrentOffset[Num].y;
                                    return;
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        neighbourOffsetArrayEven = Map.neighbourOffsetArrayEven; // Left
        neighbourOffsetArrayOdd = Map.neighbourOffsetArrayOdd; // Left
    }

    // Update is called once per frame
    void Update()
    {
        StopTileStacking();
        ArmyBattleDetect();
        if (All.Count > 0)
        {
            for (int i = 0; i < All.Count; i++)
            {
                BothInt[i] = new int2(All[i].gameObject.GetComponent<Unit>().GridX, All[i].gameObject.GetComponent<Unit>().GridY);
                IntArraySingle[i] = (BothInt[i].x * Map.RealHeight) + BothInt[i].y;
            }
        }
        
    }


}
