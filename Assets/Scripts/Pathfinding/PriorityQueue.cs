using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private Queue<Node> pos;
    private Queue<float> wt;
    public int Count;

    public PriorityQueue()
    {
        pos = new Queue<Node>();
        wt = new Queue<float>();
        Count = 0;
    }

    public void enqueue(Vector3 position, float weight)
    {
        pos.Enqueue(new Node(true, position)); // set true for walkable for now... TODO: determine if walkable
        wt.Enqueue(weight);
        Count++;
    }

    public Node dequeue()
    {
        wt.Dequeue();
        Count--;
        return pos.Dequeue();
    }

    public Node peekPos()
    {
        return pos.Peek();
    }

    public bool isEmpty()
    {
        if (pos.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
