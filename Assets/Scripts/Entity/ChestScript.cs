using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : EntityScript
{

    public bool Opened = false;
    public int ScrapRequired = 0;

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
        if (!Opened)
        {
            this.GetComponent<Animator>().Play("Open chest");
            Opened = true;
        }
        
    }
}
