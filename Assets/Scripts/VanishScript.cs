using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Wait());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
