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

    void OnMouseOver()
    {
        InputManagerObject.GetComponent<InputHandlerScript>().HoveredObject = this.gameObject;
    }

    void OnMouseExit()
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
