using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndgameBarrierTriggerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("ENDGAME BARRIER TRIGGER!");
            GameObject barrier = GameObject.Find("EndgameBarrier");
            Transform[] t = barrier.GetComponentsInChildren<Transform>();
            for (int i = 0; i < t.Length; i++)
            {
                
                if (t[i].GetComponent<MeshRenderer>()) t[i].GetComponent<MeshRenderer>().enabled = true;
                if (t[i].GetComponent<NavMeshObstacle>()) t[i].GetComponent<NavMeshObstacle>().enabled = true;
                if (t[i].GetComponent<Collider>()) t[i].GetComponent<Collider>().enabled = true;
            }

            //Trigger boss
            //GameObject.Find("Boss").GetComponent<EnemyScript>().ViewRange = 50;
            
            this.gameObject.SetActive(false);
        }
    }
}
