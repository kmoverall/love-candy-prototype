using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Simple Priority Queue implementation for A* Pathfinding
public class AStarQueue {
    private List<AStarEntry> queue;
    public int Count {get {return queue.Count;}}

    public AStarQueue() {
        queue = new List<AStarEntry>();
    }

    public void Add(int pr, Vector2 po) {
        AStarEntry entry = new AStarEntry(pr, po);
        queue.Add(entry);
    }

    public Vector2 Pop() {
        int min = int.MaxValue;
        int minindex = 0;
        Vector2 value = Vector2.zero;
        for (int i = 0; i < queue.Count; i++) {
            if (queue[i].priority < min) {
                min = queue[i].priority;
                minindex = i;
            }
        }

        value = queue[minindex].position;

        queue.RemoveAt(minindex);

        return value;
    }
}
