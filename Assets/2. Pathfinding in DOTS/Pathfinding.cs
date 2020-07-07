using UnityEngine;
using Unity.Mathematics; // for int2
using Unity.Collections; // to create our grid into NativeArray

public class Pathfinding : MonoBehaviour {

	/* ------------------2. Create a testing function -----------------------------
	THE GOAL: Find the most cost-effective path from A to B.
	FindPath(a, b) { 
		startPos - The starting position of our subject (A).
		endPos - The final destination (B)
		gridSize - Grid size in X * Y directions.
		grid - Temporary NativeArray of PathNodes stored as our grid
		double for loop - Initialize the pathNodes.
	}
	*/

	private void FindPath(int2 startPos, int2 endPos) {
		int2 gridSize = new int2(4, 4);
		NativeArray<PathNode> grid = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

		for (int x = 0; x < gridSize.x; x++) {
			for (int y = 0; y < gridSize.y; y++) {
				PathNode node = new PathNode();
				node.x = x;
				node.y = y;
				// index
				// gCost,fCost,hCost
				// isWalkable
				// cameFromIndex

			}
		}

		grid.Dispose();

	}


	/* ------------------1. Define A* Pathfinding data -----------------------------
	gCost - Move cost from start node onto this node.
	hCost - Estimated cost from this node onto end node.
	fCost - Simply G + H.
	*/

	private struct PathNode {

		public int x, y, index;
		public int gCost, hCost, fCost;
		public bool isWalkable;
		public int cameFromIndex;

	}

}

