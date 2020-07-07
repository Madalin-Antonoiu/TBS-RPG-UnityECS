using UnityEngine;
using Unity.Mathematics; // for int2
using Unity.Collections; // to create our grid into NativeArray
using System.Security.Cryptography;

public class Pathfinding : MonoBehaviour {

	private const int MOVE_STRAIGHT_COST = 10;
	private const int MOVE_DIAGONAL_COST = 14;

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
				node.hCost = CalculateHCost(new int2(m, n), endPos);
				node.CalculateFCost();
				node.bIsWalkable = true;
				node.cameFromIndex = -1;

				grid[node.index] = node;
			}
		}

		// b. Grab the start node





		grid.Dispose();

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
	private int CalculateHCost(int2 A, int2 B) {
		int xDist = math.abs(A.x - B.x);
		int yDist = math.abs(A.y - B.y);
		int remaining = math.abs(xDist - yDist);

		return MOVE_DIAGONAL_COST * math.min(xDist, yDist) + MOVE_STRAIGHT_COST * remaining; // explain all of it
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
		public bool bIsWalkable;
		public int cameFromIndex;

		public void CalculateFCost() {
			fCost = gCost + hCost;
        }
	}

}

