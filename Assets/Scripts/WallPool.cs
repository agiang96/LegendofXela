using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPool : MonoBehaviour {


    static int numWalls = 3000;
    public GameObject wallPrefab;
    static GameObject[] walls; //generate an array of trees to hold them

    // Use this for initialization
    void Start()
    {
        walls = new GameObject[numWalls]; //set the array to the size of numtrees
        for (int i = 0; i < numWalls; i++)
        {
            walls[i] = (GameObject)Instantiate(wallPrefab, Vector3.zero, Quaternion.identity); //create a tree
            walls[i].SetActive(false); //set all to be inactive making them invisible
        }
    }

    static public GameObject getWall()
    {
        for (int i = 0; i < numWalls; i++)//looping to the pool of trees and see if they are active
        {
            if (!walls[i].activeSelf)//if not active then return trees
            {
                return walls[i];
            }
        }
        return null; //if no more trees
    }

}
