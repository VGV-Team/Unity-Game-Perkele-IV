using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCamera : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100) && hit.transform.tag == "Terrain") // TODO: chack for Enemy, Item, Chest tag
            {

                // Check if we clicked on unwalkable terrain ('blocked')
                NavMeshHit nmHit;
                bool blocked = NavMesh.Raycast(Camera.main.transform.position, hit.point, out nmHit, NavMesh.AllAreas);

                if (blocked)
                {
                    print("Blocked!");
                    return;
                }
                //GameObject.Find("Player").transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                // Set a waypoint, player update will move the player to the waypoint
                GameObject waypoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                waypoint.name = "PlayerWaypoint";
                waypoint.GetComponent<Renderer>().material.color = Color.red;
                waypoint.transform.position = hit.point;

                player.SendMessage("SetWaypoint", waypoint);
                
            }
            else
                print("Krscen maticek");
        }
            
    }

    private void FixedUpdate()
    {
        
    }
}
