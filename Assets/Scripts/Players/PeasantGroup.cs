using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantGroup : MonoBehaviour
{
    public int Population;
    //public GameObject Boarder;
    //public bool IsSelected;
    public Unit unit;

    public float CountTime;
    public float Timer;

    public int ObjectiveX;
    public int ObjectiveY;

    public Castle castle;
    //public 

    

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (IsSelected == true)
        {
            Boarder.SetActive(true);
        }
        else
        {
            Boarder.SetActive(false);
        }
        
        GameObject tile = unit.map.TileTwoArray[unit.GridX, unit.GridY];
        if (tile.tag == "Castle" && Timer > CountTime)
        {
            tile.GetComponent<Castle>().AddPeasantsToCastle(Population);
            PeopleManager.instance.All.Remove(gameObject.transform);
            Destroy(gameObject);
        }
        
        if(unit.GridX == ObjectiveX && unit.GridY == ObjectiveY)
        {
            if(tile.tag == "Ground")
            {
                //unit.map.TM.ReplaceWithBeingBuilt(ObjectiveX, ObjectiveY, unit.FactionNum, castle);
                Debug.Log("create");
            }
            else
            {
                tile.GetComponent<FarmBeingBuilt>().Current += Time.deltaTime * SpeedScript.instance.CurrentSpeed;
            }
            
            Debug.Log("found");
        }
        //castle.StartFarmSequence();
        */
    }
}
