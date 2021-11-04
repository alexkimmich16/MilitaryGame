using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person
{
    public string FullName;
    
    [HideInInspector]
    public int House;
    public int Age;

    [HideInInspector]
    public string First;

    [HideInInspector]
    public string Last;

    [Range(0, 5f)]
    public int Command;

    [Range(0, 5f)]
    public int Wits;

    [Range(0, 5f)]
    public int Diplomacy;

    [Range(0, 5f)]
    public int Attractiveness;

    public Gender gender;

    [HideInInspector]
    public int PositionInList;

    [HideInInspector]
    public int OverallPositionInList;

    public List<string> Relations = new List<string>();

    [HideInInspector]
    public bool Married;

    public List<Person> Children = new List<Person>();
    private Person Mother;
    private Person Father;

    [HideInInspector]
    public Person Parent1;

    [HideInInspector]
    public Person Parent2;

    //[HideInInspector]
    public int Generation;

    //[HideInInspector]
    public bool HasParent;

    public int Faction;

    public List<string> Description = new List<string>();

    public string Discription;

    public enum Gender
    {
        Female = 0,
        Male = 1
    }

    public void GenderChoose(int Num)
    {
        gender = (Gender)Num;
    }
}
