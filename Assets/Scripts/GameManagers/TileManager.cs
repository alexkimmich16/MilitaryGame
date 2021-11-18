using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;


public class TileManager : MonoBehaviour
{
	public HexGenerator map;

	public int BarracksRange;
	public bool Restart;

	public GameObject castlesObj;

	public bool UseSurrounding;

	public int SpawnSizeLimit;

	#region Replacements

	public void SpawnEnemyCastle(int x, int y, int Faction)
	{
		//Debug.Log("pt1");
		Castle caslte = castlesObj.AddComponent<Castle>();
		map.mountainMap.SetTile(new Vector3Int(x, y, 0), map.tileTypes[5].tile);
		map.ChangeColor(x, y, Color.red);

		//Debug.Log(x + " " + y + "faction:  " + Faction);
		if (y % 2 != 1)
		{
			caslte.neighbours = map.neighbourOffsetArrayEven;
		}
		else
		{
			caslte.neighbours = map.neighbourOffsetArrayOdd;
		}

		for (int j = 0; j < caslte.neighbours.Count; j++)
		{
			int2 spot = new int2(x + caslte.neighbours[j].x, y + caslte.neighbours[j].y);
			//Debug.Log(spot.x + " " + spot.y + "faction:  " + Faction + "type: " + map.CheckTile(spot.x, spot.y));
		}
		//Debug.Log("pt2");
		map.ChangeTile(x, y, 6, Faction);
		caslte.SetFaction(Faction);
		caslte.MyPos = new int2(x,y);
		//Debug.Log("pt3");
		allegiances.instance.Lists[Faction].Castles.Add(caslte);
		//Debug.Log("spawn castle3");
	}

	public void ReplaceWithFarm(int x, int y, int TileNum, Castle castle)
	{
		map.mountainMap.SetTile(new Vector3Int(x, y, 0), map.tileTypes[6].tile);
		map.mountainMap.SetColor(new Vector3Int(x, y, 0), Color.yellow);
		map.ChangeTile(x, y, 5, TileNum);
		if (y % 2 != 1)
		{
			//farm.SpawnSpots = map.neighbourOffsetArrayEven;
		}
		else
		{
			//farm.SpawnSpots = map.neighbourOffsetArrayOdd;
		}
		//allegiances.instance.Lists[TileNum].Castles[0].Farms.Add(CT.GetComponent<Farm>());
	}

	public void SpawnPlayerCastle(int x, int y)
	{
		map.mountainMap.SetTile(new Vector3Int(x, y, 0), map.tileTypes[4].tile);
		map.ChangeColor(x, y, Color.red);
		map.ChangeTile(x, y, 6, 0);
		Castle caslte = castlesObj.AddComponent<Castle>();
		caslte.SetFaction(0);
		caslte.MyPos = new int2(x, y);
		//Debug.Log("pt1");
		for (int j = 0; j < allegiances.instance.People[0].People.Count; j++)
		{
			caslte.Inside.Add(allegiances.instance.People[0].People[j]);
		}

		if (y % 2 != 1)
		{
			//Debug.Log("pt2.1");
			caslte.neighbours = map.neighbourOffsetArrayEven;
		}
		else
		{
			//Debug.Log("pt2.2");
			caslte.neighbours = map.neighbourOffsetArrayOdd;
		}
		allegiances.instance.Lists[0].Castles.Add(caslte);
	}

	#endregion

	public void Reload()
    {
		if(Restart == true)
        {
			SceneLoader.instance.LoadScene(1);
		}
		
	}
	
