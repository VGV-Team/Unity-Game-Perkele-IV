using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class WaypointScript : MonoBehaviour {

    public GameObject waypoint;
    public float MovementSpeed;

    public bool SampleWaypointPosition;

    bool walkAnim;

    private Transform model;
    private GameObject camera;
    private Animation animation;
    private GameObject mouseOverObject;

    private float animationFadeFactor;

    private Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
        waypoint = null;
        walkAnim = false;

        animationFadeFactor = 0.15f;

        model = this.transform.FindChild("Model");
        camera = GameObject.Find("Main Camera");
        animation = model.GetComponent<Animation>();

        cameraOffset = new Vector3(-0.0f, 8.0f, -8.0f);
    }
	
	// Update is called once per frame
	void Update () {

        CheckMouseClick();

        CheckMouseScroll();

        // If waypoint for this object exists, move to it!
        if (waypoint)
        {
            model.GetComponent<NavMeshAgent>().SetDestination(waypoint.transform.position);


            if (!walkAnim)
            {
                //this.GetComponent<Animator>().CrossFade("walk", 0.15f);
                animation.CrossFade("run", animationFadeFactor);
                walkAnim = true;
            }

            if (Vector3.Distance(model.transform.position, waypoint.transform.position) < 0.2f)
            {
                //this.GetComponent<Animator>().CrossFade("idle", 0.15f);
                animation.CrossFade("idle", animationFadeFactor);
                walkAnim = false;
                Destroy(waypoint);
            }
        } 
        else
        {
            model.GetComponent<NavMeshAgent>().velocity = new Vector3(0,0,0);
        }

        //Center camera to this objects position
        camera.transform.position = model.position + cameraOffset;

    }


    // Set a waypoint for this ovbject to move to
    public void SetWaypoint(Vector3 point)
    {
        if (this.waypoint != null) Destroy(this.waypoint);

        // Set a waypoint, player update will move the player to the waypoint
        GameObject waypointTmp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        waypointTmp.name = "PlayerWaypoint";
        waypointTmp.GetComponent<Renderer>().material.color = Color.red;
        waypointTmp.GetComponent<Collider>().enabled = false;
        waypointTmp.transform.position = point;

        this.waypoint = waypointTmp;
    }

    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                print("UI clicked");
                return;
            }

            GameObject mouseOverObject = GameObject.Find("MouseOverObject").GetComponent<MouseOverObjectHandlerScript>().MouseOverObject;

            if (mouseOverObject != null)
            {
                print("Moving to object!");
                SetWaypoint(mouseOverObject.transform.position);
                return;
            }


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100)) // TODO: chack for Enemy, Item, Chest tag
            {

                switch(hit.transform.tag)
                {
                    case "Terrain":
                        MoveTo(hit);
                        break;
                    default:
                        print("Not terrain");
                        break;
                }
            }
            else
                print("Krscen maticek");
        }
    }

    private void CheckMouseScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (cameraOffset.y > 3.0f)
            {
                cameraOffset.y -= 0.5f;
                cameraOffset.z += 0.5f;
            }
                
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (cameraOffset.y < 10.0f)
            {
                cameraOffset.y += 0.5f;
                cameraOffset.z -= 0.5f;
            }
               
        }
    }

    private void MoveTo(RaycastHit hit)
    {
        // Check if we clicked on unwalkable terrain ('blocked')
        NavMeshHit nmHit;

        //bool blocked = NavMesh.Raycast(Camera.main.transform.position, camera.transform.position, out nmHit, NavMesh.AllAreas);

        // Samples to the nearest "Walkable" area postion. 3rd param

        if (SampleWaypointPosition)
        {
            NavMesh.SamplePosition(hit.point, out nmHit, 10, 1 << NavMesh.GetAreaFromName("Walkable"));
            SetWaypoint(nmHit.position);
        }
        else
        {
            // No work :/

            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool blocked = NavMesh.Raycast(this.transform.position, hit.point, out nmHit, NavMesh.AllAreas);
            print(hit.point);
            print(nmHit.distance);
            if (Vector3.Distance(hit.point, nmHit.position) < 1.0f)
            {
                SetWaypoint(nmHit.position);
            }*/

            SetWaypoint(hit.point);
        }



        /*if (blocked)
        {
            print("Blocked!");
            return;
        }*/


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
