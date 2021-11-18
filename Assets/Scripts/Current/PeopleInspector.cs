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

    //public int XRow;
    // public int YRow;

    //public float RightAdd;
    //public float DownAdd;

    public int Slots;

    public List<TransformList> FactionButtons = new List<TransformList>();
    //public TextMeshProUGUI Name;

    public Person person;
    public void SetMenu()
    {
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            Debug.Log("i: " + i);
            int Count = allegiances.instance.People[i].People.Count;
            int ToRemove = Slots - Count;
            Debug.Log("Count: " + Count);
            Debug.Log("ToRemove: " + ToRemove);
            for (int j = 0; j < ToRemove; j++)
            {
                
                int NumToRemove = Slots - j -1;
                Debug.Log("i: " + i + "  j:  " + NumToRemove);
                RemoveButton(i, NumToRemove);
            }
                
        }
    }

    public void RemoveButton(int x, int y)
    {
        GameObject button = FactionButtons[x].Buttons[y];
        FactionButtons[x].Buttons[y] = null;
        Destroy(button);
    }

    public void GetPerson(string Got)
    {
        string Xstring = Got.Substring(0, 1);
        string Ystring = Got.Substring(1, 1);
        
        int.TryParse(Xstring, out int X);
        int.TryParse(Ystring, out int Y);

        if (allegiances.instance.People.Count >= Y)
        {
            if (allegiances.instance.People[Y].People.Count >= X)
            {
                //Debug.Log(X + " " + Y);
                Person person = allegiances.instance.People[Y].People[X];
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
