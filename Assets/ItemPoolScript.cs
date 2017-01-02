using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoolScript : MonoBehaviour {

    // Edit this list in the editor!!
    public List<GameObject> ItemPool;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GenerateRandomItem()
    {
        int r = Random.Range(0, ItemPool.Count);

        //TODO: Generation logics based on rarity + random attributes + random name

        return ItemPool[r];
    }
}
