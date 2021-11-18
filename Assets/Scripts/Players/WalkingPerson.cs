using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPerson : MonoBehaviour
{
    public Person currentPerson;
    public Unit unit;

    public float CountTime;
    public float Timer;

    public GameObject Boarder;
    public bool IsSelected;

    // Start is called before the first frame update
    void Start()
    {
        //unit = GetComponent<Unit>();
    }

    public void ChangeBool()
    {
        IsSelected = !IsSelected;
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

        Timer += Time.deltaTime * SpeedScript.instance.CurrentSpeed;
        
        if (unit.map.CheckTile(unit.GridX, unit.GridY) == 6 && Timer > CountTime)
        {
            int Kingdom = unit.map.GetTileFaction(unit.GridX, unit.GridY);
            Castle castle = allegiances.instance.Lists[Kingdom].Castles[0];

            castle.AddToCastle(currentPerson, true);
            PeopleManager.instance.All.Remove(gameObject.transform);
            Destroy(gameObject);
        }
    }

    public void Destory()
    {
        Destroy(gameObject);
    }
}
