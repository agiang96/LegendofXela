using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://www.youtube.com/watch?v=uDMfVMKM_98&t=1217s

public class TreePool : MonoBehaviour
{

    static int numTrees = 3000;
    public GameObject treePrefab;
    static GameObject[] trees; //generate an array of trees to hold them

    // Use this for initialization
    void Start()
    {
        trees = new GameObject[numTrees]; //set the array to the size of numtrees
        for (int i = 0; i < numTrees; i++)
        {
            trees[i] = (GameObject)Instantiate(treePrefab, Vector3.zero, Quaternion.identity); //create a tree
            trees[i].SetActive(false); //set all to be inactive making them invisible
        }
    }

    static public GameObject getTree()
    {
        for (int i = 0; i < numTrees; i++)//looping to the pool of trees and see if they are active
        {
            if (!trees[i].activeSelf)//if not active then return trees
            {
                return trees[i];
            }
        }
        return null; //if no more trees
    }

    // Update is called once per frame
    void Update()
    {

    }
}
