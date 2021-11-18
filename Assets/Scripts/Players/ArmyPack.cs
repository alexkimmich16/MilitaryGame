using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyPack : MonoBehaviour
{
    public int Knights;
    public int Archers;
    public int Calvalry;
    public Person Leader;

    public bool IsSelected;
    private Unit unit;
    public GameObject Boarder;

    public bool Fighting = false;
    public bool Friendly = false;

   // __ was convinced that someone was going to murder her, so she
   //defected
   //killed the person who was going to kill her

    // Start is called before the first frame update
    void Start()
    {
        unit = gameObject.GetComponent<Unit>();
        PeopleManager.instance.Armies.Add(unit);
    }

    public void SetMen(int Knight, int Archer, int CalvalrySet)
    {
        Knights = Knight;
        Archers = Archer;
        Calvalry = CalvalrySet;
    }

    public void SetLeader(Person LeaderDrop)
    {
        Leader = LeaderDrop;
    }

    public void AddDamage(int KnightLoss, int ArchersLoss, int CalvalryLoss)
    {
        Knights -= KnightLoss;
        Archers -= ArchersLoss;
        Calvalry -= CalvalryLoss;
    }

    public void ChangeBool()
    {
        IsSelected = !IsSelected;
    }
    public void ClearActive()
    {
        IsSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSelected == true)
        {
            Boarder.SetActive(true);
        }
        else
        {
            Boarder.SetActive(false);
        }
    }
}