	public bool BarracksRequirements(int x, int y, int Faction, int MinWalkableBoarders)
    {
		List<int2> neighbourOffset;
		
		if (y % 2 != 1)
		{
			neighbourOffset = map.neighbourOffsetArrayEven;
		}
		else
		{
			neighbourOffset = map.neighbourOffsetArrayOdd;
		}
		//if out of map
		// change to actual mapsize
		for (int i = 0; i < neighbourOffset.Count; i++)
		{
			int XConverted = x + neighbourOffset[i].x;
			int YConverted = y + neighbourOffset[i].y;
			if (XConverted < 0 || XConverted > map.RealWidth || YConverted < 0 || YConverted > map.RealHeight)
			{
				return false;
			}
		}
		int BoarderCount = 0;
		//if this tile is ok,
		//Debug.Log("pt1");
		if (map.tiles[x, y] == 1 || map.tiles[x, y] == 2)
		{
			//Debug.Log("pt2");
			int FactionTried = map.GetTileFaction(x, y);
			//Debug.Log(FactionTried + " " + Faction);
			if (FactionTried == Faction)
			{
				//Debug.Log("pt3");
				//find neibors
				if (UseSurrounding == true)
				{
					//Debug.Log("pt4.1");
					for (int i = 0; i < neighbourOffset.Count; i++)
                    {
						//Debug.Log("pt5.1");
						int XConverted = x + neighbourOffset[i].x;
						int YConverted = y + neighbourOffset[i].y;
						if (map.tiles[XConverted, YConverted] == 1 || map.tiles[XConverted, YConverted] == 2)
						{
							BoarderCount += 1;
						}
					}
					if (MinWalkableBoarders <= BoarderCount)
					{
						//Debug.Log("pt5.2");
						return true;
					}
				}
				else
				{
					//Debug.Log("pt4.2");
					return true;
				}
			}
		}
		
		return false;
	}

	public void CreateOneEnemyBarracks(int Faction, int MinWalkableBoarders)
    {
		//random
		List<int2> GoodSpawns = new List<int2>();

		//if not random, check EVERY TILE
		for (int x = 0; x < map.RealWidth; x++)
        {
			for (int y = 0; y < map.RealHeight; y++)
			{
				bool reqirements = BarracksRequirements(x, y, Faction, MinWalkableBoarders);
				//Debug.Log(reqirements);
				if (reqirements == true)
                {
                    //Debug.Log("true");
                    if (x > SpawnSizeLimit && x < map.RealWidth - SpawnSizeLimit && y > SpawnSizeLimit && y < map.RealHeight - SpawnSizeLimit)
                    {
						GoodSpawns.Add(new int2(x, y));
					}
					
					//SpawnEnemyCastle(x, y, Faction);
				}
                else
                {
					//Debug.Log("false");
				}
			}
		}

		int RandomSelect = Random.Range(0, GoodSpawns.Count);
		int x2 = GoodSpawns[RandomSelect].x;
		int y2 = GoodSpawns[RandomSelect].y;
		SpawnEnemyCastle(x2, y2, Faction);
	}

	public void CreateBarracks(int FactionsMax, int MinWalkableBoarders)
	{
		//do random, then if can't find cycle through
		for (int f = 0; f < FactionsMax; f++)
        {
			CreateOneEnemyBarracks(f + 1, MinWalkableBoarders);
		}
		
	}

