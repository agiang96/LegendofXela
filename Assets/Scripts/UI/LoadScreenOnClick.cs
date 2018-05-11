using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//taken from:
//https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu
public class LoadScreenOnClick : MonoBehaviour {

    /* ****************************************
     * Function: LoadByIndex
     * ****************************************
     * If you put this method on a button, 
     * Loads a Scene according to the Index
     * according to the Build
     * ****************************************
     */
	public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex); 
    }
}
