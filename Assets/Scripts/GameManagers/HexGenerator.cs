using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;


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

    [Range(0f, 1f)]
    public float MountainThreshhold, DirtThreshhold, SandThreshhold;

    [Header("PivateInfo")]
	[HideInInspector]
	public int RealWidth;
	[HideInInspector]
	public int RealHeight;

    [Header("Prefabs")]
	//public Tile mountainTile;
	//public Tile sandTile, grassTile, waterTile, FriendlyBarracks, EnemyBarracks, Farm;

	public Tilemap mountainMap;
	//public Tilemap FogMap;

	[Header("Others")]
	public Transform parentOBJ;
	private float Elapsed;
	[HideInInspector] public GameObject selectedUnit;
	[HideInInspector] public Vector3 finalVector3;
	[HideInInspector] public int SpareX;
	[HideInInspector] public int SpareY;
	[HideInInspector] public int2 startPosition;
	[HideInInspector] public int2 endPosition;
	[HideInInspector] public int2 Current;
	public Transform PathParent;

	[Header("High Lists")]
	public List<int> KingdomSave;
	public List<int> TileSave = new List<int>();
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
	public int GetTileFaction(int x,int y)
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
				
				int ValueInArray = (x * RealHeight) + y;


				int CurrentTile;
				if (Value > MountainThreshhold)
				{
					CurrentTile = 0;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[0].tile);
				}
				else if (Value > DirtThreshhold)
				{
					CurrentTile = 1;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[1].tile);
				}
				else if (Value > SandThreshhold)
				{
					CurrentTile = 2;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[2].tile);
				}
				else
				{
					CurrentTile = 3;
					mountainMap.SetTile(new Vector3Int(x, y, 0), tileTypes[3].tile);
				}
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

	#region CurrentUseless

	void DrawLine(Vector3 start, Vector3 end)
	{
		GameObject myLine = new GameObject();
		myLine.transform.parent = PathParent;
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();

		LineRenderer lr = myLine.GetComponent<LineRenderer>();

		lr.SetColors(Color.red, Color.red);
		lr.SetWidth(0.3f, 0.3f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

		lr.sharedMaterials = mat;
		//lr.sharedMaterials.color = Color.white;
	}
	/*
	public void CreateMapLines()
    {
		for (int x = 0; x < RealWidth; x++)
        {
			for (int y = 0; y < RealHeight; y++)
            {
                if (y % 2 == 1)
                {
					for (int i = 0; i < neighbourOffsetArrayEven.Count; i++)
					{
						int ThisNum = (x * RealHeight) + y;
						int OffsetNum = (neighbourOffsetArrayEven[i].x + x * RealHeight) + neighbourOffsetArrayEven[i].y + y;

                        //different kingdom
                        //Debug.Log("this: " + ThisNum + "  other: " + OffsetNum);
                        if (neighbourOffsetArrayEven[i].x + x > -1 && neighbourOffsetArrayEven[i].y + y > -1)
                        {
							if (TileObject[ThisNum].Kingdom != TileObject[OffsetNum].Kingdom)
							{
								//Debug.Log("newpoath");
								if (i == 0)
								{
									Vector3 Start = new Vector3(Corners[1].x + TileObject[ThisNum].transform.position.x, Corners[1].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[2].x + TileObject[ThisNum].transform.position.x, Corners[2].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 1)
								{
									Vector3 Start = new Vector3(Corners[4].x + TileObject[ThisNum].transform.position.x, Corners[4].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[5].x + TileObject[ThisNum].transform.position.x, Corners[5].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 2)
								{
									Vector3 Start = new Vector3(Corners[5].x + TileObject[ThisNum].transform.position.x, Corners[5].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[0].x + TileObject[ThisNum].transform.position.x, Corners[0].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 3)
								{
									Vector3 Start = new Vector3(Corners[2].x + TileObject[ThisNum].transform.position.x, Corners[2].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[3].x + TileObject[ThisNum].transform.position.x, Corners[3].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 4)
								{
									Vector3 Start = new Vector3(Corners[0].x + TileObject[ThisNum].transform.position.x, Corners[0].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[1].x + TileObject[ThisNum].transform.position.x, Corners[1].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 5)
								{
									Vector3 Start = new Vector3(Corners[3].x + TileObject[ThisNum].transform.position.x, Corners[3].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[4].x + TileObject[ThisNum].transform.position.x, Corners[4].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
							}
						}
						
					}
				}
                else
                {
					for (int i = 0; i < neighbourOffsetArrayOdd.Count; i++)
					{
						int ThisNum = (x * RealHeight) + y;
						int OffsetNum = (neighbourOffsetArrayOdd[i].x + x * RealHeight) + neighbourOffsetArrayOdd[i].y + y;
						if (neighbourOffsetArrayOdd[i].x + x > -1 && neighbourOffsetArrayOdd[i].y + y > -1)
						{
							if (TileObject[ThisNum].Kingdom != TileObject[OffsetNum].Kingdom)
							{
								//Debug.Log("newpoath");
								if (i == 0)
								{
									//up
									Vector3 Start = new Vector3(Corners[0].x + TileObject[ThisNum].transform.position.x, Corners[0].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[1].x + TileObject[ThisNum].transform.position.x, Corners[1].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 1)
								{  
									//down
									Vector3 Start = new Vector3(Corners[3].x + TileObject[ThisNum].transform.position.x, Corners[3].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[4].x + TileObject[ThisNum].transform.position.x, Corners[4].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 2)
								{
									//down left
									Vector3 Start = new Vector3(Corners[2].x + TileObject[ThisNum].transform.position.x, Corners[2].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[3].x + TileObject[ThisNum].transform.position.x, Corners[3].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 3)
								{
									//down right
									Vector3 Start = new Vector3(Corners[5].x + TileObject[ThisNum].transform.position.x, Corners[5].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[0].x + TileObject[ThisNum].transform.position.x, Corners[0].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 4)
								{
									//up right
									Vector3 Start = new Vector3(Corners[4].x + TileObject[ThisNum].transform.position.x, Corners[4].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[5].x + TileObject[ThisNum].transform.position.x, Corners[5].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
								else if (i == 5)
								{
									//down right
									Vector3 Start = new Vector3(Corners[2].x + TileObject[ThisNum].transform.position.x, Corners[2].y + TileObject[ThisNum].transform.position.y, 0);
									Vector3 End = new Vector3(Corners[3].x + TileObject[ThisNum].transform.position.x, Corners[3].y + TileObject[ThisNum].transform.position.y, 0);
									DrawLine(Start, End);
								}
							}
						}
					}
				}
				
			}
		}
	}
	*/

	#endregion

	#region Basics

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
