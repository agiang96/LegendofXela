using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour {

	// Update is called once per frame
	void Start () {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject scoreManager = GameObject.Find("ScoreManager");
        if(scoreManager != null)
        {
            Destroy(scoreManager);
        }
	}
}
