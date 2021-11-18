using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Random = UnityEngine.Random;


public class PathfindingManager : MonoBehaviour
{
	public HexGenerator map;
	public BattleField field;
	public List<int2> RealPath = new List<int2>();

	#region Non-BattlePaths
	//GET OBJECTS KINGDOM AND MAKE IT SO OWN KINGDOM IS ALWAYS ALLOWED!!
	public List<int2> FindPath(int2 start, int2 end, int myKingdom, int ObjectiveType, bool OnFinish)
	{
		NativeArray<int> BasicTileSave = new NativeArray<int>(map.RealWidth * map.RealHeight, Allocator.TempJob);
		for (int x = 0; x < map.TileSave.Count; x++)
		{
			BasicTileSave[x] = map.TileSave[x];
		}

		int Tiles = map.RealHeight * map.RealWidth;
		
		NativeArray<bool> NativeWalkable = new NativeArray<bool>(Tiles, Allocator.TempJob);
		for (int x = 0; x < map.RealWidth; x++)
		{
			for (int y = 0; y < map.RealHeight; y++)
			{
				int ArrayNum = (x * map.RealHeight) + y;
				int TileKingdom = map.GetTileFaction(x,y);
				NativeWalkable[ArrayNum] = CheckMapTile(x, y, myKingdom, TileKingdom, ObjectiveType);
			}
		}

		NativeArray<int2> Even = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		NativeArray<int2> Odd = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		for (int x = 0; x < map.neighbourOffsetArrayEven.Count; x++)
		{
			Even[x] = map.neighbourOffsetArrayEven[x];
			Odd[x] = map.neighbourOffsetArrayOdd[x];
		}

		NativeArray<int2> PathOut = new NativeArray<int2>(2500, Allocator.TempJob);
		FindPathJob findPathJob = new FindPathJob
		{
			startPosition = new int2(start.x, start.y),
			endPosition = new int2(end.x, end.y),
			GridSize = new int2(map.RealWidth, map.RealHeight),
			Path = PathOut,
			WalkableTiles = NativeWalkable,
			OddOffset = Odd,
			EvenOffset = Even,
		};
		JobHandle jobHandle = findPathJob.Schedule();
		jobHandle.Complete();

		List<int2> FoundPath = new List<int2>();
		for (int i = 0; i < findPathJob.Path.Length; i++)
		{
			if (findPathJob.Path[i].x != 0)
			{
				FoundPath.Add(findPathJob.Path[i]);
			}
		}

		FoundPath.Reverse();
		//PathOut.Dispose();
		//Debug.Log(RealPath.Count);
		BasicTileSave.Dispose();
		NativeWalkable.Dispose();
        if (FoundPath.Count == 0)
        {
			//Debug.LogError("NoPathFound");
		}
		return FoundPath;
		/*
			if (OnFinish == true)
			{
				UnitMove.GetComponent<Unit>().OnFinish = true;
				UnitMove.GetComponent<Unit>().WalkingTowardsFaction = ObjectiveType;
			}
			*/
	}

