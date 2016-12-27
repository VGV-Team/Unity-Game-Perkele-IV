using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private GameObject ActivePlayer;

	public Text MoneyText;


	// Use this for initialization
	void Start () {
		ActivePlayer = GameObject.Find("Player");
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
