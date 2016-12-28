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


    //private GameObject UIAbility1Bar;
    //private GameObject UIAbility2Bar;
    private List<AbilityScript> abilitiesList = new List<AbilityScript>();

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

        //UIAbility1Bar = GameObject.Find("UIAbility1Bar");
        //UIAbility2Bar = GameObject.Find("UIAbility2Bar");
        UpdateAbilityList();
    }
	
	// Update is called once per frame
	void Update () {

        //UIAbility1Bar.transform.localScale = new Vector3(1, (float)((ActivePlayer.Abilities[0].Cooldown-ActivePlayer.Abilities[0].TimeToReady)/ ActivePlayer.Abilities[0].Cooldown), 1);
        //UIAbility2Bar.transform.localScale = new Vector3(1, (float)((ActivePlayer.Abilities[2].Cooldown - ActivePlayer.Abilities[2].TimeToReady)/ActivePlayer.Abilities[2].Cooldown), 1);
	    //AbilityScript ability = ActivePlayer.Abilities[2];
        //GameObject.Find("UIAbilityPanel").transform.GetChild(0).transform.localScale = new Vector3(1, (float)((ability.Cooldown - ability.TimeToReady) / ability.Cooldown), 1);

        /*
        for (int i = 0; i < GameObject.Find("UIAbilityPanel").transform.childCount; i++)
	    {
            GameObject.Find("UIAbilityPanel").transform.GetChild(i).transform.localScale = new Vector3(1, (float)((ActivePlayer.Abilities[i].Cooldown - ActivePlayer.Abilities[i].TimeToReady) / ActivePlayer.Abilities[i].Cooldown), 1);
        }
        */

        UpdateUIBars();

	    for (int i = 0; i < abilitiesList.Count; i++)
	    {
            GameObject.Find("UIAbilityPanel").transform.GetChild(i).transform.localScale = new Vector3(1, (float)((abilitiesList[i].Cooldown - abilitiesList[i].TimeToReady) / abilitiesList[i].Cooldown), 1);
	    }
    }


    public void UpdateAbilityList()
    {
        abilitiesList = new List<AbilityScript>();
        for (int i = 0; i < GameObject.Find("UIAbilityPanel").transform.childCount; i++)
        {
            
            if (i < ActivePlayer.Abilities.Count)
            {
                //GameObject.Find("UIAbilityPanel").transform.GetChild(i).gameObject.SetActive(true);
                if(ActivePlayer.Abilities[i].Type == AbilityType.BasicAttack) GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().color = Color.red;
                if (ActivePlayer.Abilities[i].Type == AbilityType.RangeAttack) GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
                if (ActivePlayer.Abilities[i].Type == AbilityType.Heal) GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().color = Color.blue;
                abilitiesList.Add(ActivePlayer.Abilities[i]);
            }
            else
            {
                GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().color = Color.grey;
                //GameObject.Find("UIAbilityPanel").transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }
    }


    private void UpdateUIBars()
    {
        // Update Hp bar
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

        // Update Shield bar
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

        // Update Mana bar
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

        // Update Fury bar
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

        // Update Xp bar
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



    #region Test

    public void TestPopup()
    {
        
    }

    public void TestAbility1()
    {
        //GameObject.Find("Player").GetComponent<PlayerScript>().Abilities[0].Use(GameObject.Find("Player"), GameObject.Find("Enemy"));
        GameObject.Find("Player").GetComponent<PlayerScript>().Abilities[2].Use(GameObject.Find("Player"));
    }

    public void TestMovement()
    {
        Destroy(GameObject.Find("Enemy"));
        //GameObject.Find("Enemy").GetComponent<UnitScript>().SetWaypoint(GameObject.Find("Player"));
    }

    public Text MoneyText;
    public void SpendMoneyOnClick()
    {
        MoneyText.text = "" + (int.Parse(MoneyText.text) - 10);
    }

    public void GetMoneyOnClick()
    {
        MoneyText.text = "" + (int.Parse(MoneyText.text) + 1);
    }

    #endregion


}
