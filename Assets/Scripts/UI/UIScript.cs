using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private PlayerScript ActivePlayer;


    private GameObject UIHPBar;
    private GameObject UIHPValueLabel;
    private GameObject UIShieldBar;
    private GameObject UIShieldValueLabel;
    private GameObject UIManaBar;
    private GameObject UIManaValueLabel;
    private GameObject UIFuryBar;
    private GameObject UIFuryValueLabel;

    private GameObject UIXPBar;
    private GameObject UIXPValueLabel;




    // Use this for initialization
    void Start () {
		ActivePlayer = (PlayerScript)GameObject.Find("Player").GetComponent("PlayerScript");
        UIHPBar = GameObject.Find("UIHPBar");
        UIHPValueLabel = GameObject.Find("UIHPValueLabel");
        UIShieldBar = GameObject.Find("UIShieldBar");
        UIShieldValueLabel = GameObject.Find("UIShieldValueLabel");
        UIManaBar = GameObject.Find("UIManaBar");
        UIManaValueLabel = GameObject.Find("UIManaValueLabel");
        UIFuryBar = GameObject.Find("UIFuryBar");
        UIFuryValueLabel = GameObject.Find("UIFuryValueLabel");
        UIXPBar = GameObject.Find("UIXPBar");
        UIXPValueLabel = GameObject.Find("UIXPValueLabel");
    }
	
	// Update is called once per frame
	void Update () {
        UpdateUIBars();
    }


    private void UpdateUIBars()
    {
        if (ActivePlayer.MaxHP > 0)
        {
            float HPScale = ActivePlayer.HP / (float)ActivePlayer.MaxHP;
            if (HPScale > 1) HPScale = 1;
            UIHPBar.transform.localScale = new Vector3(1, HPScale, 1);
        }
        else
        {
            UIHPBar.transform.localScale = new Vector3(1, 0, 1);
        }
        UIHPValueLabel.GetComponent<Text>().text = ActivePlayer.HP + " / " + ActivePlayer.MaxHP;

        if (ActivePlayer.MaxShield > 0)
        {
            float shieldScale = ActivePlayer.Shield / (float)ActivePlayer.MaxShield;
            if (shieldScale > 1) shieldScale = 1;
            UIShieldBar.transform.localScale = new Vector3(1, shieldScale, 1);
        }
        else
        {
            UIShieldBar.transform.localScale = new Vector3(1, 0, 1);
        }
        UIShieldValueLabel.GetComponent<Text>().text = ActivePlayer.Shield + " / " + ActivePlayer.MaxShield;

        if (ActivePlayer.MaxMana > 0)
        {
            float manaScale = ActivePlayer.Mana / (float)ActivePlayer.MaxMana;
            if (manaScale > 1) manaScale = 1;
            UIManaBar.transform.localScale = new Vector3(1, manaScale, 1);
        }
        else
        {
            UIManaBar.transform.localScale = new Vector3(1, 0, 1);
        }
        UIManaValueLabel.GetComponent<Text>().text = ActivePlayer.Mana + " / " + ActivePlayer.MaxMana;

        if (ActivePlayer.MaxFury > 0)
        {
            float furyScale = ActivePlayer.Fury / (float)ActivePlayer.MaxFury;
            if (furyScale > 1) furyScale = 1;
            UIFuryBar.transform.localScale = new Vector3(1, furyScale, 1);
        }
        else
        {
            UIFuryBar.transform.localScale = new Vector3(1, 0, 1);
        }
        UIFuryValueLabel.GetComponent<Text>().text = ActivePlayer.Fury + " / " + ActivePlayer.MaxFury;


        if (ActivePlayer.MaxXp > 0)
        {
            float XPScale = ActivePlayer.Xp / (float)ActivePlayer.MaxXp;
            if (XPScale > 1) XPScale = 1;
            UIXPBar.transform.localScale = new Vector3(XPScale, 1, 1);
        }
        else
        {
            UIXPBar.transform.localScale = new Vector3(0, 1, 1);
        }
        UIXPValueLabel.GetComponent<Text>().text = ActivePlayer.Xp + " / " + ActivePlayer.MaxXp;
    }


    public Text MoneyText;
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
