using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsManager : MonoBehaviour
{
    #region Singleton

    public static TacticsManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    //spend 0
    //save 1
    public int TacticsType;
    //public int Faction;

    public List<int> ArmyTactics = new List<int>();
    public List<SingleTactics> TacticsControllers = new List<SingleTactics>();

    public static System.Action OnUpdate;

    public static int TileAmountDifference = 150;

    void Start()
    {
        for (int i = 0; i < TacticsControllers.Count; i++)
        {
            OnUpdate += TacticsControllers[i].Update;
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
        for (int i = 0; i < ArmyTactics.Count; i++)
        {
            if(ArmyTactics[i] == 0)
            {
                if (allegiances.instance.Lists[i].Money > 5)
                {

                }
            }
            else if(ArmyTactics[i] == 1) 
            {
                if (allegiances.instance.Lists[i].Money > 15)
                {

                }
            }

            //if not enough space to expand do something jurastic if hostile, wait if patient
            //form allience for land, attack for land, marry for land, buy land
        }
    }

    public void AskPlayerForAlliance(int FactionThatAsked)
    {
        
        //create text
        // prompt yes or no
        // who to send
    }
}
