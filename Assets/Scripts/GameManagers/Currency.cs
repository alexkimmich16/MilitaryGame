using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    #region Singleton

    public static Currency instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public int Money;
    public Text text; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string SpeedDisplay = "Money: " + Money;
        text.text = SpeedDisplay;
    }
    public void ChangeCurrecy(int moneyChange)
    {
        Money += moneyChange;
    }
}
