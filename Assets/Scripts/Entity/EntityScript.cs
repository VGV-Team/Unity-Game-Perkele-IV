using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityScript : MonoBehaviour
{
    public string Name;

    private GameObject InputManagerObject;

    public void Start()
    {
        InputManagerObject = GameObject.Find("InputHandlerObject");
    }

    private void OnMouseOver()
    {
        SetThisHoverObject();
    }

    private void OnMouseExit()
    {
        UnsetThisHoverObject();
    }

    private void SetThisHoverObject()
    {
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
}
