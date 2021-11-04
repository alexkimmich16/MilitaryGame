using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class SendMenu : MonoBehaviour
{
    public int KnightCount;
    public int ArcherCount;
    public int CalvalryCount;
    public TextMeshProUGUI KnightText;
    public TextMeshProUGUI ArcherText;
    public TextMeshProUGUI CalvalryText;
    public int2 Objective;

    public bool SendOnClick;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        KnightText.text = KnightCount + "";
        ArcherText.text = ArcherCount + "";
        CalvalryText.text = CalvalryCount + "";
    }
    public void RecieveObjective(int2 objective)
    {
        Objective = objective;
    }

    public void Add(int Num)
    {
        if (Num == 1)
        {
            KnightCount += 1;
        }
        else if (Num == -1)
        {
            KnightCount -= 1;
        }

        if (Num == 2)
        {
            ArcherCount += 1;
        }
        else if (Num == -2)
        {
            ArcherCount -= 1;
        }

        if (Num == 3)
        {
            CalvalryCount += 1;
        }
        else if (Num == -3)
        {
            CalvalryCount -= 1;
        }
    }

    public void Send()
    {
        //Debug.Log("send1");

        //Objective = 

        CastleDisplay.instance.CurrentCastle.SendArmy(KnightCount, ArcherCount, CalvalryCount, SendOnClick, Objective);
        //CastleDisplay.instance.CurrentCastle.SendPeople(Count, UIManager.instance.Tile.tileX, UIManager.instance.Tile.tileY);
        //CastleDisplay.instance.CurrentCastle.Peasants -= Count;
        //Count =0;
        UIManager.instance.SendMenuOpen(false);
    }
}