	public void FindBattlePath(int2 start, int2 end, GameObject UnitMove, int myKingdom, int ObjectiveType, Castle castle, bool OnFinish)
	{
		//Debug.Log(start + "   " + end);
		List<bool> CantWalkList = new List<bool>();
		NativeArray<bool> UnWalkableKingdoms = new NativeArray<bool>(CantWalkList.Count, Allocator.TempJob);
		for (int x = 0; x < CantWalkList.Count; x++)
		{
			UnWalkableKingdoms[x] = CantWalkList[x];
		}

		int Count = map.Width * map.Height + 2;

		NativeArray<int> BasicTileSave = new NativeArray<int>(Count, Allocator.TempJob);
		for (int x = 0; x < map.TileSave.Count; x++)
		{
			BasicTileSave[x] = map.TileSave[x];
		}

		NativeArray<int2> Even = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		NativeArray<int2> Odd = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		for (int x = 0; x < map.neighbourOffsetArrayEven.Count; x++)
		{
			Even[x] = map.neighbourOffsetArrayEven[x];
			Odd[x] = map.neighbourOffsetArrayOdd[x];
			//Debug.Log(map.KingdomSave[x]);
		}

		NativeArray<int2> PathOut = new NativeArray<int2>(2500, Allocator.TempJob);

		FindPathJob findPathJob = new FindPathJob
		{
			startPosition = new int2(start.x, start.y),
			endPosition = new int2(end.x, end.y),
			GridSize = new int2(map.RealWidth, map.RealHeight),
			Path = PathOut,
			//Kingdoms = NativeKingdom,
			//BasicTileSave = BasicTileSave,
			MyKingdom = myKingdom,
			enemyType = ObjectiveType,
			//UnwalkableKingdoms = UnWalkableKingdoms,
			OddOffset = Odd,
			EvenOffset = Even,
		};
		JobHandle jobHandle = findPathJob.Schedule();
		jobHandle.Complete();


		for (int i = 0; i < findPathJob.Path.Length; i++)
		{
			if (findPathJob.Path[i].x != 0)
			{
				RealPath.Add(findPathJob.Path[i]);
			}
		}
		RealPath.Reverse();
		//PathOut.Dispose();
		//Debug.Log(RealPath.Count);
		BasicTileSave.Dispose();
		if (RealPath.Count > 0 && UnitMove != null)
		{
			//Debug.Log("Found!");
			UnitMove.GetComponent<Unit>().AddToPath(RealPath);
			if (OnFinish == true)
			{
				UnitMove.GetComponent<Unit>().OnFinish = true;
				UnitMove.GetComponent<Unit>().WalkingTowardsFaction = ObjectiveType;
			}

		}
		RealPath.Clear();
	}

	public int AlteredPathExists(int2 start, int2 end, int MyFaction)
	{
		List<int> UnFriendlyListNew = new List<int>();

		for (int i = 0; i < allegiances.instance.Lists[MyFaction].State.Count; i++)
		{
			if (allegiances.instance.Lists[MyFaction].State[i] == 1)
			{
				UnFriendlyListNew.Add(i);
			}
		}

		for (int i = 0; i < 6; i++)
		{
			List<int> NewList = UnFriendlyListNew;
			NewList.Add(i);
			if (PathExists(start, end, true, MyFaction, MyFaction) == true)
			{
				return i;
			}
		}
		return 7;
	}

	public bool PathExists(int2 start, int2 end, bool UseKingdoms, int MyKingdom, int ObjectiveType)
	{
		NativeArray<int> BasicTileSave = new NativeArray<int>(map.RealWidth * map.RealHeight, Allocator.TempJob);
		for (int x = 0; x < map.TileSave.Count; x++)
		{
			BasicTileSave[x] = map.TileSave[x];
		}

		int Tiles = map.RealHeight * map.RealWidth;
		NativeArray<bool> NativeWalkable = new NativeArray<bool>(Tiles, Allocator.TempJob);
		for (int x = 0; x < map.RealWidth; x++)
		{
			for (int y = 0; y < map.RealHeight; y++)
			{
				int TileKingdom = map.GetTileFaction(x, y);
				int ArrayNum = (x * map.RealHeight) + y;
				NativeWalkable[ArrayNum] = CheckMapTile(x, y, MyKingdom, TileKingdom, ObjectiveType);
			}
		}

		NativeArray<int2> Even = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		NativeArray<int2> Odd = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		for (int x = 0; x < map.neighbourOffsetArrayEven.Count; x++)
		{
			Even[x] = map.neighbourOffsetArrayEven[x];
			Odd[x] = map.neighbourOffsetArrayOdd[x];
		}

		NativeArray<int2> PathOut = new NativeArray<int2>(2500, Allocator.TempJob);
		FindPathJob findPathJob = new FindPathJob
		{
			startPosition = new int2(start.x, start.y),
			endPosition = new int2(end.x, end.y),
			GridSize = new int2(map.RealWidth, map.RealHeight),
			Path = PathOut,
			WalkableTiles = NativeWalkable,
			OddOffset = Odd,
			EvenOffset = Even,
		};
		JobHandle jobHandle = findPathJob.Schedule();
		jobHandle.Complete();

		for (int i = 0; i < findPathJob.Path.Length; i++)
		{
			if (findPathJob.Path[i].x != 0)
			{
				RealPath.Add(findPathJob.Path[i]);
			}
		}
		RealPath.Reverse();
		//PathOut.Dispose();
		BasicTileSave.Dispose();
		if (RealPath.Count > 0)
		{
			RealPath.Clear();
			return true;
		}
		else
		{
			RealPath.Clear();
			return false;
		}
	}
	#endregion

