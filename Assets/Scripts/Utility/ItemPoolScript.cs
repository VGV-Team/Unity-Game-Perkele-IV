﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ItemPoolScript : MonoBehaviour {

    // Edit this list in the editor!!
    public List<GameObject> CommonItemPool;
    public List<GameObject> RareItemPool;
    public List<GameObject> LegendaryItemPool;
    public List<GameObject> EpicItemPool;

    private AudioManagerScript AudioManager;

    // Use this for initialization
    void Start () {
        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
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

    public void LootDrop(GameObject looter, int discoveryBonus, int rareChance, int legendaryChance, int epicChance, Transform spawnPosition, [Optional] Vector3 velocity)
    {
        StartCoroutine(LootDropWithDelay(looter, discoveryBonus, rareChance, legendaryChance, epicChance, spawnPosition, velocity));
    }

    IEnumerator LootDropWithDelay(GameObject looter, int discoveryBonus, int rareChance, int legendaryChance, int epicChance, Transform spawnPosition, [Optional] Vector3 velocity)
    {

        int discovery = (int)looter.GetComponent<UnitScript>().Discovery + discoveryBonus;
        if (looter.tag == "Player" && looter.GetComponent<UnitScript>().EquippedItems.AmuletSlot)
        {
            discovery += looter.GetComponent<UnitScript>().EquippedItems.AmuletSlot.GetComponent<ItemScript>().Discovery;
        }

        int maxItems = 0;
        int minItems = 0;
        if (discovery < 20) { maxItems = 1; }
        else if (discovery < 30) { maxItems = 2; }
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


            //AUDIO
            if (item.GetComponent<ItemScript>().Rarity == RarityType.Legendary ||
                item.GetComponent<ItemScript>().Rarity == RarityType.Epic)
                AudioManager.PlayItemDropLegendaryAudio(looter.GetComponent<AudioSource>());
            else
                AudioManager.PlayItemDropAudio(looter.GetComponent<AudioSource>());
                

            //Randomize attributes +/- 15%
            item.GetComponent<ItemScript>().Damage += (item.GetComponent<ItemScript>().Damage * Random.Range(-15, 15) / 100.0f);
            item.GetComponent<ItemScript>().CriticalChance += (item.GetComponent<ItemScript>().CriticalChance * Random.Range(-15, 15) / 100.0f);
            item.GetComponent<ItemScript>().CriticalDamage += (item.GetComponent<ItemScript>().CriticalDamage * Random.Range(-15, 15) / 100.0f);
            item.GetComponent<ItemScript>().AttackSpeed += (item.GetComponent<ItemScript>().AttackSpeed * Random.Range(-15, 15) / 100.0f);
            item.GetComponent<ItemScript>().Armor += (item.GetComponent<ItemScript>().Armor * Random.Range(-15, 15) / 100.0f);
            item.GetComponent<ItemScript>().Discovery += (int)(item.GetComponent<ItemScript>().Discovery * Random.Range(-15, 15) / 100.0f);

            if (velocity == Vector3.zero) velocity = new Vector3(Random.Range(-1, 1), Random.Range(1, 1), Random.Range(-1, 1));

            item.GetComponent<Rigidbody>().velocity = velocity;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
