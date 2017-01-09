using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    GameObject Boss;
    EnemyScript ES;

    int Phase = 1;

    public bool Necromancer = false;
    private GameObject NecromancerUnit;


	// Use this for initialization
	void Start () {
        Boss = GameObject.Find("Boss");
        ES = Boss.GetComponent<EnemyScript>();

        ES.Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 2, 0, 0, 2, 20)); //20 <-- Strength
        ES.Abilities.Add(new AbilityScript("Fireball", AbilityType.Fireball, 1, 0, 0, ES.ViewRange, 20, GameObject.Find("UISpritesFireball").transform.GetComponent<SpriteRenderer>().sprite));
        ES.RangedAttack = false;
        ES.MeleeAttack = true;

        NecromancerUnit = GameObject.Find("EffectLoader").GetComponent<EffectLoaderScript>().NecromancerUnit;
    }
	
	// Update is called once per frame
	void Update () {
		
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
                }

                //NECROMANCER
                ES.RangedAttack = false;
                ES.Necromancer = true;

                //TODO: SOUND EFFECT, TLELEPORT TO CENTER OF ROOM
                ///sadasd

                StartCoroutine(NecromancerAbility());

            }
        }
        

	}

    IEnumerator NecromancerAbility()
    {
        

        ES.StartHealAnimation();

        GameObject newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(2.0f, 0.0f, 2.0f);

        newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(2.0f, 0.0f, -2.0f);

        newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(-2.0f, 0.0f, 2.0f);

        newUnit = GameObject.Instantiate(NecromancerUnit);
        newUnit.transform.position = Boss.transform.position + new Vector3(-2.0f, 0.0f, -2.0f);

        yield return new WaitForSeconds(10);

        StartCoroutine(NecromancerAbility());
    }
}
