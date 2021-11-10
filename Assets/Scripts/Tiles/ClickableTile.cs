using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.Mathematics;
using System.Collections.Generic;

public class ClickableTile : MonoBehaviour {

	/*
	//how many to a tile?
	//does it vary on the tile?
	//some tiles hold 10 weight and swamps hold 5 or something
	
	public int tileX;
	public int tileY;

	public float WorldX;
	public float WorldY;
	public float WorldZ;

	public HexGenerator map;
	public GameObject CurrentSelected;

	public bool Building;

	public int TileType;

	public GameObject MainLand;

	public enum Factions { FactionA, FactionB, FactionC, FactionD, FactionE, FactionF };

	public Factions factions;

	public int Kingdom;

	public List<int> Boarders = new List<int>();

	public bool Manned = false;
	public bool Reachable = true;

	public void ChangeFaction()
    {
		float minDist = Mathf.Infinity;
		for (int i = 0; i < KingdomAssigner.instance.Spots.Count; i++)
        {
			//Vector2.Distance(a, b)
			Vector2 Spare = new Vector2(KingdomAssigner.instance.Spots[i].x, KingdomAssigner.instance.Spots[i].y);
			float dist = Vector2.Distance(new Vector2(WorldX, WorldY), Spare);
			if (dist < minDist)
			{
				minDist = dist;
				Kingdom = i;
			}
		}
		allegiances.instance.Lists[Kingdom].Land += 1;
		int ValueInArray = (tileX * map.RealHeight) + tileY;
		if (gameObject.GetComponent<Castle>() == null)
        {
			//int ValueInArray = (tileX * map.RealHeight) + tileY;
		}

		map.KingdomSave[ValueInArray] = Kingdom;

		#region NotImportant

		if (Kingdom == 0)
		{
			factions = Factions.FactionA;
		}
		else if (Kingdom == 1)
		{
			factions = Factions.FactionB;
		}
		else if (Kingdom == 2)
		{
			factions = Factions.FactionC;
		}
		else if (Kingdom == 3)
		{
			factions = Factions.FactionD;
		}
		else if (Kingdom == 4)
		{
			factions = Factions.FactionE;
		}
		else if (Kingdom == 5)
		{
			factions = Factions.FactionF;
		}
		
		

		
		else if (KingdomAssigner.instance.GreyTiles == true)
        {
			if (Kingdom == 0)
			{
				SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
				Color newColor = new Color(1f, 1f, 1f, 1f);
				spriteRenderer.color = newColor;
			}
			else if (Kingdom == 1)
			{
				SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
				Color newColor = new Color(.9f, .9f, .9f, 1f);
				spriteRenderer.color = newColor;
			}
			else if (Kingdom == 2)
			{
				SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
				Color newColor = new Color(.8f, .8f, .8f, 1f);
				spriteRenderer.color = newColor;
			}
			else if (Kingdom == 3)
			{
				SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
				Color newColor = new Color(.7f, .7f, .7f, 1f);
				spriteRenderer.color = newColor;
			}
			else if (Kingdom == 4)
			{
				SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
				Color newColor = new Color(1f, 1f, 1f, 1f);
				spriteRenderer.color = Color.gray;
			}
			else if (Kingdom == 5)
			{
				SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
				Color newColor = new Color(1f, 1f, 1f, 1f);
				spriteRenderer.color = Color.magenta;
			}
			else if (Kingdom == 6)
			{
				SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
				Color newColor = new Color(1f, 1f, 1f, 1f);
				spriteRenderer.color = Color.cyan;
			}
		}
		
		#endregion
	}

	void Start()
    {
		if (Building == true)
        {
			gameObject.GetComponent<BuildProgress>().tile = this;
		}
		
		if (gameObject.GetComponent<Castle>() != null)
		{
			ChangeFaction();
		}
	}

	public void GeneratePath(GameObject CurrentSelected, int FactionNum)
    {
		//need start, end, and factions
		
		Unit UnitScript = CurrentSelected.GetComponent<Unit>();
		int2 start = new int2(UnitScript.GridX, UnitScript.GridY);
		int2 end = new int2(tileX, tileY);
		List<int> Friendly = new List<int>();
		for (int i = 0; i < allegiances.instance.Lists.Count; i++)
		{
			if (allegiances.instance.Lists[CurrentSelected.GetComponent<Unit>().FactionNum].State[i] == -1 || allegiances.instance.Lists[CurrentSelected.GetComponent<Unit>().FactionNum].State[i] == 0)
			{
				Friendly.Add(i);
			}
		}
		//20 because there is no place a player should not go (they just get consequences)
		map.PM.FindPath(start, end, CurrentSelected, FactionNum, 20, null, false);
	}

	public void DestoryAndRemoveLand()
    {
		allegiances.instance.Lists[Kingdom].LandBoarder.Remove(gameObject.transform);
		Destroy(gameObject);
	}
	public void Destroy(int ObjectSpawnNum)
    {
		if (map.tiles[tileX, tileY] == 1 || map.tiles[tileX, tileY] == 2)
        {
			if (ObjectSpawnNum == 0 || ObjectSpawnNum == 1)
			{
				if (Currency.instance.Money > 4)
				{
					Currency.instance.ChangeCurrecy(-5);
					//map.TM.ReplaceTile(tileX, tileY, 5, WorldX, WorldY, WorldZ, ObjectSpawnNum);
					//remove from tile
					//CheckBoarder
					DestoryAndRemoveLand();
				}
			}
			else if (ObjectSpawnNum == 2)
            {
				//SPAWNS PLAYER CASTLE
				//map.TM.SpawnPlayerCastle(tileX, tileY, WorldX, WorldY, WorldZ);
				DestoryAndRemoveLand();
			}
			else
			{
				if (Currency.instance.Money > 4)
				{
					Currency.instance.ChangeCurrecy(-5);
					//map.TM.ReplaceTile(tileX, tileY, 5, WorldX, WorldY, WorldZ, ObjectSpawnNum);
					DestoryAndRemoveLand();
				}
			}
		}
	}

	public void CompleteBuilding()
    {
		//map.TM.ReplaceTile(tileX, tileY, 6, WorldX, WorldY, WorldZ, 0);
	}

	public void CheckBoarder()
    {
		//Debug.Log("start1");
		if (tileY % 2 == 1)
		{
			//Debug.Log("start2");
			for (int i = 0; i < map.neighbourOffsetArrayEven.Count; i++)
			{
				//Debug.Log("start3");
				int OffsetNum = (map.neighbourOffsetArrayEven[i].x + tileX * map.RealHeight) + map.neighbourOffsetArrayEven[i].y + tileY;

				//different kingdom
				//Debug.Log("this: " + ThisNum + "  other: " + OffsetNum);
				int x = map.neighbourOffsetArrayEven[i].x + tileX;
				int y = map.neighbourOffsetArrayEven[i].y + tileY;
				if (x > -1 && y > -1 && x < 50 && y < 200)
				{
                    if (Boarders.Contains(map.KingdomSave[OffsetNum]))
                    {

                    }
					else if (Kingdom != map.KingdomSave[OffsetNum])
					{
						Boarders.Add(map.KingdomSave[OffsetNum]);
					}
				}
				//Debug.Log("start4");
			}
		}
		else
		{
			//Debug.Log("start2");
			for (int i = 0; i < map.neighbourOffsetArrayOdd.Count; i++)
			{
				//Debug.Log("start3");
				int OffsetNum = (map.neighbourOffsetArrayOdd[i].x + tileX * map.RealHeight) + map.neighbourOffsetArrayOdd[i].y + tileY;
				int x = map.neighbourOffsetArrayOdd[i].x + tileX;
				int y = map.neighbourOffsetArrayOdd[i].y + tileY;

				if (x > -1 && y > -1 && x < 50 && y < 200)
				{
					if (Boarders.Contains(map.KingdomSave[OffsetNum]))
					{

					}
					else if (Kingdom != map.KingdomSave[OffsetNum])
					{
						Boarders.Add(map.KingdomSave[OffsetNum]);
					}
				}
				//Debug.Log("start4");
			}
		}

		if(Boarders.Count > 0)
        {
            if (TileType == 0)
            {
				allegiances.instance.Lists[Kingdom].WaterBoarder.Add(gameObject.transform);
			}
            else
            {
				allegiances.instance.Lists[Kingdom].LandBoarder.Add(gameObject.transform);
			}
			

		}
	}*/

}
