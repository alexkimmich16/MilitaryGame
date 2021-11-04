using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CastleInfo
{
    public string CastleName;
    public List<Person> PeopleInside = new List<Person>();
    public int Alligence;
    public int HoldingNum;
}
