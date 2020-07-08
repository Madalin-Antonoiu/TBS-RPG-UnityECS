using UnityEngine;
using Unity.Mathematics; // for int2
using Unity.Collections; // to create our grid into NativeArray
using System.Security.Cryptography;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;

public class Pathfinding : MonoBehaviour {

	private const int MOVE_STRAIGHT_COST = 10;
	private const int MOVE_DIAGONAL_COST = 14;

    private void Start() {
		FindPath(new int2(0, 0), new int2(3, 1));
    }

    /* ------------------2. Create a testing function -----------------------------
		THE GOAL: Find the most cost-effective path from A to B.
		FindPath(a, b) { 
			startPos - The starting position of our subject (A).
			endPos - The final destination (B)
			gridSize - Grid size in X * Y directions.
			grid - Temporary NativeArray of PathNodes stored as our grid

			double for loop - Initialize the nodes(cells).
				node.x, node.y - Assign values from the loop iterations as coordinates.
				node.index - CalculateIndex() function
				node.gCost = int.MaxValue; (2147483647 - by default, have it take the maximum possible value for int type)
				node.hCost = CalculateHCost() function
				...
		}
		*/
    private void FindPath(int2 startPos, int2 endPos) {

        // a. ----- Setting up the Grid ----
		
        int2 gridSize = new int2(4, 4);
		NativeArray<PathNode> grid = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

		for (int m = 0; m < gridSize.x; m++) {
			for (int n = 0; n < gridSize.y; n++) {
				PathNode node = new PathNode();
				node.x = m;
				node.y = n;
				node.index = CalculateIndex(m, n, gridSize.x);
				node.gCost = int.MaxValue;
				node.hCost = CalculateDistanceCost(new int2(m, n), endPos);
				node.CalculateFCost();
				node.isWalkable = true;
				node.cameFromIndex = -1;

				grid[node.index] = node;
			}
		}

		// Testing time: Add some walls (obstacles) at (1,0), (1,1) and (1,2)!
        {
			PathNode walkablePathNode = grid[CalculateIndex(1, 0, gridSize.x)];
			walkablePathNode.SetIsWalkable(false);
			grid[CalculateIndex(1, 0, gridSize.x)] = walkablePathNode;

			walkablePathNode = grid[CalculateIndex(1, 1, gridSize.x)];
			walkablePathNode.SetIsWalkable(false);
			grid[CalculateIndex(1, 1, gridSize.x)] = walkablePathNode;

			walkablePathNode = grid[CalculateIndex(1, 2, gridSize.x)];
			walkablePathNode.SetIsWalkable(false);
			grid[CalculateIndex(1, 2, gridSize.x)] = walkablePathNode;


		}

		// a. Neighbours and the end node
		NativeArray<int2> neighbours = new NativeArray<int2>(new int2[] {
			new int2( -1, 0), // Left			⬅️
			new int2( +1, 0), // Right			➡️
			new int2( 0, -1), // Down			⬇️
			new int2( 0, +1), // Up				⬆️
			new int2( -1, -1), // Left Down		↙️
			new int2( -1, +1), // Left Up		↖️
			new int2( +1, -1), // Right Down	↘️
			new int2( +1, +1), // Right Up		↗️
		}, Allocator.Temp);

		int endNodeIndex = CalculateIndex(endPos.x, endPos.y, gridSize.x);

		// b. Define the startNode
		PathNode startNode = grid[CalculateIndex(startPos.x, startPos.y, gridSize.x)];
		startNode.gCost = 0;
		startNode.CalculateFCost();
		grid[startNode.index] = startNode;

		// c. openList and closedList <--- next
		NativeList<int> openList = new NativeList<int>(Allocator.Temp); // List of indexes
		NativeList<int> closedList = new NativeList<int>(Allocator.Temp); // List of indexes
		openList.Add(startNode.index); // Open list to include the startNode


		/* This is the motor that keeps doing the work 
		 * While openList still have cells to search
		 */
		while(openList.Length > 0) {

		// Current Node definition
            int currentNodeIndex = GetLowestCostFNodeIndex(openList, grid);// calculate it
			PathNode currentNode = grid[currentNodeIndex]; // current node on the grid based on the calculation above

			if(currentNodeIndex == endNodeIndex) {
				// Reached our destination !
				break;
            }

			//Remove currentNode from Open List
			for ( int i=0; i< openList.Length; i++) {
				if(openList[i] == currentNodeIndex) {
					openList.RemoveAtSwapBack(i); 
					break;
                }
            }

			closedList.Add(currentNodeIndex);

		// Neighbours!
			for ( int i=0; i< neighbours.Length; i++) {
				int2 neighbour = neighbours[i]; // get hold of each individual neighbour cell
				int2 neighbourPosition = new int2(currentNode.x + neighbour.x, currentNode.y + neighbour.y); // look back at neighbours list to understand

                if (!IsPositionInsideGrid(neighbourPosition, gridSize)) {
					// Neighbour not valid position, skip
					continue;
                }

				int neighbourIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);
                if (closedList.Contains(neighbourIndex)) {
					// Already searched this node, skip
					continue;
                }

				PathNode neighbourNode = grid[neighbourIndex];
                if (!neighbourNode.isWalkable) {
					// Not walkable
					continue;
                }

				int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

				//calculate algorithm
				int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);// current node cost + distance cost from current node pos to neighbour
				if(tentativeGCost < neighbourNode.gCost) { // it always kinda is lower because default gCost is 278248248274824 ( really big)
					neighbourNode.cameFromIndex = currentNodeIndex;
					neighbourNode.gCost = tentativeGCost;
					neighbourNode.CalculateFCost();
					grid[neighbourIndex] = neighbourNode;

                    if (!openList.Contains(neighbourIndex)) {
						openList.Add(neighbourNode.index);
                    }

					// End of Pathfinding! The outcome:  We either found the path or not.
				}
			}
		}

		// We`ve exhausted our openList, seached all, now what?
		//cameFromNodeIndex will tell us !

		PathNode endNode = grid[endNodeIndex];
		if(endNode.cameFromIndex == -1) {
			Debug.Log("Didn't find a path!");
        } else {
			// Found a path, use CalculateBackwardsPath()
			NativeList<int2> backwardsPath = CalculateBackwardsPath(grid, endNode);

			Debug.Log("Backwards Path ( from " + startPos + " to " + endPos);
			foreach (int2 pathPosition in backwardsPath) {
				
				Debug.Log(pathPosition);
            }

			// Right now wedont care about getting the path back, we just want to calculate it, so dispose of it straight away;
			backwardsPath.Dispose();
		}


		grid.Dispose();
		neighbours.Dispose();
		openList.Dispose();
		closedList.Dispose();

	}

	/* ------ Helper 1. CalculateIndex() -------
		* It takes X + Y positions and multiplies by Grid Width(x)
		* Example: gridSize(4,4) - so you have 16 grid cells. gridSize.x = 4 in the Width;
		*    xy	..y0	..y1	..y2	..y3		
		* x0	(0,0)	(0,1)	(0,2)	(0,3)	   - On row x0, 4 columns y0,y1,y2,y3. Much like 01,02,03,04...
		* x1	(1,0)	(1,1)	(1,2)	(1,3)	   - 10, 11, 12, 13...
		* x2	(2,0)	(2,1)	(2,2)	(2,3)	
		* x3	(3,0)	(3,1)	(3,2)	(3,3)
		* 
		* CalculateIndex(0,1,4) = 0 + (1*4) = 4
		* CalculateIndex(0,2,4) = 0 + (2*4) = 8
		* CalculateIndex(1,0,4) = 1 + (0*4) = 1
		* CalculateIndex(1,1,4) = 1 + (1*4) = 5
		* CalculateIndex(1,2,4) = 1 + (2*4) = 9
		* CalculateIndex(2,0,4) = 2 + (0*4) = 2
		* CalculateIndex(2,1,4) = 2 + (1*4) = 6 etc.
		* 
		* The index of each grid cell will be:
		* 0  4   8  12
		* 1  5   9  13
		* 2  6  10  14
		* 3  7  11  15
	*/
	private int CalculateIndex(int x, int y, int gridWidth) {
		return x + y * gridWidth;
	}

	/* ------ Helper 2. CalculateHCost() -------
	 * math.abs(x) - This function returns x if x > 0, 0 if x =0 or -x if x <0;
	 *  Examples:
		 *	a =3, b = 5; math.abs(a - b) = 2 (not -2);
		 *	a = 5, b =3; math.abs(a-b) = 2;
		 *	a = 1.23456, b = 7.89012; math.abs(a-b) = 6.6555599999999995;
	* math.min(a , b ) - return the smaller of the two numbers
	* int2(int x, int y) - Constructs a int2 vector from two int values.	
	* int2 a - basically means int2 (a.x, a.y) - This node with x,y coords
	* xDist, Y Dist, remaining and return - check drawn explanation if needed
	*/
	private int CalculateDistanceCost(int2 A, int2 B) {
		int xDist = math.abs(A.x - B.x);
		int yDist = math.abs(A.y - B.y);
		int remaining = math.abs(xDist - yDist);

		return MOVE_DIAGONAL_COST * math.min(xDist, yDist) + MOVE_STRAIGHT_COST * remaining; // explain all of it
    }

	/* ------ Helper 3. GetLowestCostFNodeIndex ------- 
	 * Add explanations
	 */
	private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> grid ) {
		PathNode lowestCostPathNode = grid[openList[0]]; // start from the beginning of openList

		// Loop through the openList
		for(int i =0; i< openList.Length; i++) {
			PathNode testPathNode = grid[openList[i]];
			if(testPathNode.fCost < lowestCostPathNode.fCost) {
				lowestCostPathNode = testPathNode;
            }
        }

		return lowestCostPathNode.index;
    }

	/* ------ Helper 4. IsPositionInsideGrid() ------- 
	* Here we are checking if x and y coordinates are bigger than 0, 
	* but lower than grid max coordinates, thus giving us valid cells.
	*/
	private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize) {
		return
			gridPosition.x >= 0 &&
			gridPosition.y >= 0 &&
			gridPosition.x < gridSize.x &&
			gridPosition.y < gridSize.y;
	}

	/* ------ Helper 5. CalculatePath() -------
	 * CalculatePath is a list of coordinates, it takes in the grid and the endNode.
	 * Add detailed explanation.
	 */
	private NativeList<int2> CalculateBackwardsPath(NativeArray<PathNode> grid, PathNode endNode) {
		if(endNode.cameFromIndex == -1) {
			// Couldn't find a path, return an empty native list, so our int2 is going to be our path position;
			return new NativeList<int2>(Allocator.Temp); 
        } else {
			// Found a path, need to walk backwards to get our actual path
			NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
			path.Add(new int2(endNode.x, endNode.y)); // The path receives the end node location to begin with.

			PathNode currentNode = endNode; // we start from endNode to go backwards
			while(currentNode.cameFromIndex != -1) { // while we have a cameFromindex in our currentNode
				PathNode cameFromNode = grid[currentNode.cameFromIndex]; // we grab the node in taht index
				path.Add(new int2(cameFromNode.x, cameFromNode.y)); // get position
				currentNode = cameFromNode; // and we add that position into our path
			}

			return path;

		}


		

	}

	/* ------------------1. Define A* Pathfinding data -----------------------------
		gCost - Move cost from start node onto this node.
		hCost - Estimated cost from this node onto end node.
		fCost - Simply G + H.
		x, y - Positions
		cameFromIndex - The index previous to hCost
	*/
	private struct PathNode {

		public int x, y, index;
		public int gCost, hCost, fCost;
		public bool isWalkable;
		public int cameFromIndex;

		public void CalculateFCost() {
			fCost = gCost + hCost;
        }

		public void SetIsWalkable(bool isWalkable) {
			this.isWalkable = isWalkable;
		}

	}

}

