using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedScript : MonoBehaviour
{
    #region Singleton

    public static SpeedScript instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public float CurrentSpeed;
    public float BackupSpeed;
    public float Speed1;
    public float Speed2;

    public Text text;

    

    // Update is called once per frame
    void Update()
    {
        if (allegiances.instance.Lists[0].Castles.Count == 0 && GameControl.instance.SpeedWithoutCastle == false)
        {
            Speed0Change();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Speed0Change();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Speed1Change();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Speed2Change();
        }
    }

    public void Speed0Change()
    {
        CurrentSpeed = 0;
        BackupSpeed = 0;
        text.text = "0";
    }

    public void Speed1Change()
    {
        CurrentSpeed = Speed1;
        BackupSpeed = Speed1;
        text.text = "1";
    }

    public void Speed2Change()
    {
        CurrentSpeed = Speed2;
        BackupSpeed = Speed2;
        text.text = "2";
    }
}
