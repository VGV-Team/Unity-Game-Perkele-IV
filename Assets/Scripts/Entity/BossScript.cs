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

	public GameObject SpecialAmulet;

    private AudioManagerScript AudioManager;

    GameObject particle4;

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

        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        particle4 = GameObject.Find("ParticlePhase4");
        particle4.SetActive(false);
    }

    bool dead = false;

    IEnumerator BossDead()
    {
        yield return new WaitForSeconds(7.0f);
        AudioManager.PlayBossTeleportAudio(GameObject.Find("Player").GetComponent<AudioSource>());

        //GameObject.Find("Main Camera").GetComponent<MainCameraScript>().EndGame();


        //yield return new WaitForSeconds(1.0f);

        GlobalsScript.IsGameOver = true;
        this.gameObject.SetActive(false);

        
        
    }

	// Update is called once per frame
	void Update () {
		
        if (!dead && Boss.GetComponent<UnitScript>().HP <= 0)
        {
			print("ewqeqwewqqewqw" + Boss.GetComponent<UnitScript>().HP);
			dead = true;
			Boss.GetComponent<UnitScript>().HPChange = 0;
			particle4.SetActive(true);
			AudioManager.PlayAmbientVictoryAudio();
			AudioManager.PlayBossDeathAudio();
			for (int i = 0; i < SpawnedMinions.Count; i++)
			{
				Destroy(SpawnedMinions[i]);
			}
			StartCoroutine(BossDead());
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
                AudioManager.PlayBossTeleportAudio(this.GetComponent<AudioSource>());

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
                AudioManager.PlayBossTeleportAudio(this.GetComponent<AudioSource>());

                ES.StopMovement();
                ES.StartHealAnimation();

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
        else
        {
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

            yield return new WaitForSeconds(7);

            StartCoroutine(NecromancerAbility());
        }
        
    }
}
