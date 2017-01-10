using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseEventScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public UIScript Script;
	public AbilityScript Ability;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		print(eventData.position);
		Script.ShowAbilityPopup(Ability);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		Script.HideAbilityPopup();
	}
}
