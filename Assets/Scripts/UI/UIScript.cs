using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
	#region Public UI objects

	// Active player
	private PlayerScript ActivePlayer;

    // Prefabs
    public GameObject abilityChooseButton;
    public GameObject inventoryItemButton;
	public GameObject questListItemRow;

    // bars
    public GameObject UIHPBar;
    public GameObject UIHPValueLabel;
    public GameObject UIShieldBar;
    public GameObject UIShieldValueLabel;
    public GameObject UIManaBar;
    public GameObject UIManaValueLabel;
    public GameObject UIFuryBar;
    public GameObject UIFuryValueLabel;
    public GameObject UIXPBar;
    public GameObject UIXPValueLabel;
    public GameObject UITargetHPBar;
    public GameObject UITargetShieldBar;
    public GameObject UITargetOtherBar;
    public GameObject UITargetValueLabel;
    public GameObject UIInventorySelectedItemNameLabel;
    public GameObject UIInventorySelectedItemTypeLabel;
    public GameObject UIInventorySelectedItemDamageLabel;
    public GameObject UIInventorySelectedItemAttackSpeedLabel;
    public GameObject UIInventorySelectedItemCriticalChanceLabel;
    public GameObject UIInventorySelectedItemCriticalDamageLabel;
	public GameObject UIInventorySelectedItemArmorLabel;
	public GameObject UIInventorySelectedItemRarityLabel;
    public GameObject UIInventorySelectedItemScrapValueLabel;
	public GameObject UIInventorySelectedItemDiscoveryLabel;
	public GameObject UIInventoryScrapLabel;


    // character stats
    public GameObject UICharacterStatsMaxHPLabel;
    public GameObject UICharacterStatsHPLabel;
    public GameObject UICharacterStatsHPChangeLabel;
    public GameObject UICharacterStatsMaxShieldLabel;
    public GameObject UICharacterStatsShieldLabel;
    public GameObject UICharacterStatsShieldChangeLabel;
    public GameObject UICharacterStatsMaxFuryLabel;
    public GameObject UICharacterStatsFuryLabel;
    public GameObject UICharacterStatsFuryChangeLabel;
    public GameObject UICharacterStatsMaxManaLabel;
    public GameObject UICharacterStatsManaLabel;
    public GameObject UICharacterStatsManaChangeLabel;
    public GameObject UICharacterStatsArmorLabel;
    public GameObject UICharacterStatsStrengthLabel;
    public GameObject UICharacterStatsAttackSpeedLabel;
	public GameObject UICharacterStatsCriticalChanceLabel;
	public GameObject UICharacterStatsDiscoveryLabel;
	public GameObject UICharacterStatsMovementSpeedLabel;
	public GameObject UICharacterStatsLevelLabel;
	public GameObject UICharacterStatsXPLabel;
	public GameObject UICharacterStatsMaxXPLabel;
	public GameObject UICharacterStatsScrapLabel;
	public GameObject UICharacterStatsGoldLabel;

	// Buttons
	public GameObject UIInventoryUnequipButton;
    public GameObject UIInventoryEquipButton;
    public GameObject UIInventoryDestroyButton;

	// Panels
	public GameObject UISkillConfigurePanel;
	public GameObject UIInventoryPanel;
	public GameObject UIInventoryItemsContent;
	public GameObject UIInventorySelectedItemPanel;
	public GameObject UIInventoryCenterItemCameraPanel;
	public GameObject UICharacterPanel;
	public GameObject UINPCConversationPanel;
	public GameObject UIQuestListPanel;

	// Equipped items
	public GameObject UIInventoryEquippedItemsWeaponButton;
	public GameObject UIInventoryEquippedItemsShieldButton;
	public GameObject UIInventoryEquippedItemsAmuletButton;

	public GameObject UISpritesDefault;
	public GameObject UISpritesDisabled;
	public GameObject UISpritesCheckboxChecked;
	public GameObject UISpritesCheckboxUnchecked;
	public GameObject UISpritesAbilityUpgrade;

	#endregion

	public int AbilitiesPerRow = 3;
	private AbilityScript[] abilitiesList = new AbilityScript[4];
    //private int selectedAbility = -1;
    private AbilityScript selectedAbility = null;
    private GameObject selectedItem = null;
    //private List<GameObject> selectedItemList = null;

	


	// Use this for initialization
	void Start () {
		ActivePlayer = (PlayerScript)GameObject.Find("Player").GetComponent("PlayerScript");

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
        UpdateUI();
    }


    private void UpdateUI()
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
        UIHPValueLabel.GetComponent<Text>().text = ActivePlayer.HP.ToString("F0") + " / " + ActivePlayer.MaxHP.ToString("F0");

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
        UIShieldValueLabel.GetComponent<Text>().text = ActivePlayer.Shield.ToString("F0") + " / " + ActivePlayer.MaxShield.ToString("F0");

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
        UIManaValueLabel.GetComponent<Text>().text = ActivePlayer.Mana.ToString("F0") + " / " + ActivePlayer.MaxMana.ToString("F0");

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
        UIFuryValueLabel.GetComponent<Text>().text = ActivePlayer.Fury.ToString("F0") + " / " + ActivePlayer.MaxFury.ToString("F0");

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
        UIXPValueLabel.GetComponent<Text>().text = ActivePlayer.Xp.ToString("F0") + " / " + ActivePlayer.MaxXp.ToString("F0");


        // Updating of target bar
        GameObject objectToShow = null;
        if (GameObject.Find("InputHandlerObject").GetComponent<InputHandlerScript>().HoveredObject != null) // Hovered object update
        {
            objectToShow = GameObject.Find("InputHandlerObject").GetComponent<InputHandlerScript>().HoveredObject;
        }
        else if (ActivePlayer.Target != null) // Active target update
        {
            objectToShow = ActivePlayer.Target;
        }

        UITargetHPBar.GetComponent<Image>().fillAmount = 0;
        UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
        UITargetOtherBar.GetComponent<Image>().fillAmount = 0;
	    //UITargetOtherBar.GetComponent<Image>().color = Color.clear;

		UITargetValueLabel.GetComponent<Text>().text = "";

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
                    UpdateItemUI(objectToShow);
                    break;
				case "NPC":
		            UpdateNPCUI(objectToShow);
		            break;
                default:
                    break;
            }
        }

        #endregion

        #region Update abilities bars

        // Update abilities bar
        for (int i = 0; i < abilitiesList.Length; i++)
        {
            if (abilitiesList[i] == null)
            {
                GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().overrideSprite = UISpritesDisabled.transform.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                //GameObject.Find("UIAbilityPanel").transform.GetChild(i).transform.localScale = new Vector3(1, (float)((abilitiesList[i].Cooldown - abilitiesList[i].TimeToReady) / abilitiesList[i].Cooldown), 1);
                GameObject.Find("UIAbilityPanel").transform.GetChild(i).GetComponent<Image>().overrideSprite = abilitiesList[i].ImageToShow;
                GameObject.Find("UIAbilityPanel").transform.GetChild(i).FindChild("UIAbilityBarCooldown").GetComponent<Image>().fillAmount = (float)((abilitiesList[i].TimeToReady) / abilitiesList[i].Cooldown);
            }

        }

        #endregion

        #region Update character stats

        UICharacterStatsMaxHPLabel.GetComponent<Text>().text = ActivePlayer.MaxHP.ToString("F0");
        UICharacterStatsHPLabel.GetComponent<Text>().text = ActivePlayer.HP.ToString("F0");
        UICharacterStatsHPChangeLabel.GetComponent<Text>().text = ActivePlayer.HPChange.ToString("F0");
        UICharacterStatsMaxShieldLabel.GetComponent<Text>().text = ActivePlayer.MaxShield.ToString("F0");
        UICharacterStatsShieldLabel.GetComponent<Text>().text = ActivePlayer.Shield.ToString("F0");
        UICharacterStatsShieldChangeLabel.GetComponent<Text>().text = ActivePlayer.ShieldChange.ToString("F0");
        UICharacterStatsMaxFuryLabel.GetComponent<Text>().text = ActivePlayer.MaxFury.ToString("F0");
        UICharacterStatsFuryLabel.GetComponent<Text>().text = ActivePlayer.Fury.ToString("F0");
        UICharacterStatsFuryChangeLabel.GetComponent<Text>().text = ActivePlayer.FuryChange.ToString("F0");
        UICharacterStatsMaxManaLabel.GetComponent<Text>().text = ActivePlayer.MaxMana.ToString("F0");
        UICharacterStatsManaLabel.GetComponent<Text>().text = ActivePlayer.Mana.ToString("F0");
        UICharacterStatsManaChangeLabel.GetComponent<Text>().text = ActivePlayer.ManaChange.ToString("F0");
        UICharacterStatsArmorLabel.GetComponent<Text>().text = ActivePlayer.Armor.ToString("F0");
        UICharacterStatsStrengthLabel.GetComponent<Text>().text = ActivePlayer.Strength.ToString("F0");
        UICharacterStatsAttackSpeedLabel.GetComponent<Text>().text = ActivePlayer.AttackSpeed.ToString("F0");
		UICharacterStatsCriticalChanceLabel.GetComponent<Text>().text = ActivePlayer.CriticalChance.ToString("F0");
		UICharacterStatsDiscoveryLabel.GetComponent<Text>().text = ActivePlayer.Discovery.ToString("F0");
		UICharacterStatsMovementSpeedLabel.GetComponent<Text>().text = ActivePlayer.MovementSpeed.ToString("F0");
		UICharacterStatsLevelLabel.GetComponent<Text>().text = ActivePlayer.Level.ToString("F0");
		UICharacterStatsXPLabel.GetComponent<Text>().text = ActivePlayer.Xp.ToString("F0");
		UICharacterStatsMaxXPLabel.GetComponent<Text>().text = ActivePlayer.MaxXp.ToString("F0");
		UICharacterStatsScrapLabel.GetComponent<Text>().text = ActivePlayer.Scrap.ToString("F0");
		UICharacterStatsGoldLabel.GetComponent<Text>().text = ActivePlayer.Gold.ToString("F0");

	    if (ActivePlayer.AbilityPoints > 0)
	    {
		    foreach (var item in GameObject.FindGameObjectsWithTag("StatUpgrade"))
		    {
			    item.transform.FindChild("UICharacterStatsUpgradeButton").gameObject.SetActive(true);
		    }
	    }
	    else
	    {
			foreach (var item in GameObject.FindGameObjectsWithTag("StatUpgrade"))
			{
				item.transform.FindChild("UICharacterStatsUpgradeButton").gameObject.SetActive(false);
			}
		}



		#endregion

		#region Update quest panel

		int size = UIQuestListPanel.transform.childCount;
		for (int i = 0; i < size; i++)
		{
			Destroy(UIQuestListPanel.transform.GetChild(i).gameObject);
		}
		foreach (var quest in ActivePlayer.QuestList.FindAll(q => q.Received && !q.Completed).ToList())
		{
			GameObject questRow = (GameObject)Instantiate(questListItemRow);
			questRow.transform.SetParent(UIQuestListPanel.transform, false);
			questRow.transform.FindChild("Image").GetComponent<Image>().overrideSprite = UISpritesCheckboxUnchecked.transform.GetComponent<SpriteRenderer>().sprite; // active quest image
			questRow.transform.FindChild("Text").GetComponent<Text>().text = quest.Title;
		}
		foreach (var quest in ActivePlayer.QuestList.FindAll(q => q.Received && q.Completed).ToList())
		{
			GameObject questRow = (GameObject)Instantiate(questListItemRow);
			questRow.transform.SetParent(UIQuestListPanel.transform, false);
			questRow.transform.FindChild("Image").GetComponent<Image>().overrideSprite = UISpritesCheckboxChecked.transform.GetComponent<SpriteRenderer>().sprite; // finished quest image
			questRow.transform.FindChild("Text").GetComponent<Text>().text = quest.Title;
		}

		#endregion
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
                selectedAbility = null;
                UpdateAbilityOptionsPopup();
            }

			UIInventoryPanel.SetActive(false);
			UICharacterPanel.SetActive(false);
		}
        if (objectToToggle.name == "UIInventoryPanel")
        {
            if (objectToToggle.activeInHierarchy == false)
            {
                UpdateInventoryList();
            }

			UICharacterPanel.SetActive(false);
			UISkillConfigurePanel.SetActive(false);
		}
		if (objectToToggle.name == "UICharacterPanel")
		{
			if (objectToToggle.activeInHierarchy == false)
			{
				//UpdateInventoryList();
			}

			UIInventoryPanel.SetActive(false);
			UISkillConfigurePanel.SetActive(false);
		}

		
		objectToToggle.SetActive(!objectToToggle.activeInHierarchy);
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
        UITargetHPBar.GetComponent<Image>().fillAmount = 0;
        UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
        UITargetOtherBar.GetComponent<Image>().fillAmount = 1;
		UITargetOtherBar.GetComponent<Image>().color = Color.yellow;


		string nameToShow = objectToShow.GetComponent<ChestScript>().Name;
        if (objectToShow.GetComponent<ChestScript>().Opened)
        {
            nameToShow += " (Open)";
        }
        else
        {
            nameToShow += " (" + objectToShow.GetComponent<ChestScript>().ScrapRequired + " Scrap)";
        }
        UITargetValueLabel.GetComponent<Text>().text = nameToShow;
    }

    private void UpdateItemUI(GameObject objectToShow)
    {
        UITargetHPBar.GetComponent<Image>().fillAmount = 0;
        UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
        UITargetOtherBar.GetComponent<Image>().fillAmount = 1;
		UITargetOtherBar.GetComponent<Image>().color = GlobalsScript.RarityToColor(objectToShow.GetComponent<ItemScript>().Rarity);
		UITargetValueLabel.GetComponent<Text>().text = objectToShow.GetComponent<ItemScript>().Name;
    }

	private void UpdateNPCUI(GameObject objectToShow)
	{
		UITargetHPBar.GetComponent<Image>().fillAmount = 0;
		UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
		UITargetOtherBar.GetComponent<Image>().fillAmount = 1;
		UITargetOtherBar.GetComponent<Image>().color = Color.green;
		UITargetValueLabel.GetComponent<Text>().text = objectToShow.GetComponent<NPCScript>().Name;
	}

	private void OnMouseEnter()
    {
        Debug.Log("qweqweqwe");
    }

    #region Abilities

    public void AbilityButtonClick(int id)
    {
        if (selectedAbility != null && id < abilitiesList.Length)
        {
            abilitiesList[id] = selectedAbility;
            for (int i = 0; i < UISkillConfigurePanel.transform.childCount; i++)
            {
                UISkillConfigurePanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            selectedAbility = null;
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
	        GameObject child = UISkillConfigurePanel.transform.GetChild(i).gameObject;
			if(child.name != "FancyImage") Destroy(child);
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
			goButton.transform.localPosition = new Vector3(
				5 + (width + 5) * (i % AbilitiesPerRow),
                5 + (height + 5) * (i / AbilitiesPerRow),
                0);

            Image imageComponent = goButton.GetComponent<Image>();
            if (ActivePlayer.Abilities[i].ImageToShow != null)
                imageComponent.overrideSprite = ActivePlayer.Abilities[i].ImageToShow;
            else
                imageComponent.overrideSprite = UISpritesDefault.transform.GetComponent<SpriteRenderer>().sprite;

            Button tempButton = goButton.GetComponent<Button>();
            int i1 = i;
            tempButton.onClick.AddListener(() => SelectAbilityToChange(ActivePlayer.Abilities[i1], goButton));
        }
    }


    /// <summary>
    /// Chooses active player ability ability to select to
    /// </summary>
    /// <param name="ability">Id of player ability we want to change to</param>
    /// <param name="e">Button pressed for ability change</param>
    private void SelectAbilityToChange(AbilityScript ability, GameObject e = null)
    {
        int size = UISkillConfigurePanel.transform.childCount;
        for (int i = 0; i < size; i++)
        {
            UISkillConfigurePanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        if (e != null) e.GetComponent<Image>().color = Color.cyan;
        selectedAbility = ability;
    }

    #endregion

    #region Inventory

	/// <summary>
	/// Updates item lists and clears currently selected item
	/// </summary>
    public void UpdateInventoryList()
    {
        int size = UIInventoryItemsContent.transform.childCount;
        for (int i = 0; i < size; i++)
        {
            Destroy(UIInventoryItemsContent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < ActivePlayer.InventoryItemsList.Count; i++)
        {
            GameObject goButton = (GameObject)Instantiate(inventoryItemButton);
            goButton.transform.SetParent(UIInventoryItemsContent.transform, false);
	        goButton.transform.FindChild("Image").GetComponent<Image>().overrideSprite = ActivePlayer.InventoryItemsList[i].GetComponent<ItemScript>().ImageToShow;

			//goButton.transform.GetComponent<Image>().color = GlobalsScript.RarityToColor(ActivePlayer.InventoryItemsList[i].GetComponent<ItemScript>().Rarity);
            goButton.transform.FindChild("Text").GetComponent<Text>().text = ActivePlayer.InventoryItemsList[i].GetComponent<ItemScript>().Name;
            Button tempButton = goButton.GetComponent<Button>();
            int i1 = i;
            tempButton.onClick.AddListener(() => SelectItemToShow(ActivePlayer.InventoryItemsList[i1], false, goButton));
        }

		#region Equipped items

		if (ActivePlayer.EquippedItems.WeaponSlot != null)
		{
			UIInventoryEquippedItemsWeaponButton.GetComponent<Image>().overrideSprite = ActivePlayer.EquippedItems.WeaponSlot.GetComponent<ItemScript>().ImageToShow;
			UIInventoryEquippedItemsWeaponButton.GetComponent<Button>().onClick.RemoveAllListeners();
			UIInventoryEquippedItemsWeaponButton.GetComponent<Button>().onClick.AddListener(() => SelectItemToShow(ActivePlayer.EquippedItems.WeaponSlot, true, UIInventoryEquippedItemsWeaponButton));
			UIInventoryEquippedItemsWeaponButton.SetActive(true);
		}
		else
		{
			UIInventoryEquippedItemsWeaponButton.GetComponent<Button>().onClick.RemoveAllListeners();
			UIInventoryEquippedItemsWeaponButton.GetComponent<Image>().overrideSprite = UISpritesDefault.transform.GetComponent<SpriteRenderer>().sprite;
			UIInventoryEquippedItemsWeaponButton.SetActive(false);
		}
		if (ActivePlayer.EquippedItems.ShieldSlot != null)
		{
			UIInventoryEquippedItemsShieldButton.GetComponent<Image>().overrideSprite = ActivePlayer.EquippedItems.ShieldSlot.GetComponent<ItemScript>().ImageToShow;
			UIInventoryEquippedItemsShieldButton.GetComponent<Button>().onClick.RemoveAllListeners();
			UIInventoryEquippedItemsShieldButton.GetComponent<Button>().onClick.AddListener(() => SelectItemToShow(ActivePlayer.EquippedItems.ShieldSlot, true, UIInventoryEquippedItemsWeaponButton));
			UIInventoryEquippedItemsShieldButton.SetActive(true);
		}
		else
		{
			UIInventoryEquippedItemsShieldButton.GetComponent<Button>().onClick.RemoveAllListeners();
			UIInventoryEquippedItemsShieldButton.GetComponent<Image>().overrideSprite = UISpritesDefault.transform.GetComponent<SpriteRenderer>().sprite;
			UIInventoryEquippedItemsShieldButton.SetActive(false);
		}
		if (ActivePlayer.EquippedItems.AmuletSlot != null)
		{
			UIInventoryEquippedItemsAmuletButton.GetComponent<Image>().overrideSprite = ActivePlayer.EquippedItems.AmuletSlot.GetComponent<ItemScript>().ImageToShow;
			UIInventoryEquippedItemsAmuletButton.GetComponent<Button>().onClick.RemoveAllListeners();
			UIInventoryEquippedItemsAmuletButton.GetComponent<Button>().onClick.AddListener(() => SelectItemToShow(ActivePlayer.EquippedItems.AmuletSlot, true, UIInventoryEquippedItemsWeaponButton));
			UIInventoryEquippedItemsAmuletButton.SetActive(true);
		}
		else
		{
			UIInventoryEquippedItemsAmuletButton.GetComponent<Button>().onClick.RemoveAllListeners();
			UIInventoryEquippedItemsAmuletButton.GetComponent<Image>().overrideSprite = UISpritesDefault.transform.GetComponent<SpriteRenderer>().sprite;
			UIInventoryEquippedItemsAmuletButton.SetActive(false);
		}

		#endregion

		UIInventoryScrapLabel.GetComponent<Text>().text = ActivePlayer.Scrap.ToString("F0");
	    selectedItem = null;
	    UpdateUIInventorySelectedItemLabels(null, null);
    }

    private void SelectItemToShow(GameObject item, bool isEquippedItem, GameObject e = null)
    {
		
        for (int i = 0; i < UIInventoryItemsContent.transform.childCount; i++)
        {
            UIInventoryItemsContent.transform.GetChild(i).transform.FindChild("Text").GetComponent<Text>().fontSize = 14;
            UIInventoryItemsContent.transform.GetChild(i).transform.FindChild("Text").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        if (e != null)
        {
            e.transform.FindChild("Text").GetComponent<Text>().fontSize = 16;
            e.transform.FindChild("Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
        }

        UpdateUIInventorySelectedItemLabels(item, isEquippedItem);
		
		selectedItem = item;
    }

    private void UpdateUIInventorySelectedItemLabels(GameObject item, bool? isEquippedItem)
    {
        if (item != null)
        {
            UIInventorySelectedItemNameLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Name;
            UIInventorySelectedItemTypeLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Type.ToString();
			UIInventorySelectedItemDamageLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Damage.ToString("F0");
            UIInventorySelectedItemAttackSpeedLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().AttackSpeed.ToString("F0");
            UIInventorySelectedItemCriticalChanceLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().CriticalChance.ToString("F0");
            UIInventorySelectedItemCriticalDamageLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().CriticalDamage.ToString("F0");
			UIInventorySelectedItemArmorLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Armor.ToString("F0");
			UIInventorySelectedItemRarityLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Rarity.ToString();
            UIInventorySelectedItemScrapValueLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().ScrapValue.ToString("F0");
			UIInventorySelectedItemDiscoveryLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Discovery.ToString("F0");

			UIInventorySelectedItemPanel.SetActive(true);
			UIInventoryCenterItemCameraPanel.SetActive(false);
			if (isEquippedItem == true)
			{
				UIInventoryUnequipButton.SetActive(true);
				UIInventoryEquipButton.SetActive(false);
				UIInventoryDestroyButton.SetActive(false);
			}
			else if(isEquippedItem == false)
			{
				UIInventoryUnequipButton.SetActive(false);
				UIInventoryEquipButton.SetActive(true);
				UIInventoryDestroyButton.SetActive(true);
			}
		}
        else
        {
            UIInventorySelectedItemPanel.SetActive(false);
			UIInventoryCenterItemCameraPanel.SetActive(true);

			UIInventoryUnequipButton.SetActive(false);
			UIInventoryEquipButton.SetActive(false);
			UIInventoryDestroyButton.SetActive(false);
		}
	}

    public void UIInventoryEquipButtonClick()
    {
        if (selectedItem == null) return;
		ActivePlayer.EquipItem(selectedItem);
        UpdateInventoryList();
    }

    public void UIInventoryUnquipButtonClick()
    {
        if (selectedItem == null) return;
		ActivePlayer.UnequipItem(selectedItem);
        UpdateInventoryList();
    }

	public void UIInventoryDestroyButtonClick()
    {
        if (selectedItem == null) return;
		ActivePlayer.DestroyItem(selectedItem);
        UpdateInventoryList();
    }

    #endregion


	public void StatsUpgrade(string statName)
	{
		ActivePlayer.StatsUpgrade(statName);
	}

	public void QuitGameButton()
	{
		SceneManager.LoadScene("MainMenuScene");
	}
}

#region Old

/*

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