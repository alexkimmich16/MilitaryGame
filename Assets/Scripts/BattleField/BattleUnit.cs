using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class BattleUnit : MonoBehaviour
{
    public int inside;
    public float Speed;

	[HideInInspector] public HexGenerator map;

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
    public bool Active;
	[HideInInspector] public bool Iswalking;
	[HideInInspector] public Transform PathParent;

	public List<NodeInfo> Nodes = new List<NodeInfo>();

	[HideInInspector] public int2 start;
	[HideInInspector] public int2 end;

	[HideInInspector] public float WorldX;
	[HideInInspector] public float WorldY;

	public int GridX;
	public int GridY;

	public int NowGridX;
	public int NowGridY;

	public Material[] mat;

	public void SetActive(bool active)
    {
        Active = active;
    }

    // Update is called once per frame
    void Update()
    {
        int currNode = 0;

        while (currNode < Nodes.Count - 1)
        {
            Vector3 start = new Vector3(Nodes[currNode].World.x, Nodes[currNode].World.y, -0.5f);
            Vector3 end = new Vector3(Nodes[currNode + 1].World.x, Nodes[currNode + 1].World.y, -0.5f);

			DrawLine(start, end);
            //Debug.DrawLine(start, end, Color.red);

            currNode++;
        }
    }

    public void SetDirection(int Num)
    {
        Current = (Direction)Num;

        //get number
        //int num = (int)someEnumValue;

    }

	void Awake()
	{
		map = HexGenerator.instance;
	}

	void Start()
	{
		pathParent = GameObject.Find("Paths").transform;
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

	void MoveCloser()
	{
		if (Nodes.Count > 0)
		{
			Iswalking = true;
		}
		else
		{
			Iswalking = false;
		}


		if (Vector3.Distance(transform.position, new Vector3(WorldX, WorldY, 0)) < 0.1f)
		{
			if (Nodes.Count >= 2)
			{
				WorldX = Nodes[1].World.x;
				WorldY = Nodes[1].World.y;

				GridX = Nodes[1].Grid.x;
				GridY = Nodes[1].Grid.y;

				NowGridX = Nodes[0].Grid.x;
				NowGridY = Nodes[0].Grid.y;

				Nodes.RemoveAt(0);
			}


		}
		if (Nodes.Count == 1)
		{
			WorldX = Nodes[0].World.x;
			WorldY = Nodes[0].World.y;

			GridX = Nodes[0].Grid.x;
			GridY = Nodes[0].Grid.y;

			NowGridX = Nodes[0].Grid.x;
			NowGridY = Nodes[0].Grid.y;

			Nodes.Clear();
		}
	}

	public void AddToPath(List<int2> VectorList)
	{
		Nodes.Clear();
		for (int i = 0; i < VectorList.Count; i++)
		{
			Nodes.Add(new NodeInfo());
			Nodes[i].World = map.GetPosition(VectorList[i].x, VectorList[i].y);
			Nodes[i].Grid = new int2(VectorList[i].x, VectorList[i].y);
		}
		start = new int2(VectorList[0].x, VectorList[0].y);
		end = new int2(VectorList[VectorList.Count - 1].x, VectorList[VectorList.Count - 1].y);
	}
	//a = 1
	//f = 6

	void DrawLine(Vector3 start, Vector3 end)
	{
		float duration = 0.05f;
		GameObject myLine = new GameObject();
		myLine.transform.parent = PathParent;
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();

		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.SetColors(Color.blue, Color.blue);

		lr.SetWidth(map.LineWidth, map.LineWidth);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

		lr.sharedMaterials = mat;
		//lr.sharedMaterials.color = Color.white;

		GameObject.Destroy(myLine, duration);
	}
}
