using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rotate : MonoBehaviour {

    public bool B;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (B)
        {
            transform.position = new Vector3(transform.position.x, 1.5f, 0);
        } else
        {
            transform.position = new Vector3(transform.position.x, 0.9f, 0);
        }

    }
}
