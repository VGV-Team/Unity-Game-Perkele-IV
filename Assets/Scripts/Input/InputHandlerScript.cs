using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        CheckKeyboardKeyPressed();
    }

    void CheckKeyboardKeyPressed()
    {
        if (Input.GetKeyDown("1"))
        {
            //Debug.Log("1");
	        GameObject.Find("UI Handler").GetComponent<UIScript>().AbilityButtonClick(0);
			//AbilityUse(1);
        }
        if (Input.GetKeyDown("2"))
        {
            //Debug.Log("2");
			GameObject.Find("UI Handler").GetComponent<UIScript>().AbilityButtonClick(1);
			//AbilityUse(2);
        }
        if (Input.GetKeyDown("3"))
        {
            //Debug.Log("3");
			GameObject.Find("UI Handler").GetComponent<UIScript>().AbilityButtonClick(2);
			//AbilityUse(3);
		}
        if (Input.GetKeyDown("4"))
        {
            //Debug.Log("4");
			GameObject.Find("UI Handler").GetComponent<UIScript>().AbilityButtonClick(3);
			//AbilityUse(4);
        }


		if (Input.GetKeyDown("i"))
		{
			//Debug.Log("i");
			GameObject.Find("UI Handler").GetComponent<UIScript>().ToggleActiveInactive(GameObject.Find("UI Handler").GetComponent<UIScript>().UIInventoryPanel);
		}
		if (Input.GetKeyDown("c"))
		{
			//Debug.Log("c");
			GameObject.Find("UI Handler").GetComponent<UIScript>().ToggleActiveInactive(GameObject.Find("UI Handler").GetComponent<UIScript>().UICharacterPanel);
		}
		if (Input.GetKeyDown("a"))
		{
			//Debug.Log("a");
			GameObject.Find("UI Handler").GetComponent<UIScript>().ToggleActiveInactive(GameObject.Find("UI Handler").GetComponent<UIScript>().UISkillConfigurePanel);
		}


		if (Input.GetKeyDown(KeyCode.Escape))
	    {
		    
			SceneManager.LoadScene("MainMenuScene");
	    }

		/*
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
            GameObject.Find("Player").GetComponent<UnitScript>().Abilities[0].Use(GameObject.Find("Player"), GameObject.Find("Player").GetComponent<UnitScript>().Target);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S");
            GameObject.Find("Player").GetComponent<UnitScript>().Abilities[2].Use(GameObject.Find("Player"));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P");
            //GameObject.Find("Player").GetComponent<PlayerScript>().Abilities = new List<AbilityScript>();
            GameObject.Find("Player").GetComponent<PlayerScript>().Abilities.Add(new AbilityScript("wuuut mate", AbilityType.None, 5,1,1,10, 10));
            //GameObject.Find("UI Handler").GetComponent<UIScript>().UpdateAbilityList();
        }
		*/
    }

	[Obsolete]
	public void AbilityUse(int id)
    {
        if (id >= GameObject.Find("Player").GetComponent<UnitScript>().Abilities.Count) return;
        GameObject.Find("Player").GetComponent<UnitScript>().Abilities[id].Use(GameObject.Find("Player"), GameObject.Find("Player").GetComponent<UnitScript>().Target);
    }

	[Obsolete]
    public void AbilityUse(AbilityScript ability)
    {
        ability.Use(GameObject.Find("Player"), GameObject.Find("Player").GetComponent<UnitScript>().Target);
    }

    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                //print("UI clicked");
                return;
            }
            
            if (HoveredObject != null)
            {
                PlayerWaypointScript.SetWaypoint(HoveredObject);
                //print("Moving to object!");
                //SetWaypoint(mouseOverObject.transform.position);
                //Destination = HoveredObject.transform.position;
                //return;
            }


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layerMask = ~(1 << 2);

            GameObject.Find("Player").GetComponent<PlayerScript>().DisableTriggerCollider();

            if (Physics.Raycast(ray, out hit, 100, layerMask)) // TODO: chack for Enemy, Item, Chest tag
            {
                GameObject.Find("Player").GetComponent<PlayerScript>().EnableTriggerCollider();
                switch (hit.transform.tag)
                {
                    case "Terrain":
                        PlayerWaypointScript.MoveTo(hit);
                        //MoveTo(hit);
                        //Destination = hit.point;
                        //print("Terrain");
                        break;
                    case "Chest":
                        //print("CHEST");
                        // Testing
                        //hit.transform.GetComponent<ChestScript>().OpenChest();
                        break;
                    case "Item":
                        //print("ITEM");
                        //PlayerWaypointScript.PickUpItem(hit.transform);
                        break;
					case "NPC":
						//print("NPC");
		                break;
                    case "Enemy":
                        GameObject.Find("Player").GetComponent<PlayerScript>().basicAttackClick = true;
                        break;
                    default:
                        //print("Not terrain or chest");
                        break;
                }
            }
            else
                print("Krscen maticek");
        }
    }

    private void CheckMouseScroll()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            //print("UI scroll");
            return;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (CameraScript.CameraOffset.y > 3.0f)
            {
                CameraScript.CameraOffset.y -= 2.0f;
                CameraScript.CameraOffset.z += 2.0f;
            }
            /*
            if (CameraScript.Y > 3.0f)
            {
                CameraScript.Y -= 5.0f;
                CameraScript.Z += 5.0f;
            }
            */

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (CameraScript.CameraOffset.y < 10.0f)
            {
                CameraScript.CameraOffset.y += 2.0f;
                CameraScript.CameraOffset.z -= 2.0f;
            }
            /*
            if (CameraScript.Y < 10.0f)
            {
                CameraScript.Y += 5.0f;
                CameraScript.Z -= 5.0f;
            }
            */
        }
    }
}
