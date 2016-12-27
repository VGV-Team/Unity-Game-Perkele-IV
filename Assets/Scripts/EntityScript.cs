using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityScript : MonoBehaviour
{
    public string Name;

    private GameObject MouseOverObject;

    private void Start()
    {
        MouseOverObject = GameObject.Find("MouseOverObject");
    }

    void OnMouseOver()
    {
        MouseOverObject.GetComponent<MouseOverObjectHandlerScript>().MouseOverObject = this.gameObject;
    }

    private void OnMouseExit()
    {
        MouseOverObject.GetComponent<MouseOverObjectHandlerScript>().MouseOverObject = null;
    }
}
