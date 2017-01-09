
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

    public AudioClip[] PlayerBasicAttackImpact;
    public AudioClip[] PlayerGetHitImpact;
    public AudioClip[] EnemyBasicAttackImpact;

    public AudioClip[] PlayerFootsteps;

    public AudioClip PlayerHeal;

    public AudioClip UIButtonPress;

    public AudioClip AmbientDefault;

    public AudioClip ItemDrop;
    public AudioClip PickupItem;


	// Use this for initialization
	void Start () {
        //PlayAmbientDefaultAudio(GameObject.Find("Player").GetComponent<AudioSource>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayPickupItemAudio(AudioSource AS)
    {
        AS.PlayOneShot(PickupItem);
    }

    public void PlayItemDropAudio(AudioSource AS)
    {
        AS.PlayOneShot(ItemDrop);
    }

    public void PlayAmbientDefaultAudio(AudioSource AS)
    {
        AS.PlayOneShot(AmbientDefault);
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
}
