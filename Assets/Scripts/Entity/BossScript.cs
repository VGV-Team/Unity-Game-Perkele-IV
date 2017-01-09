using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    GameObject Boss;
    EnemyScript ES;

    int Phase = 1;

    public bool Necromancer = false;
    private GameObject NecromancerUnit;

    private Vector3 initialPosition;
    private Quaternion initialRotation;


    List<GameObject> SpawnedMinions;

    // Use this for initialization
    void Start () {
        Boss = GameObject.Find("Boss");
        ES = Boss.GetComponent<EnemyScript>();

        ES.Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 2, 0, 0, 2, 20)); //20 <-- Strength
        ES.Abilities.Add(new AbilityScript("Fireball", AbilityType.Fireball, 1, 0, 0, ES.ViewRange, 20, GameObject.Find("UISpritesFireball").transform.GetComponent<SpriteRenderer>().sprite));
        ES.RangedAttack = false;
        ES.MeleeAttack = true;

        NecromancerUnit = GameObject.Find("EffectLoader").GetComponent<EffectLoaderScript>().NecromancerUnit;

        initialPosition = Boss.transform.position;
        initialRotation = Boss.transform.rotation;

        SpawnedMinions = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Boss.GetComponent<UnitScript>().HP <= 0)
        {
            Boss.GetComponent<UnitScript>().HPChange = 0;
            for (int i = 0; i < SpawnedMinions.Count; i++)
            {
                Destroy(SpawnedMinions[i]);
            }
        }

        if (Phase == 1)
        {
            if (ES.HP < 2 * ES.MaxHP / 3)
            {
                Phase = 2;
                ES.MeleeAttack = false;
                ES.RangedAttack = true;

                Component[] children = Boss.GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.name == "Sword")
                    {
                        child.gameObject.SetActive(false);
                    }
                    if (child.name == "Staff")
                    {
                        child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                    if (child.name == "ParticlePhase1")
                    {
                        child.gameObject.SetActive(false);
                    }
                    if (child.name == "ParticlePhase2")
                    {
                        child.gameObject.layer = 0;
                        Transform[] t = child.GetComponentsInChildren<Transform>();
                        for (int i = 0; i < t.Length; i++)
                        {
                            t[i].gameObject.layer = 0;
                        }
                    }
                }

                //SOUND EFFECT
                //TODO

                //Teleport
                Boss.transform.position = initialPosition;
                Boss.transform.rotation = initialRotation;

            }
        }

        if (Phase == 2)
        {
            if (ES.HP < ES.MaxHP / 3)
            {
                Phase = 3;
                ES.MeleeAttack = false;
                ES.RangedAttack = true;

                Component[] children = Boss.GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.name == "ParticlePhase2")
                    {
                        child.gameObject.SetActive(false);
                    }
                    if (child.name == "ParticlePhase3")
                    {
                        child.gameObject.layer = 0;
                        Transform[] t = child.GetComponentsInChildren<Transform>();
                        for (int i = 0; i < t.Length; i++)
                        {
                            t[i].gameObject.layer = 0;
                        }
                    }
                    if (child.name == "Shield")
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                //NECROMANCER
                ES.RangedAttack = false;
                ES.Necromancer = true;

                //TODO: SOUND EFFECT
                ///sadasd
                ///

                //Teleport
                Boss.transform.position = initialPosition;
                Boss.transform.rotation = initialRotation;

                StartCoroutine(NecromancerAbility());

            }
        }
        

	}

    IEnumerator NecromancerAbility()
    {
        if (Boss.GetComponent<UnitScript>().HP <= 0) yield return new WaitForSeconds(0);

        ES.StartHealAnimation();

        GameObject newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(2.0f, 5.0f, 2.0f);
        SpawnedMinions.Add(newUnit);

        newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(2.0f, 5.0f, -2.0f);
        SpawnedMinions.Add(newUnit);

        newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(-2.0f, 5.0f, 2.0f);
        SpawnedMinions.Add(newUnit);

        newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(-2.0f, 5.0f, -2.0f);
        SpawnedMinions.Add(newUnit);

        yield return new WaitForSeconds(10);

        StartCoroutine(NecromancerAbility());
    }
}
