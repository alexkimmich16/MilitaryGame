using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
[System.Serializable]
public class BattleFieldTile
{
    public int TileType;
    public int2 LocalGrid;
    public int2 WorldGrid;
    public int Parent;
}
