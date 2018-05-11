using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    private Node[,] grid;
    private Dictionary<Vector3, Node> gridDict;
    //PGC_Maker PGCMaker;
    private int mapSize = 10; // TODO: Get PGC size from Shawn
    private GameObject dummyObj;

    // Use this for initialization
    void Start () {
        //PGCMaker = GameObject.FindGameObjectWithTag("PGC").GetComponent<PGC_Maker>();
        grid = new Node[mapSize, mapSize];
        grid[0, 0] = new Node(true, new Vector3(0, 10f, 0));
        gridDict = new Dictionary<Vector3, Node>();

        CreateGrid();
	}
	
    public void CreateGrid()
    {
        dummyObj = Resources.Load<GameObject>("Cube1");
        grid = new Node[mapSize, mapSize];
        // 0, 0, 0 is the starting corner
        // 100, 0, 100 is the other corner

        gridDict = new Dictionary<Vector3, Node>();
        /*
        for (int i = -100; i < mapSize; i++)
        {
            for(int j = -100; j < mapSize; j++)
            {
                //grid[i, j] = new Node(true, new Vector3(i, 10f, j));

                // key = vector3 position, value = walkable
                gridDict.Add(new Vector3(i, j), new Node(true, new Vector3(i, 0f, j)));
            }
        }

        for (int i = -25; i < -15; i++)
        {
            for (int j = -25; j < -15; j++)
            {
                //grid[i, j].walkable = false;
                gridDict[new Vector3(i, j)].walkable = false;
                //Debug.Log(gridDict[new Vector3(i, j)].walkable);
                //Debug.Log(gridDict[new Vector3(i, j)].position);
            }
        }
        Instantiate(dummyObj, new Vector3(-5, 0, -20), Quaternion.identity);

        for (int i = 45; i < 55; i++)
        {
            for (int j = 25; j < 35; j++)
            {
                //grid[i, j].walkable = false;
                gridDict[new Vector3(i, j)].walkable = false;
            }
        }
        Instantiate(dummyObj, new Vector3(50, 0, -30), Quaternion.identity);

        for (int i = 45; i < 55; i++)
        {
            for (int j = 55; j < 65; j++)
            {
                //grid[i, j].walkable = false;
                gridDict[new Vector3(i, j)].walkable = false;
            }
        }
        Instantiate(dummyObj, new Vector3(50, 0, 60), Quaternion.identity);
        */
    }

    /*
    public Node[,] GetGrid()
    {
        return grid;
    }
    */

    public Node GetNode(Vector3 pos)
    {
        //return new Node(true, new Vector3(pos.x, pos.z));
        
        // ##### !!! NOTE !!!
        // #####
        // ##### The parameter 'pos' is a Vector3. The "Z" coordinate is the "Y" coordinate in pos.
        // ##### So if you wanted to get the node for (5, #, 7), 'pos' would store it as (5, 7, 0).
        // ##### So that is why 'pos.y' is being used in the numbers below and NOT Z.

        Hashtable tiles = GameObject.Find("Landscape").GetComponent<GenerateInfinite>().getTileHashtable();
        //Debug.Log("Looking for: " + "Tile_" + (pos.x) + "_" + (pos.y));
        //Debug.Log("Looking for: " + "Tile_" + Mathf.RoundToInt(pos.x) + "_" + Mathf.RoundToInt(pos.y));
        if (tiles.ContainsKey("Tile_" + Mathf.RoundToInt(pos.x) + "_" + Mathf.RoundToInt(pos.y)))
        {
            Tile tile = (Tile)tiles["Tile_" + (pos.x) + "_" + (pos.y)];
            //Debug.Log(tile.location);
            return new Node(true, tile.location);
        }
        
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                /*
                Debug.Log("Tile_" + (pos.x - i) + "_" + (pos.y - j));
                GameObject tile = GameObject.Find("Tile_" + (pos.x - i) + "_" + (pos.y - j));
                if (tile != null)
                {
                    //Debug.Log("FOUND");
                    return new Node(true, tile.transform.position);
                }
                */
                /*
                if (tiles.ContainsKey("Tile_" + (pos.x + i) + "_" + (pos.z + i)))
                {
                    Tile tile = (Tile)tiles["Tile_" + (pos.x + i) + "_" + (pos.z + i)];
                    Debug.Log(tile.location);
                    return new Node(true, tile.location);
                }
                */
            }
        }

        Debug.Log("ERROR ERROR ERROR");
        throw new System.Exception();
        //return null;
    }
}
