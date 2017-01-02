using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AnimationEventFunction(string type)
    {
        transform.parent.GetComponent<UnitScript>().AnimationEventFunction(type);
    }
}
