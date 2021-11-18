using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public TextMeshProUGUI ObjectName;
    public TextMeshProUGUI Inside;

    public TextMeshProUGUI Knights;
    public TextMeshProUGUI Archers;
    public TextMeshProUGUI Calvalry;


    public List<string> Strings = new List<string>();

    public int ObjectType;
    public GameObject Selected;
    public int MaxHealth;
    public int CurrentHealth;

    //1 = infantry
    //2 = worker
    //3 = mech
    //4 = skeleton
    /*
    void GenerateText(string Name, string Health, string Inside, string Name, string Health, string Inside,)
    {
        ObjectName.text = Name;
    }
    */

    void SetText(List<int> numbers, string RealName)
    {
        
        if(RealName == null)
        {
            ObjectName.text = Strings[numbers[0]];
        }
        else
        {
            ObjectName.text = RealName;
        }
        if (numbers[1] != 10000)
        {
            Inside.text = "Inside: " + numbers[1];
        }
        else
        {
            Inside.text = "";
        }
        if (numbers[2] != 10000)
        {
            Knights.text = "Knights: " + numbers[2];
        }
        else
        {
            Knights.text = "";
        }
        if (numbers[3] != 10000)
        {
            Archers.text = "Archers: " + numbers[3];
        }
        else
        {
            Archers.text = "";
        }
        if (numbers[4] != 10000)
        {
            Calvalry.text = "Calvalry: " + numbers[4];
        }
        else
        {
            Calvalry.text = "";
        }


    }
    void Update()
    {
        if(ClickScript.instance.CurrentSelected != null)
        {
            Selected = ClickScript.instance.CurrentSelected;
        }
        else
        {
            Selected = null;
        }
        

        int Num = 0;

        //Peasants.text = "";
        bool Custom = false;
        List<int> Display = new List<int>();
        for (int i = 0; i < 7; i++)
            Display.Add(0);
        if (Selected.GetComponent("ArmyPack"))
        {
            Display[0] = 0;
            Display[1] = 10000;
            Display[2] = Selected.GetComponent<ArmyPack>().Knights;
            Display[3] = Selected.GetComponent<ArmyPack>().Archers;
            Display[4] = Selected.GetComponent<ArmyPack>().Calvalry;
            SetText(Display, null);
        }
        else if (Selected.GetComponent("BattleUnit"))
        {
            Display[2] = 10000;
            Display[3] = 10000;
            Display[4] = 10000;
            if (Selected.GetComponent("BattleKnight"))
            {
                Display[0] = 1;
            }
            else if (Selected.GetComponent("BattleArcher"))
            {
                Display[0] = 2;
            }
            else if (Selected.GetComponent("BattleCalvalry"))
            {
                Display[0] = 3;
            }
            Display[1] = Selected.GetComponent<BattleUnit>().inside;
            SetText(Display, null);
        }
        else if (Selected.GetComponent("WalkingPerson"))
        {
            /*
            string Name = Selected.GetComponent<WalkingPerson>().currentPerson.FullName;
            Display[0] = 0;
            Display[1] = Selected.GetComponent<PeasantGroup>().Population;
            Display[2] = 10000;
            Display[3] = 10000;
            Display[4] = 10000;
            SetText(Display, Name);
            */
        }
        else if (Selected.GetComponent("PeasantGroup"))
        {
            Display[0] = 4;
            Display[1] = Selected.GetComponent<PeasantGroup>().Population;
            Display[2] = 10000;
            Display[3] = 10000;
            Display[4] = 10000;
            SetText(Display, null);
        }
        
    }
}