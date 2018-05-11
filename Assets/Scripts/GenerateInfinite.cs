using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tile
{
    public GameObject theTile;
    public float creationTime;
    public Vector3 location;

    public Tile(GameObject t, float ct, Vector3 l)
    {
        theTile = t;
        creationTime = ct;
        location = l;
    }
}

//the camera of the FPS, the skybox, go to solid color, se;ect the 
//eyedropper and make it the same color as the fog, makes it not see the snapping change

//Source: https://www.youtube.com/watch?v=dycHQFEz8VI&t=422s

public class GenerateInfinite : MonoBehaviour {

    public GameObject plane;
    public GameObject player;

    int planeSize = 10;
    int halfTileX = 10;//radius around the player in any direction
    int halfTileZ = 10;


    Vector3 startPos; //use to keep track where the player is and walks

    public Hashtable tiles = new Hashtable(); //allows us to hold on to the gameobjects
    Hashtable pathfindingTiles = new Hashtable();
    public Vector3[] meshVertices;

    

	// Use this for initialization
	void Start () {
        this.gameObject.transform.position = Vector3.zero;
        startPos = Vector3.zero;

        float updateTime = Time.realtimeSinceStartup;

        //CReates a 2D ground plane made of all the connecting planes
        for(int x = -halfTileX; x < halfTileZ; x++)
        {
            for(int z = -halfTileZ; z < halfTileZ; z++)
            {
                Vector3 pos = new Vector3((x * planeSize + startPos.x), 0, (z * planeSize + startPos.z));
                GameObject t = (GameObject)Instantiate(plane, pos, Quaternion.identity); //creating the plane 
                Mesh mesh = t.GetComponent<MeshFilter>().mesh;
                meshVertices = mesh.vertices;

                string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString(); //naming the plane
                t.name = tilename;
                Tile tile = new Tile(t, updateTime, pos); //sets the tile gameObject and gives the update time
                tiles.Add(tilename, tile); //adding to the hashtable

                for (int i = 0; i <= planeSize; i++)
                {
                    for (int j = 0; j <= planeSize; j++)
                    {
                        string tmpTileName = "Tile_" + ((int)(pos.x + i)).ToString() + "_" + ((int)(pos.z + j)).ToString();
                        if (!pathfindingTiles.ContainsKey(tmpTileName))
                        {

                            // (Mathf.Abs(X - 11)) * (Mathf.Abs(Z - 11))
                            // (Mathf.Abs(i - 11)) * (Mathf.Abs(j - 11))
                            // (Mathf.Abs(i - (planeSize / 2)) * (Mathf.Abs(j - (planeSize / 2))

                            float y = t.GetComponent<GenerateTerrain>().getVertices()[(planeSize - (Mathf.Abs(i)) + Mathf.Abs(planeSize - j) * (planeSize + 1))].y;
                            //Debug.Log(y);
                            // (13, -85)
                            // (3, 5)
                            // (-2, 0) solution
                            // 51
                            //Debug.Log((planeSize - (Mathf.Abs(i)) + Mathf.Abs(planeSize - j) * (planeSize + 1)));

                            Tile tmpTile = new Tile(t, updateTime,
                                new Vector3(pos.x + i,
                                t.GetComponent<GenerateTerrain>().getVertices()[(planeSize - (Mathf.Abs(i)) + Mathf.Abs(planeSize - j) * (planeSize + 1))].y,
                                pos.z + j));
                            //Debug.Log("Adding: " + tmpTile.location);
                            pathfindingTiles.Add(tmpTileName, tmpTile);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < meshVertices.Length; i++)
        {
            //Debug.Log("TILES!");
            //Debug.Log(meshVertices[i].x);
            //Debug.Log(meshVertices[i].y);
            //Debug.Log(meshVertices[i].z);
        }
	}

    void Update()
    {
        //determine how far the player has moved since last terrain update
        int xMove = (int)(player.transform.position.x - startPos.x);
        int zMove = (int)(player.transform.position.z - startPos.z);

        /*
        //TODO: if it is greaterthan the planesize and divisible by 10
        //if the player has moved more than one planeSize
        if(Mathf.Abs(xMove) >= planeSize || Mathf.Abs(zMove) >= planeSize)
        {
            float updateTime = Time.realtimeSinceStartup; //use these time to delete the tiles 

            //force integer position and round to nearest tilesize
            int playerX = (int)(Mathf.Floor(player.transform.position.x / planeSize) * planeSize);
            int playerZ = (int)(Mathf.Floor(player.transform.position.z / planeSize) * planeSize);

            for(int x = -halfTileX; x < halfTileZ; x++)
            {
                for (int z = -halfTileZ; z < halfTileZ; z++)
                {
                    Vector3 pos = new Vector3((x * planeSize + startPos.x), 0, (z * planeSize + startPos.z));

                    string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString(); //naming the plane


                    if (!tiles.ContainsKey(tilename))//create the new tiles if they don't exist
                    {
                        GameObject t = (GameObject)Instantiate(plane, pos, Quaternion.identity); //creating the plane 
                        Mesh mesh = t.GetComponent<MeshFilter>().mesh;
                        meshVertices = mesh.vertices;


                        t.name = tilename;
                        Tile tile = new Tile(t, updateTime, pos); //sets the tile gameObject and gives the update time
                        tiles.Add(tilename, tile); //adding to the hashtable

                        for (int i = 0; i <= planeSize; i++)
                        {
                            for (int j = 0; j <= planeSize; j++)
                            {
                                string tmpTileName = "Tile_" + ((int)(pos.x + i)).ToString() + "_" + ((int)(pos.z + j)).ToString();
                                if (!pathfindingTiles.ContainsKey(tmpTileName))
                                {

                                    // (Mathf.Abs(X - 11)) * (Mathf.Abs(Z - 11))
                                    // (Mathf.Abs(i - 11)) * (Mathf.Abs(j - 11))
                                    //Debug.Log(Mathf.Abs(i - planeSize) * Mathf.Abs(j - planeSize));
                                    Tile tmpTile = new Tile(t, updateTime, 
                                        new Vector3(pos.x + i, 
                                        t.GetComponent<GenerateTerrain>().getVertices()[(planeSize - (Mathf.Abs(i)) +  Mathf.Abs(planeSize - j) * (planeSize + 1))].y, 
                                        pos.z + j));
                                    //Debug.Log("Adding: " + tmpTile.location.z);
                                    pathfindingTiles.Add(tmpTileName, tmpTile);
                                }
                            }
                        }
                    }
                    else //the tile does exist but don't wanna discount it, still a relavant tile
                    {
                        (tiles[tilename] as Tile).creationTime = updateTime; //update the tile to the new time
                    }
                    //zTile = zTile + 10;

                }
               // xTile = xTile + 10;
            }
            //destroy all tiles not just created or with time updated
            //and put new tiles and tiles to be kept in a new hashtable
            Hashtable newTerrain = new Hashtable();
            foreach(Tile tls in tiles.Values)
            {
                if(tls.creationTime != updateTime) //when not inside
                {
                    //delete gameobject
                    Destroy(tls.theTile);
                }
                else
                {
                    newTerrain.Add(tls.theTile.name, tls);
                    
                }
            }
            //copy new hashtable contents to the working hashtable
            tiles = newTerrain;

            startPos = player.transform.position;
        }
        */
        
    }

    public Hashtable getTileHashtable()
    {
        return pathfindingTiles;
    }
}
