using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//taken from 
//https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu
public class QuitOnClick : MonoBehaviour {
     
    /* ****************************************
     * Function: Quit
     * ****************************************
     * If you put this method on a button, 
     * It will quit the application, unless 
     * you're in the Editor
     * ****************************************
     */
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
