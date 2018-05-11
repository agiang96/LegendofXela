using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour {

    private GameObject flockPrefab;
    private int mapSize = 50;
    private float edgeOfMap = 5.0f;

    private static int flockSize = 25;
    private List<GameObject> flockArr = new List<GameObject>();

    private GameObject _playerObject;
    public Vector3 _destinationPosition;

    private GraphController _graphController;

    private bool _followPlayer;
    private bool _lazyFlight;
    private bool _lazyFlightDestinationReached;
    private bool finalDestinationReached;

    private Node[,] map;
    private PriorityQueue frontier = new PriorityQueue();
    int totalNodesRequired = 0;
    int numNodesTraveled = 0;
    int lookAhead = 1;
    Vector3 lastDestinationPosition;
    List<Vector3> nodeList = new List<Vector3>();
    int globalCounter = 0;

    // Use this for initialization
    void Start () {
        flockPrefab = Resources.Load<GameObject>("Boid");
        _playerObject = GameObject.Find("MainPlayer");

        float negative = Random.Range(0f, 1f);
        float xRange = Random.Range(_playerObject.transform.position.x + 30f, _playerObject.transform.position.x + 45f);

        if (negative < 0.5f)
        {
            xRange *= -1;
        }

        negative = Random.Range(0f, 1f);
        float zRange = Random.Range(_playerObject.transform.position.x + 30f, _playerObject.transform.position.x + 45f);
        
        if (negative < 0.5f)
        {
            zRange *= -1;
        }

        spawnBoids(xRange, zRange);
        /*
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _destinationPosition = new Vector3(mapSize / 2f, 10, mapSize / 2f);
        
        _graphController = GameObject.FindGameObjectWithTag("GraphController").GetComponent<GraphController>();
        */
        _followPlayer = false;
        _lazyFlight = true;
        _lazyFlightDestinationReached = false;
        finalDestinationReached = false;

        /*
        _destinationPosition = _graphController.GetGrid()[(int)Random.Range(edgeOfMap, mapSize - edgeOfMap),
                (int)Random.Range(edgeOfMap, mapSize - edgeOfMap)].position;
        _destinationPosition.y = (int)Random.Range(5f, 20f);
        */

        // DEMO
        _graphController = GameObject.FindGameObjectWithTag("GraphController").GetComponent<GraphController>();
        //_graphController.CreateGrid();
        //map = _graphController.GetGrid();
        //transform.position = new Vector3(0, 0, 0);
        _destinationPosition = new Vector3(xRange, 0, zRange);
        lastDestinationPosition = _destinationPosition;
        //calculatePath(new Vector3(8, 0, 12));
        //_destinationPosition = frontier.dequeue().position;

        for (int i = 0; i < 10; i++)
        {
            nodeList.Add(new Vector3(0, 0));
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_followPlayer)
        {
            _destinationPosition = _playerObject.transform.position;
        }
        else if (_lazyFlight && _lazyFlightDestinationReached)
        {
            if (!frontier.isEmpty())
            {
                lastDestinationPosition = _destinationPosition;
                //Debug.Log("Last Destination Position: " + lastDestinationPosition);
                _destinationPosition = frontier.dequeue().position;
                numNodesTraveled++;

                //Debug.Log("Curr NumNodes: " + numNodesTraveled);
                //Debug.Log("Total: " + totalNodesRequired);
                // If half the nodes have been traveled, recalculate
                if (totalNodesRequired > 10 && numNodesTraveled >= totalNodesRequired / 10)
                {
                    lastDestinationPosition = frontier.dequeue().position;
                    while (frontier.Count > 0)
                    {
                        //Debug.Log("Dequeue: " + frontier.dequeue().position);
                        frontier.dequeue();
                    }
                    //Debug.Log("CALCULATE NEW PATH");
                    //Debug.Log(totalNodesRequired);
                    //Debug.Log(lastDestinationPosition);
                    //calculatePath(frontier.dequeue().position); // final destination
                    calculatePath(_playerObject.transform.position);
                    _destinationPosition = frontier.dequeue().position;
                    totalNodesRequired = frontier.Count;
                    numNodesTraveled = 0;
                    numNodesTraveled++;
                }
                else if(totalNodesRequired <= 10)
                {
                    //Debug.Log("Follow player!");
                    _followPlayer = true;
                    _lazyFlight = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).transform.Find("Sphere").GetComponent<Renderer>().material.color = Color.blue;
                    }
                }
                //Debug.Log(_destinationPosition);
            }
            else
            {
                calculatePath(_playerObject.transform.position);
                //Debug.Log(GameObject.Find("Player").transform.position);
                //calculatePath(new Vector3((int)Random.Range(edgeOfMap, mapSize - edgeOfMap), 0, (int)Random.Range(edgeOfMap, mapSize - edgeOfMap)));
                //calculatePath(new Vector3((int)Random.Range(5, 30), 0, (int)Random.Range(5, 30)));
                totalNodesRequired = frontier.Count;
            }
            //_destinationPosition.y = (int)Random.Range(5f, 20f);
            _lazyFlightDestinationReached = false;
        }

        /*
        if (finalDestinationReached)
        {
            finalDestinationReached = false;
        }
        */

        if (GetFlockArray().Length == 0)
        {
            float negative = Random.Range(0f, 1f);
            float xRange = Random.Range(_playerObject.transform.position.x + 30f, _playerObject.transform.position.x + 40f);

            if (negative < 0.5f)
            {
                xRange *= -1;
            }

            negative = Random.Range(0f, 1f);
            float zRange = Random.Range(_playerObject.transform.position.x + 30f, _playerObject.transform.position.x + 40f);

            if (negative < 0.5f)
            {
                zRange *= -1;
            }

            spawnBoids(xRange, zRange);
        }
	}

    private void spawnBoids(float xRange, float zRange)
    {
        for (int i = 0; i < flockSize; i++)
        {
            /*
            Vector3 position = new Vector3(Random.Range(edgeOfMap, mapSize - edgeOfMap),
                                           Random.Range(5f, 10f),
                                           Random.Range(edgeOfMap, mapSize - edgeOfMap));
                                           */
            Vector3 position = new Vector3(xRange + (i / 10f), 10f, zRange + (i / 10f));
            flockArr.Add(Instantiate(flockPrefab, position, Quaternion.identity) as GameObject);
            flockArr[i].transform.SetParent(gameObject.transform);
        }
    }

    private void calculatePath(Vector3 destination)
    {
        //Debug.Log(transform.GetChild(0).transform.position);
        frontier.enqueue(lastDestinationPosition, 0);
        //Debug.Log(lastDestinationPosition);
        float currCost = 0;
        bool destinationPathFound = false;

        // Node hops to allow us to calculate when half the nodes have been traveled... [?]
        int nodeHops = (int)(Mathf.Abs(destination.x - frontier.peekPos().position.x) + Mathf.Abs(destination.z - frontier.peekPos().position.z));
        int counter = 0;
        int lastDirection = 0;

        Vector3 currPos = lastDestinationPosition;
        //frontier.dequeue();

        while (!destinationPathFound)
        {
            counter++;

            if (counter == 500)
            {
                //Debug.Log("HIT 500");
                //Debug.Log(currPos);
                //Debug.Log(destination);
                break;
            }
            
            int direction = 0; // 0 = left, 1 = up, 2 = right, 3 = down

            float[] neighborArr = calculateNeighbors(currPos, destination, lookAhead);
            direction = (int) neighborArr[1];
            lastDirection = direction;
            currCost += neighborArr[0];
            //Debug.Log(direction);

            if (direction == 0)
            {
                frontier.enqueue(new Vector3(currPos.x - lookAhead, neighborArr[2], currPos.z), currCost);
                currPos = new Vector3(currPos.x - lookAhead, currPos.y, currPos.z);
            }
            else if (direction == 1)
            {
                frontier.enqueue(new Vector3(currPos.x, neighborArr[2], currPos.z + lookAhead), currCost);
                currPos = new Vector3(currPos.x, currPos.y, currPos.z + lookAhead);
            }
            else if (direction == 2)
            {
                frontier.enqueue(new Vector3(currPos.x + lookAhead, neighborArr[2], currPos.z), currCost);
                currPos = new Vector3(currPos.x + lookAhead, currPos.y, currPos.z);
            }
            else if (direction == 3)
            {
                frontier.enqueue(new Vector3(currPos.x, neighborArr[2], currPos.z - lookAhead), currCost);
                currPos = new Vector3(currPos.x, currPos.y, currPos.z - lookAhead);
            }
            else if (direction == 4)
            {
                frontier.enqueue(new Vector3(currPos.x - lookAhead, neighborArr[2], currPos.z + lookAhead), currCost);
                currPos = new Vector3(currPos.x - lookAhead, currPos.y, currPos.z + lookAhead);
            }
            else if (direction == 5)
            {
                frontier.enqueue(new Vector3(currPos.x + lookAhead, neighborArr[2], currPos.z + lookAhead), currCost);
                currPos = new Vector3(currPos.x + lookAhead, currPos.y, currPos.z + lookAhead);
            }
            else if (direction == 6)
            {
                frontier.enqueue(new Vector3(currPos.x - lookAhead, neighborArr[2], currPos.z - lookAhead), currCost);
                currPos = new Vector3(currPos.x - lookAhead, currPos.y, currPos.z - lookAhead);
            }
            else if (direction == 7)
            {
                frontier.enqueue(new Vector3(currPos.x + lookAhead, neighborArr[2], currPos.z - lookAhead), currCost);
                currPos = new Vector3(currPos.x + lookAhead, currPos.y, currPos.z - lookAhead);
            }

            nodeList[counter % nodeList.Count] = currPos;
            //Debug.Log(currPos);

            //Debug.Log("Distance: " + Vector3.Distance(currPos, destination));
            //Debug.Log(currPos);
            //Debug.Log(destination);

            //Debug.Log(Vector3.Distance(currPos, destination));
            if (Vector3.Distance(currPos, destination) <= 3)
            {
                //Debug.Log("BREAK HERE!");
                frontier.enqueue(destination, 0);
                currPos = destination;
                destinationPathFound = true;
                //finalDestinationReached = true;
            }
        }

        /*
        while (!frontier.isEmpty())
        {
            Instantiate(flockPrefab, frontier.dequeue().position, Quaternion.identity);
        }
        */
    }

    private float[] calculateNeighbors(Vector3 currPos, Vector3 destination, int lookAhead)
    {
        globalCounter++;
        //Debug.Log("Calculating neighbors: " + globalCounter);
        float lowestWeight = 9999f;
        int dir = 0; // 0 = left, 1 = "up", 2 = right, 3 = "down"
        // 4 = up left, 5 = up right, 6 = down left, 7 = down right

        int x = (int) currPos.x;
        int z = (int) currPos.z;

        float y = 0;

        int[] oppositeDir = new int[8] { 2, 3, 0, 1, 7, 6, 5, 4 };

        // Get the nearest, closest walkable node
        if (_graphController.GetNode(new Vector3(x - lookAhead, z)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x - lookAhead, z)).position, destination);
            if (dist < lowestWeight)
            {
                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x - lookAhead, z)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 0;
                    y = _graphController.GetNode(new Vector3(x - lookAhead, z)).position.y;
                }
            }
        }

        if (_graphController.GetNode(new Vector3(x, z + lookAhead)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x, z + lookAhead)).position, destination);
            //Debug.Log((x) + "," + (z + lookAhead));
            //Debug.Log(_graphController.GetNode(new Vector3(x, z + lookAhead)).position);
            if (dist < lowestWeight)
            {
                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x, z + lookAhead)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 1;
                    y = _graphController.GetNode(new Vector3(x, z + lookAhead)).position.y;
                }
            }
        }
        if (_graphController.GetNode(new Vector3(x + lookAhead, z)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x + lookAhead, z)).position, destination);
            if (dist < lowestWeight)
            {
                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x + lookAhead, z)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 2;
                    y = _graphController.GetNode(new Vector3(x + lookAhead, z)).position.y;
                }
            }
        }
        if (_graphController.GetNode(new Vector3(x, z - lookAhead)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x, z - lookAhead)).position, destination);
            if (dist < lowestWeight)
            {

                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x, z - lookAhead)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 3;
                    y = _graphController.GetNode(new Vector3(x, z - lookAhead)).position.y;
                }
            }
        }

        // up left
        if (_graphController.GetNode(new Vector3(x - lookAhead, z + lookAhead)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x - lookAhead, z + lookAhead)).position, destination);
            if (dist < lowestWeight)
            {
                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x - lookAhead, z + lookAhead)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 4;
                    y = _graphController.GetNode(new Vector3(x - lookAhead, z + lookAhead)).position.y;
                }
            }
        }

        // up right
        if (_graphController.GetNode(new Vector3(x + lookAhead, z + lookAhead)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x + lookAhead, z + lookAhead)).position, destination);

            if (dist < lowestWeight)
            {
                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x + lookAhead, z + lookAhead)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 5;
                    y = _graphController.GetNode(new Vector3(x + lookAhead, z + lookAhead)).position.y;
                }
            }
        }

        // down left
        if (_graphController.GetNode(new Vector3(x - lookAhead, z - lookAhead)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x - lookAhead, z - lookAhead)).position, destination);
            if (dist < lowestWeight)
            {
                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x - lookAhead, z - lookAhead)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 6;
                    y = _graphController.GetNode(new Vector3(x - lookAhead, z - lookAhead)).position.y;
                }
            }
        }

        // down right
        if (_graphController.GetNode(new Vector3(x + lookAhead, z - lookAhead)).walkable)
        {
            float dist = Vector3.Distance(_graphController.GetNode(new Vector3(x + lookAhead, z - lookAhead)).position, destination);
            if (dist < lowestWeight)
            {
                bool found = false;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] == _graphController.GetNode(new Vector3(x + lookAhead, z - lookAhead)).position)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    lowestWeight = dist;
                    dir = 7;
                    y = _graphController.GetNode(new Vector3(x + lookAhead, z - lookAhead)).position.y;
                }
            }
        }

        return new float[] { lowestWeight, dir , y};
    }


    public Vector3 GetDestinationPosition()
    {
        return _destinationPosition;
    }

    public GameObject[] GetFlockArray()
    {
        for (int i = 0; i < flockArr.Count; i++)
        {
            if (flockArr[i] == null)
            {
                flockArr.RemoveAt(i);
            }
        }
        return flockArr.ToArray();
    }

    public void SetLazyFlightDestinationReached(bool set)
    {
        _lazyFlightDestinationReached = set;
    }

    public void SetFollowPlayer(bool set)
    {
        _followPlayer = set;
    }

    public void SetLazyFlight(bool set)
    {
        _lazyFlight = set;
        if (!set)
        {
            _lazyFlightDestinationReached = false;
        }
    }
}
