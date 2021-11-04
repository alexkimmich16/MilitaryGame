using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public Text ObjectName;
    public Text Health;
    public Text Peasants;

    public List<string> Strings = new List<string>();

    public int ObjectType;
    public GameObject Selected;
    public int MaxHealth;
    public int CurrentHealth;

    //1 = infantry
    //2 = worker
    //3 = mech
    //4 = skeleton

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


        Peasants.text = "";
        bool Custom = false;
        if (Selected.GetComponent("Infantry"))
        {
            MaxHealth = Selected.GetComponent<Infantry>().MaxHealth;
            CurrentHealth = Selected.GetComponent<Infantry>().Health;
            Num = 1;
        }
        else if (Selected.GetComponent("Worker"))
        {
            
            MaxHealth = Selected.GetComponent<Worker>().MaxHealth;
            CurrentHealth = Selected.GetComponent<Worker>().Health;
            Num = 2;
        }
        else if (Selected.GetComponent("Mech"))
        {
            
            MaxHealth = Selected.GetComponent<Mech>().MaxHealth;
            CurrentHealth = Selected.GetComponent<Mech>().Health;
            Num = 3;
        }
        else if (Selected.GetComponent("Skeleton"))
        {
            
            MaxHealth = Selected.GetComponent<Skeleton>().MaxHealth;
            CurrentHealth = Selected.GetComponent<Skeleton>().Health;
            Num = 4;
        }
        else if (Selected.GetComponent("WalkingPerson"))
        {
            ObjectName.text = Selected.GetComponent<WalkingPerson>().currentPerson.FullName;
            MaxHealth = 0;
            CurrentHealth = 0;
            Custom = true;
        }
        else if (Selected.GetComponent("PeasantGroup"))
        {
            ObjectName.text = "PeasantGroup";
            Peasants.text = "Peasants: " + Selected.GetComponent<PeasantGroup>().Population;
            MaxHealth = 0;
            CurrentHealth = 0;
            Custom = true;
        }
        string HealthDisplay = "Health: " + MaxHealth + "/" + MaxHealth;
        Health.text = HealthDisplay;
        if(Custom == false)
        {
            string ObjectDisplay = Strings[Num];
            ObjectName.text = ObjectDisplay;
        }    
    }
}