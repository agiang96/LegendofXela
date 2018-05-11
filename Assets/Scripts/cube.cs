using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour {

    float movespeed = 7f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(movespeed*Input.GetAxis("Horizontal") * Time.deltaTime, 0f, movespeed*Input.GetAxis("Vertical") * Time.deltaTime);
	}
}
