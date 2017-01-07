using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ItemPoolScript : MonoBehaviour {

    // Edit this list in the editor!!
    public List<GameObject> CommonItemPool;
    public List<GameObject> RareItemPool;
    public List<GameObject> LegendaryItemPool;
    public List<GameObject> EpicItemPool;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GenerateRandomItem(int rareChance, int legendaryChance, int epicChance)
    {
        int r = Random.Range(0, 100);
        List<GameObject> itemPool;
        if (r < epicChance)
        {
            //Epic
            itemPool = EpicItemPool;
        }
        else if (r < legendaryChance)
        {
            //Legendary
            itemPool = LegendaryItemPool;
        }
        else if (r < rareChance)
        {
            //Rare
            itemPool = RareItemPool;
        }
        else
        {
            //Common
            itemPool = CommonItemPool;
        }



        r = Random.Range(0, itemPool.Count);
        return itemPool[r];
    }

    public void LootDrop(int discovery, int rareChance, int legendaryChance, int epicChance, Transform spawnPosition, [Optional] Vector3 velocity)
    {
        StartCoroutine(LootDropWithDelay(discovery, rareChance, legendaryChance, epicChance, spawnPosition, velocity));
    }

    IEnumerator LootDropWithDelay(int discovery, int rareChance, int legendaryChance, int epicChance, Transform spawnPosition, [Optional] Vector3 velocity)
    {
        int maxItems = 0;
        int minItems = 0;
        if (discovery < 20) { maxItems = 1; }
        else if (discovery < 40) { maxItems = 2; }
        else if (discovery < 50) { maxItems = 3; }
        else { maxItems = 3 + discovery / 20; minItems = 1; }

        //OR discovery = chance to drop until no drop


        int numItems = Random.Range(minItems, maxItems);

        for (int i = 0; i < numItems; i++)
        {
            GameObject item = GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().GenerateRandomItem(rareChance, legendaryChance, epicChance);
            item = GameObject.Instantiate(item);

            item.transform.position = spawnPosition.position;
            item.transform.position += new Vector3(0, 3, 0);
            //item.GetComponent<ItemScript>().Name += " " + Random.Range(1000, 5555);

            //Randomize attributes +/- 15%
            item.GetComponent<ItemScript>().Damage *= (Random.Range(-15, 15) / 100.0f);
            item.GetComponent<ItemScript>().CriticalChance *= (Random.Range(-15, 15) / 100.0f);
            item.GetComponent<ItemScript>().CriticalDamage *= (Random.Range(-15, 15) / 100.0f);

            if (velocity == Vector3.zero) velocity = new Vector3(Random.Range(-5, 5), Random.Range(1, 5), Random.Range(-5, 5));

            item.GetComponent<Rigidbody>().velocity = velocity;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
