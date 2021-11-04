using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleButton : MonoBehaviour
{
    public Person Current;

    public void RemoveFromCastle()
    {
        CastleDisplay.instance.CurrentCastle.RemoveFromCastle(Current);
        CastleDisplay.instance.ChangeLists();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
