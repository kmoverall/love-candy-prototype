using UnityEngine;
using System.Collections;

//A node of a priority queue used for A* pathfinding
public class AStarEntry {
    public int priority;
    public Vector2 position;
	
    public AStarEntry() {
        priority = 0;
        position = Vector2.zero;
    }

    public AStarEntry(int pr, Vector2 po) {
        priority = pr;
        position = po;
    }
}
