using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointScript : MonoBehaviour {

    public GameObject waypoint;
    public float MovementSpeed;

    bool walkAnim;

    private Transform model;
    private GameObject camera;

	// Use this for initialization
	void Start () {
        waypoint = null;
        walkAnim = false;

        model = this.transform.FindChild("Model");
        camera = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {

        CheckMouseClick();

        // If waypoint for this object exists, move to it!
        if (waypoint)
        {
            model.GetComponent<NavMeshAgent>().SetDestination(waypoint.transform.position);


            if (!walkAnim)
            {
                this.GetComponent<Animator>().CrossFade("walk", 0.15f);
                walkAnim = true;
            }

            if (Vector3.Distance(model.transform.position, waypoint.transform.position) < 0.1f)
            {
                this.GetComponent<Animator>().CrossFade("idle", 0.15f);
                walkAnim = false;
                Destroy(waypoint);
            }
        } 
        else
        {
            model.GetComponent<NavMeshAgent>().velocity = new Vector3(0,0,0);
        }

        //Center camera to this objects position
        camera.transform.position = model.position + new Vector3(-0.0f, 8.0f, -7.0f);

    }


    // Set a waypoint for this ovbject to move to
    public void SetWaypoint(GameObject w)
    {
        if (this.waypoint != null) Destroy(this.waypoint);
        this.waypoint = w;
    }

    void CheckMouseClick()
    {
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
                waypoint.GetComponent<Collider>().enabled = false;
                waypoint.transform.position = hit.point;

                SetWaypoint(waypoint);

            }
            else
                print("Krscen maticek");
        }
    }


    /* ***********************+
     * 
     * OLD WAYPOINT CODE
     * 
     * void Update () {

        if (waypoint)
        {
            //this.transform.LookAt(waypoint.transform);

            

           

            //model.transform.eulerAngles = new Vector3(0.0f, 45, 0.0f);
            //model
            //model.GetComponent<Rigidbody>().velocity = transform.TransformDirection(waypoint.transform.position* Time.deltaTime * MovementSpeed);
            //model.transform.position = Vector3.MoveTowards(model.transform.position, waypoint.transform.position, Time.deltaTime * MovementSpeed);
            //model.transform.position = Vector3.Lerp(model.transform.position, waypoint.transform.position, Time.fixedDeltaTime * MovementSpeed);

            
            

            
            //model.transform.rotation = Quaternion.Euler(0.0f, model.transform.rotation.y, 0.0f);

            //if (!blocked)
            {
                model.GetComponent<NavMeshAgent>().SetDestination(waypoint.transform.position);

                Vector3 temp = model.rotation.eulerAngles;
                temp.x = 0;
                temp.z = 0;
                model.rotation = Quaternion.Euler(temp);

                model.LookAt(waypoint.transform);
                
                //GameObject.Find


                //bool isName = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("walk");

                if (!walkAnim)
                {
                    this.GetComponent<Animator>().CrossFade("walk", 0.15f);
                    walkAnim = true;
                }



                if (Vector3.Distance(model.transform.position, waypoint.transform.position) < 0.1f)
                {
                    this.GetComponent<Animator>().CrossFade("idle", 0.15f);
                    walkAnim = false;
                    Destroy(waypoint);
                    }
                CharacterController cc = this.GetComponent<CharacterController>();

                cc.Move(waypoint.transform.position * Time.deltaTime * 0.005f);
            }

        } 
        else
        {
            model.GetComponent<NavMeshAgent>().velocity = new Vector3(0,0,0);
        }

        camera.transform.position = model.position + new Vector3(-0.0f, 8.0f, -7.0f);

    }

    */
}
