﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public GameObject MainCameraObject;
    public Transform CenterCameraObject;
    public Vector3 CameraOffset;

    public float X {
        get { return gameObject.transform.position.x; }
        set {
            MainCameraObject.transform.position = new Vector3(
            value,
            gameObject.transform.position.y,
            gameObject.transform.position.z); }
    }
    public float Y
    {
        get { return gameObject.transform.position.y; }
        set
        {
            MainCameraObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                value,
                gameObject.transform.position.z);
        }
    }
    public float Z
    {
        get { return gameObject.transform.position.z; }
        set
        {
            MainCameraObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                value);
        }
    }

    // Use this for initialization
    void Start ()
    {
        MainCameraObject = GameObject.Find("Main Camera");
        CenterCameraObject = GameObject.Find("Player").transform;
        CameraOffset = new Vector3(-0.0f, 8.0f, -8.0f);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (CenterCameraObject != null)
	    {
            X = CenterCameraObject.transform.position.x + CameraOffset.x;
	        Y = CenterCameraObject.transform.position.y + CameraOffset.y;
	        Z = CenterCameraObject.transform.position.z + CameraOffset.z;
	    }
    }
}