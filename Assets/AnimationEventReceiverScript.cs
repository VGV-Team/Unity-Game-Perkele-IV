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

    void AnimationEventFunction(AnimationEvent e)
    {
        Debug.Log(this.transform.parent.name + "HAS RECIEVED ANIM EVENT " + e.stringParameter);
        transform.parent.GetComponent<UnitScript>().AnimationEventFunctionRelay(e.stringParameter);
    }
}
