using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class DropScript : MonoBehaviour
{
    public HexGenerator Hex;
	public int DropPointsPerSide;

	private bool StartUpdate = false;

	public List<int2> DropPoints = new List<int2>();

	int Spawned;

	public int BoarderAdd;

    void Update()
    {
		if(StartUpdate == false)
        {
			FindDropPoints();
			int LowX = BoarderAdd;
			int HighX = HexGenerator.instance.RealWidth - BoarderAdd;
			int LowY = BoarderAdd;
			int HighY = HexGenerator.instance.RealHeight - BoarderAdd;
			DropPoint(LowX, HighX, LowY, HighY);
			StartUpdate = true;
			//ad castle
		}
    }

	void FindDropPoints()
	{
		//left

		for (int b = 0; b < DropPointsPerSide; b++)
		{
			
			//PeopleSpawner.instance.SpawnZone = 4;
		}


	}

	public void ClearDropList()
	{
		//int num = .Count;
		foreach (int2 num in DropPoints)
		{
			Hex.ChangeColor(num.x, num.y, Color.white);
		}
		DropPoints.Clear();
	}

	void DropPoint(int LowX, int HighX, int LowY, int HighY)
	{
		while (Spawned < DropPointsPerSide)
		{
			int x = Random.Range(LowX, HighX);
			int y = Random.Range(LowY, HighY);

			if (Hex.tiles[x, y] == 2 && Hex.GetTileFaction(x, y) == 0)
			{
				Hex.ChangeColor(x, y, Color.black);
				DropPoints.Add(new int2(x, y));
				Spawned += 1;
			}
		}
	}
}
