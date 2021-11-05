using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class BattleUnit : MonoBehaviour
{
    public int inside;
    public float Speed;

    public enum Direction
    {
        Up = 0,
        UpperRight = 1,
        LowerRight = 2,
        Down = 3,
        LowerLeft = 4,
        UpperLeft = 5,
    }
    public Direction Current;

    public int2 Grid;
    public Vector2 World;
    public bool Friendly;
    private Transform pathParent;

    

    void Start()
    {
        pathParent = GameObject.Find("Paths").transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        int2 Difference = new int2(Grid)
        if ()
        {

        }
        */
    }

    public void SetDirection(int Num)
    {
        Current = (Direction)Num;

        //get number
        //int num = (int)someEnumValue;

    }
}
