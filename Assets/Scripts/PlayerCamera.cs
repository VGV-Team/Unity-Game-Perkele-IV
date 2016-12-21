using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                //GameObject.Find("Player").transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                // Set a waypoint, player update will move the player to the waypoint
                GameObject waypoint = new GameObject("PlayerWaypoint");
                waypoint.transform.position = hit.point;

                GameObject.Find("Player").SendMessage("SetWaypoint", waypoint);
                
            }
            else
                print("Krscen maticek");
        }
            
    }

    private void FixedUpdate()
    {
        
    }
}
