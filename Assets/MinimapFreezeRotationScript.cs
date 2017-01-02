using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFreezeRotationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
