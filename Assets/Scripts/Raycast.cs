using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {

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
                //Debug.DrawLine(ray.origin, hit.point);
                GameObject.Find("Ethan").transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                //(hit.point);
            }
            else
                print("Krscen maticek");
        }
            
    }

    private void FixedUpdate()
    {
        
    }
}
