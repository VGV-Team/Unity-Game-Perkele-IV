using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premikanje : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W))
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + 0.1f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - 0.1f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
    }
}
