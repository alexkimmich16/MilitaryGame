using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Mech : MonoBehaviour
{
    /*
	public Unit unit;
	public List<int2> neighbourOffsetArrayEven = new List<int2>();
	public List<int2> neighbourOffsetArrayOdd = new List<int2>();

	public List<int2> EvenPushArray = new List<int2>();
	public List<int2> OddPushArray = new List<int2>();

	public bool Moving;
	public bool Even;

	public bool IsSelected;

	public Transform Target;

	public int Damage = 3;

	public bool CanPunch;

	public float Count;
	public int Max;

	SpriteRenderer SR;

	public int Health;
	public int MaxHealth;

	public float ColorValue;
	public float Fallspeed;

	public int multiplier;

	public GameObject Boarder;



	void Start()
    {
		Health = MaxHealth;
		SR = gameObject.GetComponent<SpriteRenderer>();
		//ClickScript.instance.Mechs.Add(this);
	}

	public void Attack(Transform target)
	{
		Target = target;
	}

	public void Punch(int Direction, int Enemy, bool Even)
    {
		//Transform Enemytrans = PeopleSpawner.instance.Enemies[Enemy];
		//Transform Enemytrans = Target;
		Unit Enemyunit = Target.GetComponent<Unit>();
		Enemyunit.transform.position = new Vector3(Enemyunit.WorldX, Enemyunit.WorldY, 1);

		if (Even == true)
        {
			//findTile
			int X = Enemyunit.GridX;
			int Y = Enemyunit.GridY;
			bool EvenSave = true;
			for (int i = 0; i < multiplier; i++)
            {
                if (EvenSave == true)
                {
					X += neighbourOffsetArrayEven[Direction].x;
					Y += neighbourOffsetArrayEven[Direction].y;
					EvenSave = false;
				}
                else
                {
					X += neighbourOffsetArrayOdd[Direction].x;
					Y += neighbourOffsetArrayOdd[Direction].y;
					EvenSave = true;
				}
				
			}
			


			//Transform tile = GameObject.Find(X + " " + Y).transform;
			//ClickableTile CT = tile.GetComponent<ClickableTile>();


			//damage
			int TileNum = unit.map.CheckTile(X, Y);
			if (TileNum == 3)
            {
				Target.GetComponent<Skeleton>().AddDamage(1);
			}
			else if(TileNum == 1)
            {
				Target.GetComponent<Skeleton>().AddDamage(1);
			}

			//AddToArray
			List<int2> VectorList = new List<int2>();
			//VectorList.Add(new int2(CT.tileX, CT.tileY));
			//VectorList.Add(new int2(CT.tileX, CT.tileY));
			//Enemyunit.AddToPath(VectorList);
			//Enemyunit.PunchLaunched = true;
		}
        else
        {
			//findTile
			int X = Enemyunit.GridX;
			int Y = Enemyunit.GridY;
			bool EvenSave = false;
			for (int i = 0; i < multiplier; i++)
			{
				if (EvenSave == true)
				{
					X += neighbourOffsetArrayEven[Direction].x;
					Y += neighbourOffsetArrayEven[Direction].y;
					EvenSave = false;
				}
				else
				{
					X += neighbourOffsetArrayOdd[Direction].x;
					Y += neighbourOffsetArrayOdd[Direction].y;
					EvenSave = true;
				}

			}

			Transform tile = GameObject.Find(X + " " + Y).transform;
			ClickableTile CT = tile.GetComponent<ClickableTile>();

			int TileNum = unit.map.CheckTile(X, Y);
			if (TileNum == 3)
			{
				Target.GetComponent<Skeleton>().AddDamage(1);
			}
			else if (TileNum == 1)
			{
				Target.GetComponent<Skeleton>().AddDamage(1);
			}

			//AddToArray
			List<int2> VectorList = new List<int2>();
			VectorList.Add(new int2(CT.tileX, CT.tileY));
			VectorList.Add(new int2(CT.tileX, CT.tileY));
			Enemyunit.AddToPath(VectorList);
			Enemyunit.PunchLaunched = true;


		}
		Target.GetComponent<Skeleton>().AddDamage(Damage);
	}

	// Update is called once per frame
	void Update()
    {
		if (IsSelected == true)
		{
			Boarder.SetActive(true);
		}
		else
		{
			Boarder.SetActive(false);
		}

		if (Target != null)
        {
			Vector3 start = unit.map.TileCoordToWorldCoord(transform.position.x, transform.position.y) +
					new Vector3(0, 0, -0.5f);
			Vector3 end = unit.map.TileCoordToWorldCoord(Target.position.x, Target.position.y) +
				new Vector3(0, 0, -0.5f);
			DrawLine(start, end);
		}

			


		ColorValue += Fallspeed;

		Color NewColor = new Color(1, ColorValue, ColorValue);
		SR.color = NewColor;

		if (unit.GridY % 2 == 1)
		{
			Even = false;
		}
		else
		{
			Even = true;
		}

		Count += Time.deltaTime;
		if (Count > Max)
		{
			//Debug.Log("Mech1");
			if (Moving == false)
            {
				//Debug.Log("Mech2");
				if (Target != null)
                {
					Debug.Log("Mech3");
					for (int j = 0; j < allegiances.instance.Lists.Count - 1; j++)
                    {
						if (allegiances.instance.Lists[j + 1].ArmyTransforms.Contains(Target) && allegiances.instance.Lists[j + 1].State[0] == -1)
                        {
							Debug.Log("Mech3.5");
							if (Even == true)
							{
								Debug.Log("Mech4");
								for (int i = 0; i < allegiances.instance.Lists[j + 1].ArmyTransforms.Count; i++)
								{
									for (int x = 0; x < neighbourOffsetArrayEven.Count; x++)
									{
										Unit Enemy = Target.GetComponent<Unit>();
										if (Enemy.GridX == unit.NowGridX + neighbourOffsetArrayEven[x].x && Enemy.NowGridY == unit.GridY + neighbourOffsetArrayEven[x].y && Enemy.CurrentPath.Count == 0)
										{
											CanPunch = true;
											Punch(x, i, true);
											Count = 0;
										}
										//Unit Enemy = PeopleSpawner.instance.Enemies[i].GetComponent<Unit>();
										//if (Enemy.GridX == unit.NowGridX + neighbourOffsetArrayEven[x].x && Enemy.NowGridY == unit.GridY + neighbourOffsetArrayEven[x].y && Enemy.CurrentPath.Count == 0)

									}

								}


							}
							else
							{
								Debug.Log("Mech4");
								for (int i = 0; i < allegiances.instance.Lists[j + 1].ArmyTransforms.Count; i++)
								{
									for (int x = 0; x < neighbourOffsetArrayOdd.Count; x++)
									{
										Unit Enemy = Target.GetComponent<Unit>();
										if (Enemy.NowGridX == unit.NowGridX + neighbourOffsetArrayOdd[x].x && Enemy.NowGridY == unit.NowGridY + neighbourOffsetArrayOdd[x].y && Enemy.CurrentPath.Count == 0)
										{
											//Debug.Log("Mech5");
											CanPunch = true;
											Punch(x, i, false);
											Count = 0;
										}
									}

								}
							}
						}

					}
						
					
				}
            }
			

		}
        else
        {
			CanPunch = false;
			if (Count > Max)
            {
				//Count = 0;
			}
		}


		

		if (unit.Grids.Count > 0)
        {
			Moving = true;
		}
        else
        {
			Moving = false;
		}

		
		
    }

	public void AddDamage(int Damage)
	{
		ColorValue -= ColorValue;
		Health -= Damage;
	}

	void DrawLine(Vector3 start, Vector3 end)
	{
		//Color color = color.red;
		float duration = 0.2f;
		GameObject myLine = new GameObject();
		myLine.transform.parent = unit.PathParent;
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();

		lr.SetColors(Color.red, Color.red);
		lr.SetWidth(0.1f, 0.1f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

		lr.sharedMaterials = unit.mat;
		GameObject.Destroy(myLine, duration);
	}

	public void ChangeBool()
	{
		IsSelected = !IsSelected;
	}
	public void ClearActive()
	{
		IsSelected = false;
	}
	*/
}
