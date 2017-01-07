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
            StartCoroutine(DropLootDelay(2));
        }
        
    }

    IEnumerator DropLootDelay(int delay)
    {
        //Vector3 velocity = -this.transform.forward;
        //velocity.x *= Random.Range(-20, 20) / 100.0f;
        //velocity.z *= Random.Range(-20, 20) / 100.0f;
        //velocity.y = Random.Range(100, 200) / 100.0f;
        yield return new WaitForSeconds(delay);

        Vector3 thisVelocity = (GameObject.Find("Player").transform.position - this.transform.position).normalized*3;
        thisVelocity.y = 0;
        thisVelocity.y = 2.0f;

        GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().LootDrop(
                    100,
                    100,
                    60,
                    30,
                    this.transform,
                    velocity: thisVelocity);
    }
}
