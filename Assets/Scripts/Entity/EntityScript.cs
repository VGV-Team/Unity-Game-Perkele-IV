using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityScript : MonoBehaviour
{
    public string Name;
    public bool Active;

    private GameObject InputManagerObject;

    // Tells us if player is touching the object
    // Object needs 2 colliders - 1 normal and 1 trigger collider which should be roughly more than 1.5x bigger than the normal one
    // Example: Item pickups
    [HideInInspector]
    public bool PlayerTouching = false;

    public void Start()
    {
        InputManagerObject = GameObject.Find("InputHandlerObject");
    }

    //private Color tmpColor;
    protected void OnMouseEnter()
    {
        SetThisHoverObject();
        //tmpColor = this.GetComponent<Renderer>().material.color;
        //this.GetComponent<Renderer>().material.color = Color.red;
    }

    protected void OnMouseExit()
    {
        //this.GetComponent<Renderer>().material.color = tmpColor;
        UnsetThisHoverObject();
        
    }

    private void SetThisHoverObject()
    {
        if (this.gameObject == null) Debug.Log("WQWE");
        InputManagerObject.GetComponent<InputHandlerScript>().HoveredObject = this.gameObject;
    }

    private void UnsetThisHoverObject()
    {
        InputManagerObject.GetComponent<InputHandlerScript>().HoveredObject = null;
    }

    /*
    void OnMouseDown()
    {
        InputManagerObject.GetComponent<InputHandlerScript>().ClickedObject = this.gameObject;
    }
    */

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlayerTouching = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlayerTouching = false;
        }
    }
}
