﻿
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

    public AudioClip[] PlayerBasicAttackImpact;
    public AudioClip[] PlayerGetHitImpact;
    public AudioClip[] EnemyBasicAttackImpact;
	

	public AudioClip[] PlayerFootsteps;

    public AudioClip PlayerHeal;

    public AudioClip UIButtonPress;

    public AudioClip AmbientDefault;
    public AudioClip AmbientVictory;
    public AudioClip AmbientDefeat;

    public AudioClip ItemDrop;
    public AudioClip ItemDropLegendary;
    public AudioClip PickupItem;

    public AudioClip BossTeleport;

	public AudioClip AmbientBoss;

    public AudioClip[] PlayerDeath;
    public AudioClip BossDeath;
	public AudioClip[] BossLaugh;

    public AudioClip[] CrateDestroy;

	public AudioClip ChestOpen;
	public AudioClip LevelUp;
	public AudioClip QuestCompleted;


	// Use this for initialization
	void Start () {
        //PlayAmbientDefaultAudio(GameObject.Find("Player").GetComponent<AudioSource>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayCrateDestroyAudio()
    {
        int r = Random.Range(0, CrateDestroy.Length);
        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(CrateDestroy[r]);
    }

    public void PlayPlayerDeathAudio()
    {
        //PlayerBasicAttackImpact[0].Play();
        int r = Random.Range(0, PlayerDeath.Length);
        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(PlayerDeath[r]);
    }

    public void PlayBossDeathAudio()
    {
        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(BossDeath);
    }

    public void PlayBossTeleportAudio(AudioSource AS)
    {
        AS.PlayOneShot(BossTeleport);
    }

    public void PlayPickupItemAudio(AudioSource AS)
    {
        AS.PlayOneShot(PickupItem);
    }

    public void PlayItemDropAudio(AudioSource AS)
    {
        AS.PlayOneShot(ItemDrop);
    }
    public void PlayItemDropLegendaryAudio(AudioSource AS)
    {
        AS.PlayOneShot(ItemDropLegendary);
    }

    public void PlayAmbientDefaultAudio(AudioSource AS)
    {
        AS.PlayOneShot(AmbientDefault);
    }
    public void PlayAmbientVictoryAudio()
    {
        GameObject.Find("Player").GetComponent<AudioSource>().Stop();
        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(AmbientVictory);
    }
    public void PlayAmbientDefeatAudio()
    {
        GameObject.Find("Player").GetComponent<AudioSource>().Stop();
        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(AmbientDefeat);
    }

    public void PlayPlayerBasicAttackImpact(AudioSource AS)
    {
        //PlayerBasicAttackImpact[0].Play();
        int r = Random.Range(0, PlayerBasicAttackImpact.Length);
        AS.PlayOneShot(PlayerBasicAttackImpact[r]);
    }

    public void PlayPlayerGetHitImpact(AudioSource AS)
    {
        int r = Random.Range(0, PlayerGetHitImpact.Length);
        AS.PlayOneShot(PlayerGetHitImpact[r]);
    }

    public void PlayEnemyBasicAttackImpact(AudioSource AS)
    {
        //PlayerBasicAttackImpact[0].Play();
        int r = Random.Range(0, EnemyBasicAttackImpact.Length);
        AS.PlayOneShot(EnemyBasicAttackImpact[r]);
    }

    public void PlayPlayerHealAudio(AudioSource AS)
    {
        AS.PlayOneShot(PlayerHeal);
    }


    private AudioClip oldFootsteps;
    private float footstepEnd;
   
    public void PlayFootstepAudio(AudioSource AS)
    {
        if (oldFootsteps == null)
        {
            int r = Random.Range(0, PlayerFootsteps.Length);
            AS.PlayOneShot(PlayerFootsteps[r]);
            oldFootsteps = PlayerFootsteps[r];
            footstepEnd = Time.time + oldFootsteps.length;
        }
        else if (footstepEnd <= Time.time)
        {
            int r = Random.Range(0, PlayerFootsteps.Length);
            AS.PlayOneShot(PlayerFootsteps[r]);
            oldFootsteps = PlayerFootsteps[r];
            footstepEnd = Time.time + oldFootsteps.length;
        }
        
    }

    public void PlayUIButtonPressAudio(AudioSource AS)
    {
        AS.PlayOneShot(UIButtonPress);
    }

	public void PlayAmbientBossAudio()
	{
		GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(AmbientBoss);
	}

	public void PlayBossLaughAudio()
	{
		GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(BossLaugh[Random.Range(0, BossLaugh.Length-1)]);
	}

	public void PlayChestOpenAudio()
	{
		GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(ChestOpen);
	}

	public void PlayLevelUpAudio()
	{
		GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(LevelUp);
	}

	public void PlayQuestCompletedAudio()
	{
		GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(QuestCompleted);
	}
}