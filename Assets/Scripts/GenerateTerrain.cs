using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://www.youtube.com/watch?v=uDMfVMKM_98&t=1217s

public class GenerateTerrain : MonoBehaviour {

    //correct amount of height and detail in plane
    int heightScale = 4;
    float detailScale = 10.0f;
    public Vector3[] vert;
    List<GameObject> myTrees = new List<GameObject>();
    List<GameObject> myWalls = new List<GameObject>();

    private void Awake()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        for (int v = 0; v < vertices.Length; v++)
        {
            //lifting up each vertices
            vertices[v].y = Mathf.PerlinNoise((vertices[v].x + this.transform.position.x) / detailScale,
                                                (vertices[v].z + this.transform.position.z) / detailScale) * heightScale;
            //Debug.Log(vertices[v].y);
            if (vertices[v].y > 2.6 && Mathf.PerlinNoise((vertices[v].x + 5) / 10, (vertices[v].z + 5) / 10) * 10 > 4.0)
            {
                GameObject newTree = TreePool.getTree(); //assign our tree that the pool gives us
                if (newTree != null) //if there is a tree
                {
                    //construct the tree
                    Vector3 treePos = new Vector3(vertices[v].x + this.transform.position.x,
                                                vertices[v].y, vertices[v].z + this.transform.position.z);
                    newTree.transform.position = treePos; //set the tree here
                    newTree.SetActive(true);//make it visisble
                    myTrees.Add(newTree);//add it to the list
                }
            }
            if ((vertices[v].x + this.transform.position.x) == -100
                && (vertices[v].z + this.transform.position.z) >= -100 && (vertices[v].z + this.transform.position.z) <= 90)
            {
                GameObject newWall = WallPool.getWall(); //assign our wall that the pool gives us
                if (newWall != null) //if there is a wall
                {
                    //construct the wall
                    Vector3 wallPos = new Vector3(vertices[v].x + this.transform.position.x,
                                                vertices[v].y, vertices[v].z + this.transform.position.z);
                    newWall.transform.position = wallPos; //set the wall here
                    newWall.SetActive(true);//make it visisble
                    myWalls.Add(newWall);//add it to the list
                }
            }
            if ((vertices[v].x + this.transform.position.x) == 90
                && (vertices[v].z + this.transform.position.z) >= -100 && (vertices[v].z + this.transform.position.z) <= 90)
            {
                GameObject newWall = WallPool.getWall(); //assign our wall that the pool gives us
                if (newWall != null) //if there is a wall
                {
                    //construct the wall
                    Vector3 wallPos = new Vector3(vertices[v].x + this.transform.position.x,
                                                vertices[v].y, vertices[v].z + this.transform.position.z);
                    newWall.transform.position = wallPos; //set the wall here
                    newWall.SetActive(true);//make it visisble
                    myWalls.Add(newWall);//add it to the list
                }
            }
            if ((vertices[v].z + this.transform.position.z) == 90
                && (vertices[v].x + this.transform.position.x) >= -100 && (vertices[v].x + this.transform.position.x) <= 90)
            {
                GameObject newWall = WallPool.getWall(); //assign our wall that the pool gives us
                if (newWall != null) //if there is a wall
                {
                    //construct the wall
                    Vector3 wallPos = new Vector3(vertices[v].x + this.transform.position.x,
                                                vertices[v].y, vertices[v].z + this.transform.position.z);
                    newWall.transform.position = wallPos; //set the wall here
                    newWall.SetActive(true);//make it visisble
                    myWalls.Add(newWall);//add it to the list
                }
            }
            if ((vertices[v].z + this.transform.position.z) == -100
                && (vertices[v].x + this.transform.position.x) >= -100 && (vertices[v].x + this.transform.position.x) <= 90)
            {
                GameObject newWall = WallPool.getWall(); //assign our wall that the pool gives us
                if (newWall != null) //if there is a wall
                {
                    //construct the wall
                    Vector3 wallPos = new Vector3(vertices[v].x + this.transform.position.x,
                                                vertices[v].y, vertices[v].z + this.transform.position.z);
                    newWall.transform.position = wallPos; //set the wall here
                    newWall.SetActive(true);//make it visisble
                    myWalls.Add(newWall);//add it to the list
                }
            }
        }
        vert = vertices;
        mesh.vertices = vertices; //setting the vertices back to the mesh
        mesh.RecalculateBounds(); //recalculate because they are changed
        mesh.RecalculateNormals();
        this.gameObject.AddComponent<MeshCollider>(); //adding mesh collider so you can walk
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3[] getVertices()
    {
        return vert;
    }
}
