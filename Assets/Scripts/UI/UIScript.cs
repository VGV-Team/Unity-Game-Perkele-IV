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

    private GameObject UITargetBar;
    private GameObject UITargetValueLabel;

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
        UITargetBar = GameObject.Find("UITargetBar");
        UITargetValueLabel = GameObject.Find("UITargetValueLabel");

        //UIHPBar.GetComponent<Image>().type = Image.Type.Filled;
        //UIHPBar.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
        //UIHPBar.GetComponent<Image>().fillAmount = 0.5f;
        //UIAbility1Bar = GameObject.Find("UIAbility1Bar");
        //UIAbility2Bar = GameObject.Find("UIAbility2Bar");
        //UpdateAbilityList();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateUIBars();
        UpdateAbilityList();
    }

    public void AbilityButtonClick(int id)
    {
        GameObject.Find("Player").GetComponent<UnitScript>().Abilities[id].Use(GameObject.Find("Player"), GameObject.Find("Player").GetComponent<UnitScript>().Target);
    }


    public void UpdateAbilityList()
    {
        if (ActivePlayer.Abilities.Count != abilitiesList.Count)
        {
            abilitiesList = new List<AbilityScript>();
            for (int i = 0; i < GameObject.Find("UIAbilityPanel").transform.childCount; i++)
            {
                if (i < ActivePlayer.Abilities.Count)
                {
                    if (ActivePlayer.Abilities[i].ImageToShow != null)
                    {
                        GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().overrideSprite = ActivePlayer.Abilities[i].ImageToShow;
                    }
                    else
                    {
                        GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().overrideSprite = GameObject.Find("UISpritesDefault").transform.GetComponent<SpriteRenderer>().sprite;
                    }
                    abilitiesList.Add(ActivePlayer.Abilities[i]);
                }
                else
                {
                    GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().overrideSprite = GameObject.Find("UISpritesDisabled").transform.GetComponent<SpriteRenderer>().sprite;
                    GameObject.Find("UIAbilityPanel").transform.GetChild(i).FindChild("UIAbilityBarCooldown").GetComponent<Image>().fillAmount = 0;
                }
            }
        }

        for (int i = 0; i < abilitiesList.Count; i++)
        {
            //GameObject.Find("UIAbilityPanel").transform.GetChild(i).transform.localScale = new Vector3(1, (float)((abilitiesList[i].Cooldown - abilitiesList[i].TimeToReady) / abilitiesList[i].Cooldown), 1);
            GameObject.Find("UIAbilityPanel").transform.GetChild(i).FindChild("UIAbilityBarCooldown").GetComponent<Image>().fillAmount = (float)((abilitiesList[i].TimeToReady) / abilitiesList[i].Cooldown);
        }
    }


    private void UpdateUIBars()
    {
        // Update Hp bar
        float HPScale = ActivePlayer.HP / (float)ActivePlayer.MaxHP;
        if (ActivePlayer.MaxHP > 0)
        {
            if (HPScale > 1) HPScale = 1;
        }
        else
        {
            HPScale = 0;
        }
        UIHPBar.GetComponent<Image>().fillAmount = HPScale;
        UIHPValueLabel.GetComponent<Text>().text = ActivePlayer.HP + " / " + ActivePlayer.MaxHP;

        // Update Shield bar
        float shieldScale = ActivePlayer.Shield / (float)ActivePlayer.MaxShield;
        if (ActivePlayer.MaxShield > 0)
        {
            if (shieldScale > 1) shieldScale = 1;
        }
        else
        {
            shieldScale = 0;
        }
        UIShieldBar.GetComponent<Image>().fillAmount = shieldScale;
        UIShieldValueLabel.GetComponent<Text>().text = ActivePlayer.Shield + " / " + ActivePlayer.MaxShield;

        // Update Mana bar
        float manaScale = ActivePlayer.Mana / (float)ActivePlayer.MaxMana;
        if (ActivePlayer.MaxMana > 0)
        {
            if (manaScale > 1) manaScale = 1;
        }
        else
        {
            manaScale = 0;
        }
        UIManaBar.GetComponent<Image>().fillAmount = manaScale;
        UIManaValueLabel.GetComponent<Text>().text = ActivePlayer.Mana + " / " + ActivePlayer.MaxMana;

        // Update Fury bar
        float furyScale = ActivePlayer.Fury / (float)ActivePlayer.MaxFury;
        if (ActivePlayer.MaxFury > 0)
        {
            if (furyScale > 1) furyScale = 1;
        }
        else
        {
            furyScale = 0;
        }
        UIFuryBar.GetComponent<Image>().fillAmount = furyScale;
        UIFuryValueLabel.GetComponent<Text>().text = ActivePlayer.Fury + " / " + ActivePlayer.MaxFury;

        // Update Xp bar
        float XPScale = ActivePlayer.Xp / (float)ActivePlayer.MaxXp;
        if (ActivePlayer.MaxXp > 0)
        {
            if (XPScale > 1) XPScale = 1;
        }
        else
        {
            XPScale = 0;
        }
        UIXPBar.GetComponent<Image>().fillAmount = XPScale;
        UIXPValueLabel.GetComponent<Text>().text = ActivePlayer.Xp + " / " + ActivePlayer.MaxXp;

        if (ActivePlayer.Target != null)
        {
            float enemyTotalLife = ActivePlayer.Target.GetComponent<UnitScript>().MaxHP + ActivePlayer.Target.GetComponent<UnitScript>().MaxShield;
            float enemyLife = ActivePlayer.Target.GetComponent<UnitScript>().HP + ActivePlayer.Target.GetComponent<UnitScript>().Shield;

            float enemyScale = enemyLife / (float)enemyTotalLife;
            if (ActivePlayer.MaxHP > 0)
            {
                if (enemyScale > 1) enemyScale = 1;
            }
            else
            {
                enemyScale = 0;
            }
            UITargetBar.GetComponent<Image>().fillAmount = enemyScale;
            //UITargetValueLabel.GetComponent<Text>().text = enemyLife + " / " + enemyTotalLife;
            UITargetValueLabel.GetComponent<Text>().text = ActivePlayer.Target.GetComponent<UnitScript>().Name;
        }
        else
        {
            UITargetBar.GetComponent<Image>().fillAmount = 0;
            UITargetValueLabel.GetComponent<Text>().text = "";
        }
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
