using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : EntityScript {

    public GameObject remains;

    private AudioManagerScript AudioManager;

    //bool destroyed=false;
	// Use this for initialization
	void Start () {
        base.Start();

        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyCrate(GameObject destroyer)
    {
        GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().LootDrop(
                                destroyer,
                                -10,
                                25,
                                5,
                                0,
                                this.transform);

        Debug.Log("qwe");
        AudioManager.PlayCrateDestroyAudio();
        Instantiate(remains, transform.position, transform.rotation);
        this.GetComponent<EntityScript>().Active = false;
        this.GetComponent<Collider>().enabled = false;
        Destroy(gameObject);
    }

}
