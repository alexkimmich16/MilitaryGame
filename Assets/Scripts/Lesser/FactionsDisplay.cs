using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionsDisplay : MonoBehaviour
{
    #region Singleton

    public static FactionsDisplay instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public List<string> HouseNames = new List<string>();
    public List<string> StandingTexts = new List<string>();
    public List<Image> Buttons = new List<Image>();
    
    //tetx
    public List<Text> Displays = new List<Text>();

    public int ActiveHouse;

    private bool Start = true;

    // Update is called once per frame
    void Update()
    {
        if(Start == true)
        {
            HouseNames[0] = "<b>" + "House " + allegiances.instance.Lists[0].DominentHouseName + "</b>";
            HouseNames[1] = "<color=green>" + "House " + allegiances.instance.Lists[1].DominentHouseName + "</color>";
            HouseNames[2] = "<color=blue>" + "House " + allegiances.instance.Lists[2].DominentHouseName + "</color>";
            HouseNames[3] = "<color=red>" + "House " + allegiances.instance.Lists[3].DominentHouseName + "</color>";
            HouseNames[4] = "<color=grey>" + "House " + allegiances.instance.Lists[4].DominentHouseName + "</color>";
            HouseNames[5] = "<color=cyan>" + "House " + allegiances.instance.Lists[5].DominentHouseName + "</color>";
            Start = false;
        }
        
        for (int i = 0; i < Displays.Count; i++)
        {
            if (i != ActiveHouse)
            {
                Displays[i].text = HouseNames[ActiveHouse] + " " + StandingTexts[allegiances.instance.Lists[ActiveHouse].State[i] + 1] + " " + HouseNames[i];
            }
            else
            {
                Displays[i].text = "";
            }
            
        }
    }

    public void SetActiveHouse(int NumChange)
    {
        Buttons[ActiveHouse].color = Color.red;
        Buttons[NumChange].color = Color.blue;
        ActiveHouse = NumChange;
    }
}
