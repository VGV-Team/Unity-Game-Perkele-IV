using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : EntityScript {

    public GameObject remains;
    //bool destroyed=false;
	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyCrate()
    {
        Instantiate(remains, transform.position, transform.rotation);
        this.GetComponent<EntityScript>().Active = false;
        this.GetComponent<Collider>().enabled = false;
        Destroy(gameObject);
    }

}
