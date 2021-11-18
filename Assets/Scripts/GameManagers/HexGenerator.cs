using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using GD.MinMaxSlider;


public class HexGenerator : MonoBehaviour
{
	#region Singleton

	public static HexGenerator instance;

	void Awake()
	{
		instance = this;
	}

	#endregion

	[Header("Generator Stats")]
	public int Width;
    public int Height;
	public float Scale;
    public float Decay;

    [MinMaxSlider(0f, 1f)]
    public Vector2 MountainThreshhold, DirtThreshhold, SandThreshhold, WaterThreshhold;
	//mount 0.339
	//dirt 0.04
	//sand 0.02


	[Header("PivateInfo")]
	[HideInInspector]
	public int RealWidth;
	[HideInInspector]
	public int RealHeight;

    [Header("Prefabs")]
	public Tilemap mountainMap;
	//public Tilemap FogMap;

	[Header("Others")]
	public Transform parentOBJ;
	private float Elapsed;
	[HideInInspector] public GameObject selectedUnit;
	public Transform PathParent;

	[Header("High Lists")]
	public List<int> KingdomSave;
	public List<int> TileSave = new List<int>();
	public List<float> TerrainValues = new List<float>();
	public TileType[] tileTypes;
	public GameObject[,] TileTwoArray;
	public List<Vector2> TilesPos = new List<Vector2>();

	[Header("Scripts")]
	public TileManager TM;
	public PathfindingManager PM;
	public Noise noise;

	[Header("Low Lists")]
	//public NativeArray<int> BasicTileSave;
	
	public List<int2> neighbourOffsetArrayEven = new List<int2>();
	public List<int2> neighbourOffsetArrayOdd = new List<int2>();
	public Material[] mat;

	[SerializeField]
	public int[,] tiles;

	private List<Vector2> Corners = new List<Vector2>();

	private int Zint = 0;

	public float YDrawOffset;
	public float LineWidth;


	public void GenerateMap()
    {
		//how x is hight, width is y
		//RealWidth = Width / 2;
		RealWidth = Height;
		//RealHeight = Height * 2;
		RealHeight = Width; 
		tiles = new int[RealWidth, RealHeight];

		Vector3 Maxplace = mountainMap.CellToWorld(new Vector3Int(RealWidth, RealHeight, 0));
		float CenterX = Maxplace.x / 2;
		float CenterY = Maxplace.y / 2;
		Vector2 Center = new Vector2(CenterX, CenterY);
		bool First = true;
		for (int x = 0; x < RealWidth; x++)
		{
			for (int y = 0; y < RealHeight; y++)
			{
				Vector3 place = mountainMap.CellToWorld(new Vector3Int(x, y, 0));
				Vector2 position = new Vector2(place.x, place.y);

				//get tilepoint
				float Distence = Vector2.Distance(Center, position);
				float Value = noise.GetValue(position.x, position.y, Scale);
				Value -= Distence / Decay;

				//set max
				
				if (Value < 0)
                {
					//Value = 0.9999f;

				}

				int ValueInArray = (x * RealHeight) + y;
				
				//if nothing else
				int CurrentTile = 1;
				
				if (Value > MountainThreshhold.x && Value < MountainThreshhold.y)
				{
					CurrentTile = 0;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[0].tile);
				}
				else if (Value > DirtThreshhold.x && Value < DirtThreshhold.y)
				{
					CurrentTile = 1;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[1].tile);
				}
				else if (Value > SandThreshhold.x && Value < SandThreshhold.y)
				{
					CurrentTile = 2;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[2].tile);
				}
                else
                {
					CurrentTile = 3;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[3].tile);
				}
				/*
				else if (Value > WaterThreshhold.x && Value < WaterThreshhold.y)
				{
					CurrentTile = 3;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[3].tile);
				}*/
				//mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[1].tile);

				TerrainValues.Insert(ValueInArray, Value);
				TileSave.Insert(ValueInArray, CurrentTile);
				tiles[x, y] = CurrentTile;

				Vector3Int coordinate = new Vector3Int(x, y, 0);
				Vector3 TilePos = mountainMap.CellToWorld(coordinate);
				TilePos.y += YDrawOffset;
				TilesPos[ValueInArray] = TilePos;

				TileType tt = tileTypes[tiles[x, y]];
				
			}
		}
	}

	#region Basics


	public void ChangeTile(int x, int y, int Type, int NewKingdom)
	{
		int ValueInArray = (x * RealHeight) + y;
		tiles[x, y] = Type;
		KingdomSave[ValueInArray] = NewKingdom;
	}

	public int CheckTile(int x, int y)
	{
		return (tiles[x, y]);
	}

	public void ChangeColor(int x, int y, Color color)
	{
		//Debug.Log("change color");
		mountainMap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
		mountainMap.SetColor(new Vector3Int(x, y, 0), color);
	}
	public int GetTileFaction(int x, int y)
	{
		int Num = (x * RealHeight) + y;
		return KingdomSave[Num];
	}
	public Vector2 GetPosition(int x, int y)
	{
		int Num = (x * RealHeight) + y;
		return TilesPos[Num];
	}
	public void InitialiseCapacity(int Capacity)
	{
		for (int x = 0; x < Capacity; x++)
		{
			KingdomSave.Add(0);
			TilesPos.Add(new Vector2(0, 0));
		}
	}

	public float CostOfTile(int targetX, int targetY)
    {
		TileType tt = tileTypes[tiles[targetX, targetY]];
		return tt.movementCost;
	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
	{

		TileType tt = tileTypes[tiles[targetX, targetY]];

		float cost = tt.movementCost;

		if (sourceX != targetX && sourceY != targetY)
		{
			// We are moving diagonally!  Fudge the cost for tie-breaking
			// Purely a cosmetic thing!
			cost += 0.001f;
		}

		return cost;

	}

	public float GetTerrainValue(int x, int y)
    {
		int Num = (x * RealHeight) + y;
		return TerrainValues[Num];
	}

	public Vector3 TileCoordToWorldCoord(float x, float y)
	{
		return new Vector3(x, y, 0);
	}

	public bool CheckForBuilding(int x, int y)
	{
		if (tiles[x, y] == 4)
		{
			return true;
		}
        else
        {
			return false;
		}
	}

	

	#endregion
}
