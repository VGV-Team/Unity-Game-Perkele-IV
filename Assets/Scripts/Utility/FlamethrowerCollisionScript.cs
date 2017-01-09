using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerCollisionScript : MonoBehaviour {

    public AbilityScript Ability;
    public GameObject Caster;

	// Use this for initialization
	void Start () {
        float seconds = this.GetComponent<FireBaseScript>().Duration;
        StartCoroutine(DisableCollider(seconds));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
        {
            Ability.AbilityTypeFlamethrowerImpact(Caster, collision.gameObject);
        }
    }

    IEnumerator DisableCollider(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.GetComponent<Collider>().enabled = false;
    }
}