	#region Battle

	#endregion
	[BurstCompile]
	private struct FindPathJob : IJob
	{
		[DeallocateOnJobCompletion] public int2 startPosition;
		[DeallocateOnJobCompletion] public int2 endPosition;

		[DeallocateOnJobCompletion] public int2 GridSize;

		public NativeArray<int2> Path;

		public NativeArray<int2> OddOffset;
		public NativeArray<int2> EvenOffset;

		//public NativeArray<int> BasicTileSave;

		//public NativeArray<int> Kingdoms;

		//public NativeArray<bool> UnwalkableKingdoms;

		public NativeArray<bool> WalkableTiles;

		public int MyKingdom;

		public int enemyType;

		public bool FoundPath;

		public void Execute()
		{
			int2 Grid = new int2(GridSize.x, GridSize.y);
			NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(Grid.x * Grid.y, Allocator.Temp);
			for (int x = 0; x < GridSize.x; x++)
			{
				for (int y = 0; y < GridSize.y; y++)
				{
					PathNode pathNode = new PathNode();

					pathNode.xGrid = x;
					pathNode.yGrid = y;
					pathNode.index = CalculateIndex(x, y, GridSize.y);

					pathNode.gCost = int.MaxValue;
					//pathNode.hCost = 1;
					pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
					pathNode.CalculateFCost();
					pathNode.cameFromNodeIndex = -1;

					//set initially

					//k is tile check's kingdom

					int ArrayNum = (x * GridSize.y) + y;

					pathNode.SetIsWalkable(WalkableTiles[ArrayNum]);
					//Debug.Log(WalkableTiles[ArrayNum]);
					if (y % 2 == 1)
					{
						pathNode.IsEven = true;
					}
					else
					{
						pathNode.IsEven = false;
					}
					pathNodeArray[pathNode.index] = pathNode;
				}
			}

			int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, GridSize.y);

			PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, GridSize.y)];

			startNode.gCost = 0;
			startNode.CalculateFCost();
			pathNodeArray[startNode.index] = startNode;

			NativeList<int> openList = new NativeList<int>(Allocator.Temp);
			NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

			openList.Add(startNode.index);
			//Debug.Log("passed");
			NativeArray<int2> neighbourOffsetArrayCurrent = new NativeArray<int2>(6, Allocator.Temp);
			int Count = 0;
			while (openList.Length > 0)
			{
				int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);

				PathNode currentNode = pathNodeArray[currentNodeIndex];
				if (currentNodeIndex == endNodeIndex)
				{
					// Reached our destination!
					//Debug.Log("found!");
					break;
				}

				// Remove current node from Open List
				for (int i = 0; i < openList.Length; i++)
				{
					if (openList[i] == currentNodeIndex)
					{
						openList.RemoveAtSwapBack(i);
						break;
					}
				}

				if (currentNode.IsEven == true)
				{
					neighbourOffsetArrayCurrent = EvenOffset;
				}
				else
				{
					neighbourOffsetArrayCurrent = OddOffset;
				}
				for (int i = 0; i < neighbourOffsetArrayCurrent.Length; i++)
				{
					int2 neighbourOffset = neighbourOffsetArrayCurrent[i];
					int2 neighbourPosition = new int2(currentNode.xGrid + neighbourOffset.x, currentNode.yGrid + neighbourOffset.y);

					if (!IsPositionInsideGrid(neighbourPosition, Grid))
					{
						continue;
					}
					int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, Grid.y);

					if (closedList.Contains(neighbourNodeIndex))
					{
						// Already searched this node
						continue;
					}
					PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
					//Debug.Log(currentNode.xGrid + currentNode.yGrid);

					if (!neighbourNode.Walkable)
					{
						continue;
					}
					else
					{
						//Debug.Log("node  " + neighbourNode.xGrid + " " + neighbourNode.yGrid);
					}
					//if passes restirctions, test
					int2 currentNodePosition = new int2(currentNode.xGrid, currentNode.yGrid);
					//Debug.Log("Make1");
					int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
					if (Count < 30)
					{
						Count += 1;
						//Debug.Log("tentativeGCost: " + tentativeGCost + "  distanceCost  " + CalculateDistanceCost(currentNodePosition, neighbourPosition) + "  currentNode  " + currentNodePosition + "  neighbourPosition  " + neighbourPosition);
					}
					if (tentativeGCost < neighbourNode.gCost)
					{
						neighbourNode.cameFromNodeIndex = currentNodeIndex;
						neighbourNode.gCost = tentativeGCost;
						neighbourNode.CalculateFCost();
						pathNodeArray[neighbourNodeIndex] = neighbourNode;
						//Debug.Log(pathNodeArray[neighbourNodeIndex].xGrid + " " + pathNodeArray[neighbourNodeIndex].yGrid);
						if (!openList.Contains(neighbourNode.index))
						{
							//Debug.Log(neighbourNode.xGrid + " " + neighbourNode.yGrid);
							//Debug.Log("oneFound");
							openList.Add(neighbourNode.index);
						}
					}

				}
				closedList.Add(currentNodeIndex);
				//neighbourOffsetArrayCurrent.Dispose();
			}

			PathNode endNode = pathNodeArray[endNodeIndex];
			//Debug.Log("found1");
			for (int i = 0; i < pathNodeArray.Length; i++)
			{
				//Debug.Log(pathNodeArray[i].xGrid + " " + pathNodeArray[i].yGrid);
			}

			if (endNode.cameFromNodeIndex == -1)
			{
				// Didn't find a path!
				//Debug.Log("Didn't find a path!");
				FoundPath = false;
			}
			else
			{
				//Debug.Log("path");
				//Debug.Log(pathNodeArray.Length);

				NativeArray<int2> path = CalculatePath(pathNodeArray, endNode);
				for (int i = 0; i < path.Length; i++)
				{
					//Debug.Log(path[i].x + " " + path[i].y);
				}
				FoundPath = true;
				//HexGenerator.instance.DebugList();
				//Path = path;
				//Path = new NativeArray<int2>(path.Length, Allocator.TempJob);

				for (int i = 0; i < path.Length; i++)
				{
					Path[i] = path[i];
				}
				path.Dispose();
			}
			//Debug.Log("found4");
			pathNodeArray.Dispose();
			//neighbourOffsetArrayCurrent.Dispose();
			openList.Dispose();
			closedList.Dispose();
		}

		private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
		{
			//.36
			int difference = math.abs(aPosition.y - bPosition.y);

			if (difference == 2)
			{
				int xDistance = math.abs(aPosition.x - bPosition.x);
				int yDistance = math.abs(aPosition.y - bPosition.y);
				int remaining = math.abs(xDistance - yDistance);
				return 5;
			}
			else
			{
				int xDistance = math.abs(aPosition.x - bPosition.x);
				int yDistance = math.abs(aPosition.y - bPosition.y);
				int remaining = math.abs(xDistance - yDistance);
				return 10;
			}

		}

		private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
		{
			//Debug.Log("List");
			//Debug.Log(endNode.xGrid + " " + endNode.yGrid);
			if (endNode.cameFromNodeIndex == -1)
			{
				// Couldn't find a path!
				Debug.Log("couldn't find");
				return new NativeList<int2>(Allocator.Temp);
			}
			else
			{
				// Found a path
				NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
				path.Add(new int2(endNode.xGrid, endNode.yGrid));

				PathNode currentNode = endNode;
				while (currentNode.cameFromNodeIndex != -1)
				{
					PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
					path.Add(new int2(cameFromNode.xGrid, cameFromNode.yGrid));
					currentNode = cameFromNode;
				}

				return path;
			}
		}

		private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
		{
			return
				gridPosition.x >= 0 &&
				gridPosition.y >= 0 &&
				gridPosition.x < gridSize.x &&
				gridPosition.y < gridSize.y;
		}

		private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
		{
			PathNode lowestCostPathNode = pathNodeArray[openList[0]];
			for (int i = 1; i < openList.Length; i++)
			{
				PathNode testPathNode = pathNodeArray[openList[i]];
				if (testPathNode.fCost < lowestCostPathNode.fCost)
				{
					lowestCostPathNode = testPathNode;
				}
			}
			return lowestCostPathNode.index;
		}

		private int CalculateIndex(int x, int y, int gridWidth)
		{
			return (x * gridWidth) + y;
		}

		private struct PathNode
		{
			public int gCost;
			public int hCost;
			public int fCost;

			public int xGrid;
			public int yGrid;

			public bool Walkable;

			public bool ISOdd;

			public int cameFromNodeIndex;

			public int index;

			public bool IsEven;

			public void CalculateFCost()
			{
				fCost = gCost + hCost;
			}

			public void SetIsWalkable(bool isWalkable)
			{
				this.Walkable = isWalkable;
			}
		}
	}

	public bool CheckMapTile(int x, int y, int MyFaction, int OtherFaction, int Objective)
	{
		bool KingdomWalk = false;
		int ArrayNum = (x * map.RealHeight) + y;
		int TileType = map.CheckTile(x, y);
		if (allegiances.instance.Lists[MyFaction].State[OtherFaction] == 1 || OtherFaction == MyFaction || OtherFaction == Objective)
		{
			KingdomWalk = true;
		}
		if (KingdomWalk == true && TileType != 3 && TileType != 0)
		{
			return true;
			//Debug.Log(x + " " + y + "  true");
		}
		else
		{
			return false;
			//Debug.Log(x + " " + y + "  false");
		}
	}

	public List<int2> BattlePath(int2 start, int2 end)
    {
		NativeArray<bool> NativeWalkable = new NativeArray<bool>(map.RealHeight * map.RealWidth, Allocator.TempJob);
		for (int x = 0; x < field.MapSize.x; x++)
		{
			for (int y = 0; y < field.MapSize.y; y++)
			{
				int ArrayNum = (x * map.RealHeight) + y;
				NativeWalkable[ArrayNum] = field.WalkAble(x,y);
			}
		}

		NativeArray<int2> Even = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		NativeArray<int2> Odd = new NativeArray<int2>(map.neighbourOffsetArrayEven.Count, Allocator.TempJob);
		for (int x = 0; x < map.neighbourOffsetArrayEven.Count; x++)
		{
			Even[x] = map.neighbourOffsetArrayEven[x];
			Odd[x] = map.neighbourOffsetArrayOdd[x];
		}

		NativeArray<int2> PathOut = new NativeArray<int2>(2500, Allocator.TempJob);
		FindPathJob findPathJob = new FindPathJob
		{
			startPosition = new int2(start.x, start.y),
			endPosition = new int2(end.x, end.y),
			GridSize = new int2(field.MapSize.x, field.MapSize.y),
			Path = PathOut,
			WalkableTiles = NativeWalkable,
			OddOffset = Odd,
			EvenOffset = Even,
		};
		JobHandle jobHandle = findPathJob.Schedule();
		jobHandle.Complete();

		List<int2> FoundPath = new List<int2>();
		for (int i = 0; i < findPathJob.Path.Length; i++)
		{
			if (findPathJob.Path[i].x != 0)
			{
				FoundPath.Add(findPathJob.Path[i]);
			}
		}

		FoundPath.Reverse();
		NativeWalkable.Dispose();
		if (FoundPath.Count == 0)
		{
			//Debug.LogError("NoPathFound");
		}
		return FoundPath;
	}
	//field
	//MapSize
	//only goes in friendly and same terriorty
	//or if willing to declair war or lose rating
}
