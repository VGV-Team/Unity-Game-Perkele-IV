using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
	public Text MoneyText;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	
	public void SpendMoneyOnClick()
	{
		print("qwe");
		MoneyText.text = ""+ (int.Parse(MoneyText.text) - 10);
	}

	public void GetMoneyOnClick()
	{
		MoneyText.text = "" + (int.Parse(MoneyText.text) + 1);
	}

}
