using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    // Active player
    private PlayerScript ActivePlayer;

    // Prefabs
    public GameObject abilityChooseButton;

    // different bars
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

    private GameObject UITargetHPBar;
    private GameObject UITargetShieldBar;
    private GameObject UITargetOtherBar;
    private GameObject UITargetValueLabel;

    //private GameObject UIAbility1Bar;
    //private GameObject UIAbility2Bar;
    //private List<AbilityScript> abilitiesList = new List<AbilityScript>();
    private AbilityScript[] abilitiesList = new AbilityScript[4];

    // Panels
    public GameObject UISkillConfigurePanel;
    public GameObject UIInventoryPanel;

    public int AbilitiesPerRow = 3;
    private int abilitySwapTo = -1;



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
        UITargetHPBar = GameObject.Find("UITargetHPBar");
        UITargetShieldBar = GameObject.Find("UITargetShieldBar");
        UITargetOtherBar = GameObject.Find("UITargetOtherBar");
        UITargetValueLabel = GameObject.Find("UITargetValueLabel");

        #region Commented
        //UIHPBar.GetComponent<Image>().type = Image.Type.Filled;
        //UIHPBar.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
        //UIHPBar.GetComponent<Image>().fillAmount = 0.5f;
        //UIAbility1Bar = GameObject.Find("UIAbility1Bar");
        //UIAbility2Bar = GameObject.Find("UIAbility2Bar");
        //UpdateAbilityList();
        #endregion

        UpdateAbilityOptionsPopup();
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateUIBars();
    }


    public void AbilityButtonClick(int id)
    {
        if (abilitySwapTo >= 0 && id < abilitiesList.Length && abilitySwapTo < ActivePlayer.Abilities.Count)
        {
            abilitiesList[id] = ActivePlayer.Abilities[abilitySwapTo];
            UISkillConfigurePanel.transform.GetChild(abilitySwapTo).GetComponent<Image>().color = Color.white;
            abilitySwapTo = -1;
        }
        else
        {
            if (abilitiesList[id] != null)
                GameObject.Find("InputHandlerObject").GetComponent<InputHandlerScript>().AbilityUse(abilitiesList[id]);
        }
    }


    public void UpdateAbilityOptionsPopup()
    {
        // Remove any previous abilities
        int size = UISkillConfigurePanel.transform.childCount;
        for (int i = 0; i < size; i++)
        {
            Destroy(UISkillConfigurePanel.transform.GetChild(i).gameObject);
        }

        int abilityCount = ActivePlayer.Abilities.Count;
        int numOfCols = (int)Mathf.Ceil((float)abilityCount / AbilitiesPerRow);
        float width = abilityChooseButton.GetComponent<RectTransform>().rect.width;
        float height = abilityChooseButton.GetComponent<RectTransform>().rect.height;

        // Scale the ability selection window
        UISkillConfigurePanel.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(AbilitiesPerRow * (width + 5) + 5, numOfCols * (height + 5) + 5);
        for (int i = 0; i < ActivePlayer.Abilities.Count; i++)
        {
            GameObject goButton = (GameObject)Instantiate(abilityChooseButton);
            goButton.transform.SetParent(UISkillConfigurePanel.transform, false);
            goButton.transform.position = UISkillConfigurePanel.transform.position + new Vector3(
                5 + (width + 5) * (i % AbilitiesPerRow),
                5 + (height + 5) * (i / AbilitiesPerRow),
                0);

            Image imageComponent = goButton.GetComponent<Image>();
            if (ActivePlayer.Abilities[i].ImageToShow != null)
                imageComponent.overrideSprite = ActivePlayer.Abilities[i].ImageToShow;
            else
                imageComponent.overrideSprite = GameObject.Find("UISpritesDefault").transform.GetComponent<SpriteRenderer>().sprite;

            Button tempButton = goButton.GetComponent<Button>();
            int i1 = i;
            tempButton.onClick.AddListener(() => SelectAbilityToChange(i1, goButton));
        }
    }

    /// <summary>
    /// Chooses active player ability id to select to
    /// </summary>
    /// <param name="id">Id of player ability we want to change to</param>
    /// <param name="e">Button pressed for ability change</param>
    private void SelectAbilityToChange(int id, GameObject e = null)
    {
        int size = UISkillConfigurePanel.transform.childCount;
        for (int i = 0; i < size; i++)
        {
            UISkillConfigurePanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        if (e != null) e.GetComponent<Image>().color = Color.cyan;
        abilitySwapTo = id;
    }



    private void UpdateUIBars()
    {
        #region Update main UI bars

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


        // Updating of target bar
        GameObject objectToShow = null;
        if(GameObject.Find("InputHandlerObject").GetComponent<InputHandlerScript>().HoveredObject != null) // Hovered object update
        {
            objectToShow = GameObject.Find("InputHandlerObject").GetComponent<InputHandlerScript>().HoveredObject;
        }
        else if (ActivePlayer.Target != null) // Active target update
        {
            objectToShow = ActivePlayer.Target;
        }

        if (objectToShow != null)
        {

            switch (objectToShow.tag)
            {
                case "Enemy":
                    UpdateEnemyUI(objectToShow);
                    break;
                case "Chest":
                    UpdateChestUI(objectToShow);
                    break;
                case "Item":
                    //ITEM
                    break;
                default:
                    break;
            }

            
        }
        else
        {
            UITargetHPBar.GetComponent<Image>().fillAmount = 0;
            UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
            UITargetOtherBar.GetComponent<Image>().fillAmount = 0;
            UITargetValueLabel.GetComponent<Text>().text = "";
        }

        #endregion

        #region Update abilities bars
        
        // Update abilities bar
        for (int i = 0; i < abilitiesList.Length; i++)
        {
            if (abilitiesList[i] == null)
            {
                GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().overrideSprite = GameObject.Find("UISpritesDisabled").transform.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                //GameObject.Find("UIAbilityPanel").transform.GetChild(i).transform.localScale = new Vector3(1, (float)((abilitiesList[i].Cooldown - abilitiesList[i].TimeToReady) / abilitiesList[i].Cooldown), 1);
                GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().overrideSprite = abilitiesList[i].ImageToShow;
                GameObject.Find("UIAbilityPanel").transform.GetChild(i).FindChild("UIAbilityBarCooldown").GetComponent<Image>().fillAmount = (float)((abilitiesList[i].TimeToReady) / abilitiesList[i].Cooldown);
            }

        }

        #endregion
    }

    private void UpdateEnemyUI(GameObject objectToShow)
    {
        // Update Hp bar
        float targetHPScale = objectToShow.GetComponent<UnitScript>().HP / (float)objectToShow.GetComponent<UnitScript>().MaxHP;
        if (objectToShow.GetComponent<UnitScript>().MaxHP > 0)
        {
            if (targetHPScale > 1) targetHPScale = 1;
        }
        else
        {
            targetHPScale = 0;
        }
        UITargetHPBar.GetComponent<Image>().fillAmount = targetHPScale;

        // Update Shield bar
        float targetShieldScale = objectToShow.GetComponent<UnitScript>().Shield / (float)objectToShow.GetComponent<UnitScript>().MaxShield;
        if (objectToShow.GetComponent<UnitScript>().MaxShield > 0)
        {
            if (targetShieldScale > 1) targetShieldScale = 1;
        }
        else
        {
            targetShieldScale = 0;
        }
        UITargetShieldBar.GetComponent<Image>().fillAmount = targetShieldScale;

        UITargetOtherBar.GetComponent<Image>().fillAmount = 0;
        UITargetValueLabel.GetComponent<Text>().text = objectToShow.GetComponent<UnitScript>().Name;
    }

    private void UpdateChestUI(GameObject objectToShow)
    {
        // TODO
        UITargetHPBar.GetComponent<Image>().fillAmount = 0;
        UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
        UITargetOtherBar.GetComponent<Image>().fillAmount = 1;

        UITargetValueLabel.GetComponent<Text>().text = objectToShow.GetComponent<ChestScript>().Name;

    }


    /// <summary>
    /// Called by button to toggle objectToToggle state between active and inactive
    /// </summary>
    /// <param name="objectToToggle">GameObject to toggle</param>
    public void ToggleActiveInactive(GameObject objectToToggle)
    {
        // if we are showing ability list window then update list first
        if (objectToToggle.name == "UISkillConfigurePanel")
        {
            if (objectToToggle.activeInHierarchy == false)
            {
                UpdateAbilityOptionsPopup();
            }
        }
        if (objectToToggle.name == "UIInventoryPanel")
        {
            if (objectToToggle.activeInHierarchy == false)
            {
                // TODO: Update inventory items
            }
        }


        objectToToggle.SetActive(!objectToToggle.activeInHierarchy);
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

#region Old

/*
// Old ability updating
public void UpdateAbilityBars()
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

}
*/

#endregion