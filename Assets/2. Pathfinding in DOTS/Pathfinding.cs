using UnityEngine;

public class Pathfinding : MonoBehaviour {


	// So, the first thing we need for Pathfinding is to store data on a grid
	private struct PathNode {

		// X axis, y Axis and each node index
		public int x, y, index; 

		/* A* Pathfinding Costs 
			gCost - Move cost from start node onto this node.
			hCost - Estimated cost from this node onto end node.
			fCost - Simply G + H.
		 */
		public int gCost, hCost, fCost;

		public bool isWalkable; // To determine can walk or not on a particular node

		public int cameFromIndex; // For reverse pathfinding, previous index to hCost
    }

}