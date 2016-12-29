using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : EntityScript
{

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OpenChest()
    {
        this.GetComponent<Animator>().Play("Open chest");
    }
}
