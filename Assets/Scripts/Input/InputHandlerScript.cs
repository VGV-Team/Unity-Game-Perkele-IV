using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandlerScript : MonoBehaviour
{

    // manage selected item
    // manage hovered object
    // manage clicks

    public GameObject HoveredObject;
    public GameObject ClickedObject;

    public MainCameraScript CameraScript;
    public PlayerScript PlayerWaypointScript;

    // Use this for initialization
    void Start ()
    {
        CameraScript = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
        PlayerWaypointScript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }
	
    // Update is called once per frame
    void Update ()
    {
        CheckMouseClick();
        CheckMouseScroll();
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
            
            if (HoveredObject != null)
            {
                PlayerWaypointScript.SetWaypoint(HoveredObject);
                print("Moving to object!");
                //SetWaypoint(mouseOverObject.transform.position);
                //Destination = HoveredObject.transform.position;
                return;
            }


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100)) // TODO: chack for Enemy, Item, Chest tag
            {

                switch (hit.transform.tag)
                {
                    case "Terrain":
                        PlayerWaypointScript.MoveTo(hit);
                        //MoveTo(hit);
                        //Destination = hit.point;
                        print("Terrain");
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
            
            if (CameraScript.Y > 3.0f)
            {
                CameraScript.Y -= 5.0f;
                CameraScript.Z += 5.0f;
            }

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (CameraScript.Y < 10.0f)
            {
                CameraScript.Y += 5.0f;
                CameraScript.Z -= 5.0f;
            }
        }
    }
}
