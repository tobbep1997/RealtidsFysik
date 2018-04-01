using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopEverything : MonoBehaviour {

    bool b = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y <= 0 && b)
            Debug.Break();
        if (transform.position.y > 0)
            b = true;
            
	}
}
