using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleDisplay : MonoBehaviour
{
    #region Singleton

    public static CastleDisplay instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public Castle CurrentCastle;

    public List<GameObject> Panels = new List<GameObject>();

    public int ObjectType;
    public int MaxHealth;
    public int CurrentHealth;

    public Transform parent;
    public GameObject CastlePanel;

    public TextMeshProUGUI Food;
    public TextMeshProUGUI Peasants;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Name;

    public TextMeshProUGUI KnightText;
    public TextMeshProUGUI ArcherText;
    public TextMeshProUGUI CalvalryText;

    public void RecieveCastle(Castle caslte)
    {
        CurrentCastle = caslte;
    }

    bool Spare = false;
    public void RemovePanel(Person person)
    {
        for (int i = 0; i < Panels.Count; i++)
        {
            if (Panels[i].transform.GetComponent<CastleButton>().Current == person)
            {
                Destroy(Panels[i]);
            }
            CurrentCastle.Inside.Remove(person);
        }
    }
    public void ChangeLists()
    {
        
        if (CurrentCastle != null)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                Destroy(Panels[i]);
            }
            Panels.Clear();
            //Debug.Log(CurrentCastle.Inside.Count);
            //Debug.Log(CurrentCastle);

            for (int i = 0; i < CurrentCastle.Inside.Count; i++)
            {
                GameObject PanelSpawn = Instantiate(CastlePanel, new Vector3(0, 0, 0), transform.rotation) as GameObject;
                PanelSpawn.transform.parent = parent;
                Panels.Add(PanelSpawn);

                TextMeshProUGUI text = PanelSpawn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = CurrentCastle.Inside[i].FullName;

                PanelSpawn.transform.GetComponent<CastleButton>().Current = CurrentCastle.Inside[i];
            }
        }
        
            
    }

    public void AddPanel(Person person)
    {
        CurrentCastle.Inside.Add(person);
        GameObject PanelSpawn = Instantiate(CastlePanel, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        PanelSpawn.transform.parent = parent;
        Panels.Add(PanelSpawn);

        TextMeshProUGUI text = PanelSpawn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = person.FullName;

        PanelSpawn.transform.GetComponent<CastleButton>().Current = person;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (CurrentCastle != null)
        {
            Food.text = "Food: " + CurrentCastle.food;
            //Infantry.text = "Infantry: " + CurrentCastle.infantry;
            Peasants.text = "Peasants: " + CurrentCastle.Peasants;
            Health.text = "Health: " + CurrentCastle.CurrentHealth + " / " + CurrentCastle.MaxHealth;
            Name.text = CurrentCastle.FortName;

            KnightText.text = "Knights: " + CurrentCastle.Knights;
            ArcherText.text = "Archers: " + CurrentCastle.Archers;
            CalvalryText.text = "Calvalry: " + CurrentCastle.Calvalry;
        }
        /*
        if (ClickScript.instance.CurrentSelected != null)
        {
            //CurrentCastle = ClickScript.instance.CurrentSelected.GetComponent<Castle>();
            if (Spare == false)
            {
                Spare = true;
                ChangeLists();
            }
        }
        else
        {
            //CurrentCastle = null;
            if (Spare == true)
            {
                Spare = false;
            }
        }
        */
    }
}
