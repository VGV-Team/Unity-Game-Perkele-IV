using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour {

    public GameObject waypoint;
    public float MovementSpeed;

    bool walkAnim;

	// Use this for initialization
	void Start () {
        waypoint = null;
        walkAnim = false;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (waypoint)
        {
            //this.transform.LookAt(waypoint.transform);

            Transform model = this.transform.FindChild("Model");

            model.LookAt(waypoint.transform);
            //model.transform.rotation = Quaternion.Euler(0.0f, model.transform.rotation.y, 0.0f);

            Vector3 temp = model.rotation.eulerAngles;
            temp.x = 0;
            temp.z = 0;
            model.rotation = Quaternion.Euler(temp);

            //model.transform.eulerAngles = new Vector3(0.0f, 45, 0.0f);
            //model
            model.transform.position = Vector3.MoveTowards(model.transform.position, waypoint.transform.position, Time.deltaTime * MovementSpeed);

            

            this.transform.FindChild("Main Camera").transform.position = model.position + new Vector3(-0.0f, 8.0f, -7.0f) ;

            //bool isName = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("walk");

            if (!walkAnim)
            {
                this.GetComponent<Animator>().CrossFade("walk", 0.15f);
                walkAnim = true;
            }
                


            if (Vector3.Distance(model.transform.position, waypoint.transform.position) < 1.0f)
            {
                this.GetComponent<Animator>().CrossFade("idle", 0.15f);
                walkAnim = false;
                Destroy(waypoint);
            }
            /*CharacterController cc = this.GetComponent<CharacterController>();

            cc.Move(waypoint.transform.position * Time.deltaTime * 0.005f);*/

        }

	}

    public void SetWaypoint(GameObject w)
    {
        if (this.waypoint != null) Destroy(this.waypoint);
        this.waypoint = w;
    }
}
