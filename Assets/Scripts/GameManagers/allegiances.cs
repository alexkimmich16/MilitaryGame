using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allegiances : MonoBehaviour
{
    #region Singleton

    public static allegiances instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    
    public List<ListOfEnemies> Lists = new List<ListOfEnemies>();

    public List<Group> People = new List<Group>();
    //0 is neutral
    //-1 is enemy
    // +1 is ally

    public List<string> ArmyNames = new List<string>();

    public void StartWar(int Num1, int Num2)
    {
        Lists[Num1].State[Num2] = -1;
        Lists[Num2].State[Num1] = -1;
    }

    public void StartAlliance(int Num1, int Num2)
    {
        Lists[Num1].State[Num2] = 1;
        Lists[Num2].State[Num1] = 1;
    }

    public void StartTempAlliance(int Num1, int Num2)
    {
        Lists[Num1].State[Num2] = 2;
        Lists[Num2].State[Num1] = 2;
    }
}
