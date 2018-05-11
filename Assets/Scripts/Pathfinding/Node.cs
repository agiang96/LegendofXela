using UnityEngine;
using System.Collections;

public class Node
{
    public bool walkable;
    public Vector3 position;

    public Node(bool walkable, Vector3 position)
    {
        this.walkable = walkable; // not really using this since there are no obstacles in our map right now
        this.position = position;
    }
}
