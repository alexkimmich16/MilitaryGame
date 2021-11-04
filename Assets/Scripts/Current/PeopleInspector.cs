using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeopleInspector : MonoBehaviour
{
    #region Singleton
    public static PeopleInspector instance;
    void Awake() { instance = this; }
    #endregion

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Age;
    public TextMeshProUGUI Gender;
    public TextMeshProUGUI House;

    public int XRow;
    public int YRow;

    public float RightAdd;
    public float DownAdd;

    //public TextMeshProUGUI Name;
    public Person person;

    public void SetMenu()
    {

    }

    public void GetPerson(string Got)
    {
        string Xstring = Got.Substring(0, 1);
        string Ystring = Got.Substring(1, 1);
        
        int.TryParse(Xstring, out int X);
        int.TryParse(Ystring, out int Y);

        if (allegiances.instance.People.Count >= X)
        {
            if (allegiances.instance.People[X].People.Count >= Y)
            {
                //Debug.Log(X + " " + Y);
                Person person = allegiances.instance.People[X].People[Y];
                UpdateStats(person);
                UIManager.instance.AllPeopleOpen(false);
                UIManager.instance.PeopleMenuOpen(true);
            }
        }
    }

    public void UpdateStats(Person newPerson)
    {
        person = newPerson;
        Name.text = person.FullName;
        Description.text = person.Discription;
        Age.text = "Age: " + person.Age;
        House.text = "Of House " + person.Last;
        
        if ((int)person.gender == 0)
        {
            Gender.text = "Gender: Female";
        }
        else
        {
            Gender.text = "Gender: Male";
        }
    }
}
