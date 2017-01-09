using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHealEffectScript : MonoBehaviour {

    public float Seconds = 5;

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyThisObject(Seconds));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DestroyThisObject(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
