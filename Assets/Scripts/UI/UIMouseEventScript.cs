using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEnterScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		print("qwe");
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		print("ewq");
	}
}
