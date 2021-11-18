using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class Unit : MonoBehaviour {

	// tileX and tileY represent the correct map-tile position
	// for this piece.  Note that this doesn't necessarily mean
	// the world-space coordinates, because our map might be scaled
	// or offset or something of that nature.  Also, during movement
	// animations, we are going to be somewhere in between tiles.
	private LineRenderer lineRenderer;

	[HideInInspector] public float WorldX;
	[HideInInspector] public float WorldY;

	public int GridX;
	public int GridY;

	public int NowGridX;
	public int NowGridY;

	[HideInInspector] public HexGenerator map;

	[SerializeField]
	public List<float> PathCosts = null;

	[SerializeField]
	public List<int2> Grids = new List<int2>();
	public List<Vector2> CurrentPath = new List<Vector2>();
	public List<int2> SpareHolder = new List<int2>();

	public float moveSpeed;
	[HideInInspector] public bool FirstTransform;

	[HideInInspector] public Transform PathParent;

	public int ColorType;

	public Material[] mat;

	[HideInInspector] public bool Iswalking;

	[HideInInspector] public float PunchedSpeed;

	[HideInInspector] public bool PunchLaunched = false;

	[HideInInspector] public bool Surrounded;

	public enum Factions { FactionA, FactionB, FactionC, FactionD, FactionE, FactionF };

	private Factions factions;

	public int FactionNum;

	[HideInInspector] public bool OnFinish = false;
	public int WalkingTowardsFaction;

	[HideInInspector] public int2 start;
	[HideInInspector] public int2 end;

	public bool Blue;

	public bool Selected;
	public GameObject Boarder;

	//
	public void CheckActive()
    {
		if (Selected == true)
		{
			Boarder.SetActive(true);
		}
		else
		{
			Boarder.SetActive(false);
		}
	}

	public void SetActive(bool Active)
    {
		//Debug.Log("active");
		Selected = Active;
		CheckActive();
	}
	public void SendToStart()
    {
		int ValueInArray = (end.x * map.RealHeight) + end.y;
		int Enemy = map.KingdomSave[ValueInArray];
		List<int2> Path = map.PM.FindPath(end, start, FactionNum, Enemy, false);
		AddToPath(Path);
	}
	
	void Awake()
    {
		map = HexGenerator.instance;
	}

	void Start()
    {
		CheckActive();
		PunchLaunched = false;
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		string IdentifyNumber = "Paths";
		PathParent = GameObject.Find(IdentifyNumber).transform;
		if (gameObject != null)
		{
			TurnGenerator.OnTick2 += delegate (object sender, TurnGenerator.OnTickEventArgs e)
			{
				//&& AllowedToMove == true
				if (this != null)
                {
					MoveCloser();
				}
				
			};
		}
		/*
		Vector2 point = new Vector2(transform.position.x, transform.position.y);
		int layerMask = (1 << 8);
		Collider2D objectHit = Physics2D.OverlapPoint(point, layerMask);
        if (objectHit != null)
        {
			if (FirstTransform == false && objectHit.gameObject.GetComponent<ClickableTile>())
			{
				//Debug.Log(objectHit.gameObject.name);
				//GridX = objectHit.gameObject.GetComponent<ClickableTile>().tileX;
				//GridY = objectHit.gameObject.GetComponent<ClickableTile>().tileY;

				//WorldX = objectHit.gameObject.GetComponent<ClickableTile>().WorldX;
				//WorldY = objectHit.gameObject.GetComponent<ClickableTile>().WorldY;

				NowGridX = GridX;
				NowGridY = GridY;
				FirstTransform = true;
			}
		}
		*/
		
	}

	void Update()
    {
		int currNode = 0;

		while (currNode < CurrentPath.Count - 1)
		{
			Vector3 start = map.TileCoordToWorldCoord(CurrentPath[currNode].x, CurrentPath[currNode].y) +
				new Vector3(0, 0, -0.5f);
			Vector3 end = map.TileCoordToWorldCoord(CurrentPath[currNode + 1].x, CurrentPath[currNode + 1].y) +
				new Vector3(0, 0, -0.5f);

			DrawLine(start, end);
			//Debug.DrawLine(start, end, Color.red);

			currNode++;
		}

        if (OnFinish == true && CurrentPath.Count == 0)
        {
			TacticsManager.instance.TacticsControllers[FactionNum].FinishedPeaceTalk(WalkingTowardsFaction, GetComponent<WalkingPerson>());
			//Debug.Log("ARRIVED!!");
			OnFinish = false;
			WalkingTowardsFaction = 0;

			SendToStart();
		}
	}

	void CheckSurrounded()
    {
		bool SurroundedBool = false;
		for (int i = 0; i < 6; i++)
		{
			if (GridY % 2 == 1)
			{
				int X = GridX + map.neighbourOffsetArrayOdd[i].x;
				int Y = GridY + map.neighbourOffsetArrayOdd[i].y;
				if (X > 0 && X < map.RealWidth && Y > 0 && Y < map.RealHeight)
				{
					//closed
					if (PeopleManager.instance.IntArraySingle.Contains((X * map.RealHeight) + Y) || map.CheckTile(X, Y) == 0 || map.CheckTile(X, Y) == 3)
					{
						Surrounded = true;

					}
					else
					{
						Surrounded = false;
						return;
					}
				}
				else
				{
					return;
				}
			}
			else
			{
				int X = GridX + map.neighbourOffsetArrayEven[i].x;
				int Y = GridY + map.neighbourOffsetArrayEven[i].y;

				if (X > 0 && X < map.RealWidth && Y > 0 && Y < map.RealHeight)
                {
					if (PeopleManager.instance.IntArraySingle.Contains((X * map.RealHeight) + Y) || map.CheckTile(X, Y) == 0 || map.CheckTile(X, Y) == 3)
					{
						Surrounded = false;
						return;
					}
					else
					{
						Surrounded = true;
					}
				}
				else
				{
					return;
				}
			}
		}
	}

	void MoveCloser()
    {
		CheckSurrounded();

		if (CurrentPath.Count > 0)
        {
			Iswalking = true;
		}
        else
        {
			Iswalking = false;
		}
		
		if(PunchLaunched == true)
        {
			transform.position = Vector2.MoveTowards(transform.position, new Vector3(WorldX, WorldY, 0), PunchedSpeed);
        }
        else
        {
			transform.position = Vector2.MoveTowards(transform.position, new Vector3(WorldX, WorldY, 0), moveSpeed * SpeedScript.instance.CurrentSpeed);
		}
		
		
		if (Vector3.Distance(transform.position, new Vector3(WorldX, WorldY, 0)) < 0.1f)
		{
			if (CurrentPath.Count >= 2)
			{
				
				if (PunchLaunched == true)
				{
					PunchLaunched = false;
				}
				
				WorldX = CurrentPath[1].x;
				WorldY = CurrentPath[1].y;

				GridX = Grids[1].x;
				GridY = Grids[1].y;

				NowGridX = Grids[0].x;
				NowGridY = Grids[0].y;

				CurrentPath.RemoveAt(0);
				Grids.RemoveAt(0);
				PathCosts.RemoveAt(0);
			}


		}
		if (CurrentPath.Count == 1)
		{
			if (PunchLaunched == true)
			{
				PunchLaunched = false;
			}
			WorldX = CurrentPath[0].x;
			WorldY = CurrentPath[0].y;

			GridX = Grids[0].x;
			GridY = Grids[0].y;

			NowGridX = Grids[0].x;
			NowGridY = Grids[0].y;

			Grids.Clear();
			CurrentPath.Clear();
			PathCosts.Clear();
		}
	}

	public void AddToPath(List<int2> VectorList)
	{
		//SpareHolder = VectorList;
		Grids.Clear();
		CurrentPath.Clear();
		PathCosts.Clear();
		for (int i = 0; i < VectorList.Count; i++)
		{
			Vector2 NextPath = map.GetPosition(VectorList[i].x, VectorList[i].y);
			CurrentPath.Add(NextPath);
			int2 newInt = new int2(VectorList[i].x, VectorList[i].y);
			Grids.Add(newInt);
			PathCosts.Add(map.CostOfTile(newInt.x, newInt.y));
		}
		start = new int2(VectorList[0].x, VectorList[0].y);
		end = new int2(VectorList[VectorList.Count - 1].x, VectorList[VectorList.Count - 1].y);
	}

	public void RemoveFromArmyList()
	{
		if (factions == Factions.FactionA)
		{
			allegiances.instance.Lists[1].ArmyTransforms.Remove(transform);
		}
		else if (factions == Factions.FactionB)
		{
			allegiances.instance.Lists[2].ArmyTransforms.Remove(transform);
		}
		else if (factions == Factions.FactionC)
		{
			allegiances.instance.Lists[3].ArmyTransforms.Remove(transform);
		}
		else if (factions == Factions.FactionD)
		{
			allegiances.instance.Lists[4].ArmyTransforms.Remove(transform);
		}
		else if (factions == Factions.FactionE)
		{
			allegiances.instance.Lists[5].ArmyTransforms.Remove(transform);
		}
		else if (factions == Factions.FactionF)
		{
			allegiances.instance.Lists[6].ArmyTransforms.Remove(transform);
		}
	}
	//a = 1
	//f = 6

	public void SetFaction(int num)
	{
		if (num == 1)
		{
			factions = Factions.FactionA;

		}
		else if (num == 2)
		{
			factions = Factions.FactionB;
		}
		else if (num == 3)
		{
			factions = Factions.FactionC;
		}
		else if (num == 4)
		{
			factions = Factions.FactionD;
		}
		else if (num == 5)
		{
			factions = Factions.FactionE;
		}
		else if (num == 6)
		{
			factions = Factions.FactionF;
		}
		FactionNum = num;
	}

	void DrawLine(Vector3 start, Vector3 end)
	{
		float duration = 0.05f;
		GameObject myLine = new GameObject();
		myLine.transform.parent = PathParent;
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();

		LineRenderer lr = myLine.GetComponent<LineRenderer>();
        if (Blue == true)
        {
			lr.SetColors(Color.blue, Color.blue);
		}
        else
        {
			lr.SetColors(Color.red, Color.red);
		}
		
		lr.SetWidth(map.LineWidth, map.LineWidth);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

		lr.sharedMaterials = mat;
		//lr.sharedMaterials.color = Color.white;

		GameObject.Destroy(myLine, duration);
	}
}