	public void FindBuildings(Transform Object, int Type)
	{
		//Debug.Log("FIndPath2");
		/*
		List<Transform> distences = new List<Transform>();
		List<int2> Grids = new List<int2>();
		List<bool> IsTile = new List<bool>();
		List<Transform> targets = new List<Transform>();
		List<int> Friendly = new List<int>();

		for (int x = 0; x < map.RealWidth; x++)
		{
			for (int y = 0; y < map.RealHeight; y++)
			{
				if (map.tiles[x, y] == 4 || map.tiles[x, y] == 5 || map.tiles[x, y] == 6)
				{
					Transform TileOBJ = HexGenerator.instance.TileTwoArray[x, y].transform;
					distences.Add(TileOBJ);
					Grids.Add(new int2(x, y));
					IsTile.Add(false);
					targets.Add(null);
					//ifENemyISwhatever
				}
			}
		}

		//if enemy of this perticular army
		for (int i = 0; i < allegiances.instance.Lists.Count; i++)
		{
			if (allegiances.instance.Lists[Type].State[i] == -1)
			{
				for (int k = 0; k < allegiances.instance.Lists[i].ArmyTransforms.Count; k++)
				{
					if (allegiances.instance.Lists[i].ArmyTransforms[k] != null && allegiances.instance.Lists[i].ArmyTransforms[k] != Object)
					{
						if (allegiances.instance.Lists[i].ArmyTransforms[k].GetComponent<Unit>().GridY % 2 == 1)
						{
							for (int j = 0; j < map.neighbourOffsetArrayEven.Count; j++)
							{
								if (allegiances.instance.Lists[i].ArmyTransforms[k] != null)
								{
									int NewX = allegiances.instance.Lists[i].ArmyTransforms[k].GetComponent<Unit>().GridX + map.neighbourOffsetArrayOdd[j].x;
									int NewY = allegiances.instance.Lists[i].ArmyTransforms[k].GetComponent<Unit>().GridY + map.neighbourOffsetArrayOdd[j].y;

									if (NewX > 0 && NewY > 0)
									{
										if (map.tiles[NewX, NewY] != 3 || map.tiles[NewX, NewY] != 0)
										{
											Transform TileOBJ = HexGenerator.instance.TileTwoArray[NewX, NewY].transform;
											if (PeopleManager.instance.IntArraySingle.Contains((NewX * map.RealHeight) + NewY))
											{

											}
											else
											{
												if (TileOBJ != null)
												{
													distences.Add(TileOBJ);
													Grids.Add(new int2(NewX, NewY));
													IsTile.Add(true);
													targets.Add(allegiances.instance.Lists[i].ArmyTransforms[k]);
												}
											}
										}
									}
								}
								//if

								//check mountains
							}
						}
						else
						{
							for (int j = 0; j < map.neighbourOffsetArrayOdd.Count; j++)
							{
								if (allegiances.instance.Lists[i].ArmyTransforms[k] != null)
								{
									int NewX = allegiances.instance.Lists[i].ArmyTransforms[k].GetComponent<Unit>().GridX + map.neighbourOffsetArrayEven[j].x;
									int NewY = allegiances.instance.Lists[i].ArmyTransforms[k].GetComponent<Unit>().GridY + map.neighbourOffsetArrayEven[j].y;
									//Debug.Log(NewX + " X" + NewY + " Y");

									if (NewX > 0 && NewY > 0)
									{
										if (map.tiles[NewX, NewY] != 3 || map.tiles[NewX, NewY] != 0)
										{
											Transform TileOBJ = HexGenerator.instance.TileTwoArray[NewX, NewY].transform;

											if (PeopleManager.instance.IntArraySingle.Contains((NewX * map.RealHeight) + NewY))
											{

											}
											else
											{
												if (TileOBJ != null)
												{
													distences.Add(TileOBJ);
													Grids.Add(new int2(NewX, NewY));
													IsTile.Add(true);
													targets.Add(allegiances.instance.Lists[i].ArmyTransforms[k]);
												}
											}
										}
									}
								}
								//check mountains
							}
						}
					}

				}
			}
		}

		for (int i = 0; i < allegiances.instance.Lists.Count; i++)
		{
			if (allegiances.instance.Lists[Type].State[i] == 0 || allegiances.instance.Lists[Type].State[i] == -1)
			{
				Friendly.Add(i);
				//Debug.Log("Me: " + Type + "  Them:  " + i);
			}
		}
		//Debug.Log("gotT1");
		//done adding to list
		float minDist = Mathf.Infinity;
		Vector3 currentPos = Object.position;
		Vector3 final;
		Transform finalTransform = null;
		bool IsTileEnd = true;
		Transform EndTarget = null;
		if (distences.Count == 0)
		{
			return;
		}
		for (int i = 0; i < distences.Count; i++)
		{
			Vector3 SpaceVector3 = new Vector3(distences[i].position.x, distences[i].position.y, 0);
			float dist = Vector3.Distance(SpaceVector3, currentPos);
			if (dist < minDist)
			{
				final = new Vector3(distences[i].position.x, distences[i].position.y, 0);
				minDist = dist;
				map.finalVector3 = final;
				map.Current = Grids[i];
				finalTransform = distences[i];
				IsTileEnd = IsTile[i];
				EndTarget = targets[i];
			}
		}
		int TypeOfObject = 0;

		//number unimportant(could be anything thats not 0 - 6
		int EnemyType = 15;
		if (finalTransform.GetComponent<Unit>())
		{
			TypeOfObject = 1;
			EnemyType = finalTransform.GetComponent<Unit>().FactionNum - 1;
		}
		else if (finalTransform.GetComponent<ClickableTile>())
		{
			TypeOfObject = 2;
			EnemyType = finalTransform.GetComponent<ClickableTile>().Kingdom;
		}
		float SpareXFloat = map.finalVector3.x;
		map.SpareX = Mathf.RoundToInt(SpareXFloat);
		float SpareYFloat = map.finalVector3.y;
		map.SpareY = Mathf.RoundToInt(SpareYFloat);
		int2 start = new int2(Object.GetComponent<Unit>().GridX, Object.GetComponent<Unit>().GridY);

		map.PM.FindPath(start, map.Current, Object.gameObject, Type, EnemyType, null, false);
		Object.gameObject.GetComponent<Skeleton>().TargetFound(EndTarget, TypeOfObject);
		*/
	}
}
