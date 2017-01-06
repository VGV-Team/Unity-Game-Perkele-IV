using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
    public GameObject inventoryItemButton;

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
    public GameObject UIInventorySelectedItemRarityLabel;
    public GameObject UIInventorySelectedItemScrapValueLabel;
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
    public GameObject UICharacterStatsDiscoveryLabel;

    // Buttons
    public GameObject UIInventoryUnequipButton;
    public GameObject UIInventoryEquipButton;
    public GameObject UIInventoryDestroyButton;

    //private GameObject UIAbility1Bar;
    //private GameObject UIAbility2Bar;
    //private List<AbilityScript> abilitiesList = new List<AbilityScript>();
    private AbilityScript[] abilitiesList = new AbilityScript[4];

    // Panels
    public GameObject UISkillConfigurePanel;
    public GameObject UIInventoryPanel;
    public GameObject UIInventoryItemsContent;
    public GameObject UIInventoryEquippedItemsContent;
    public GameObject UIInventorySelectedItemPanel;

    public int AbilitiesPerRow = 3;
    //private int selectedAbility = -1;
    private AbilityScript selectedAbility = null;
    private GameObject selectedItem = null;
    private List<GameObject> selectedItemList = null;


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

        #region Update character stats

        UICharacterStatsMaxHPLabel.GetComponent<Text>().text = ActivePlayer.MaxHP.ToString();
        UICharacterStatsHPLabel.GetComponent<Text>().text = ActivePlayer.HP.ToString();
        UICharacterStatsHPChangeLabel.GetComponent<Text>().text = ActivePlayer.HPChange.ToString();
        UICharacterStatsMaxShieldLabel.GetComponent<Text>().text = ActivePlayer.MaxShield.ToString();
        UICharacterStatsShieldLabel.GetComponent<Text>().text = ActivePlayer.Shield.ToString();
        UICharacterStatsShieldChangeLabel.GetComponent<Text>().text = ActivePlayer.ShieldChange.ToString();
        UICharacterStatsMaxFuryLabel.GetComponent<Text>().text = ActivePlayer.MaxFury.ToString();
        UICharacterStatsFuryLabel.GetComponent<Text>().text = ActivePlayer.Fury.ToString();
        UICharacterStatsFuryChangeLabel.GetComponent<Text>().text = ActivePlayer.FuryChange.ToString();
        UICharacterStatsMaxManaLabel.GetComponent<Text>().text = ActivePlayer.MaxMana.ToString();
        UICharacterStatsManaLabel.GetComponent<Text>().text = ActivePlayer.Mana.ToString();
        UICharacterStatsManaChangeLabel.GetComponent<Text>().text = ActivePlayer.ManaChange.ToString();
        UICharacterStatsArmorLabel.GetComponent<Text>().text = ActivePlayer.Armor.ToString();
        UICharacterStatsStrengthLabel.GetComponent<Text>().text = ActivePlayer.Strength.ToString();
        UICharacterStatsAttackSpeedLabel.GetComponent<Text>().text = ActivePlayer.AttackSpeed.ToString();
        UICharacterStatsDiscoveryLabel.GetComponent<Text>().text = ActivePlayer.Discovery.ToString();

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
        }
        if (objectToToggle.name == "UIInventoryPanel")
        {
            if (objectToToggle.activeInHierarchy == false)
            {
                selectedItemList = null;
                selectedItem = null;
                UpdateInventoryList();
                UpdateUIInventorySelectedItemLabels(null);
            }
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
        // TODO
        UITargetHPBar.GetComponent<Image>().fillAmount = 0;
        UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
        UITargetOtherBar.GetComponent<Image>().fillAmount = 1;

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
        // TODO
        UITargetHPBar.GetComponent<Image>().fillAmount = 0;
        UITargetShieldBar.GetComponent<Image>().fillAmount = 0;
        UITargetOtherBar.GetComponent<Image>().fillAmount = 1;

        UITargetValueLabel.GetComponent<Text>().text = objectToShow.GetComponent<ItemScript>().Name;

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
            goButton.transform.GetComponent<Image>().color = GlobalsScript.RarityToColor(ActivePlayer.InventoryItemsList[i].GetComponent<ItemScript>().Rarity);
            goButton.transform.FindChild("Text").GetComponent<Text>().text = ActivePlayer.InventoryItemsList[i].GetComponent<ItemScript>().Name;
            Button tempButton = goButton.GetComponent<Button>();
            int i1 = i;
            tempButton.onClick.AddListener(() => SelectItemToShow(ActivePlayer.InventoryItemsList[i1], ActivePlayer.InventoryItemsList, goButton));
        }


        size = UIInventoryEquippedItemsContent.transform.childCount;
        for (int i = 0; i < size; i++)
        {
            Destroy(UIInventoryEquippedItemsContent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < ActivePlayer.EquippedItemsList.Count; i++)
        {
            GameObject goButton = (GameObject)Instantiate(inventoryItemButton);
            goButton.transform.SetParent(UIInventoryEquippedItemsContent.transform, false);
            goButton.transform.GetComponent<Image>().color = GlobalsScript.RarityToColor(ActivePlayer.EquippedItemsList[i].GetComponent<ItemScript>().Rarity);
            goButton.transform.FindChild("Text").GetComponent<Text>().text = ActivePlayer.EquippedItemsList[i].GetComponent<ItemScript>().Name;
            Button tempButton = goButton.GetComponent<Button>();
            int i1 = i;
            tempButton.onClick.AddListener(() => SelectItemToShow(ActivePlayer.EquippedItemsList[i1], ActivePlayer.EquippedItemsList, goButton));
        }

        UIInventoryScrapLabel.GetComponent<Text>().text = ActivePlayer.Scrap.ToString();
}

    private void SelectItemToShow(GameObject item, List<GameObject> itemList, GameObject e = null)
    {
        for (int i = 0; i < UIInventoryItemsContent.transform.childCount; i++)
        {
            //UIInventoryItemsContent.transform.GetChild(i).GetComponent<Image>().color = GlobalsScript.RarityToColor(UIInventoryItemsContent.transform.GetChild(i).GetComponent<ItemScript>().Rarity);
            UIInventoryItemsContent.transform.GetChild(i).transform.FindChild("Text").GetComponent<Text>().fontSize = 14;
            UIInventoryItemsContent.transform.GetChild(i).transform.FindChild("Text").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
        for (int i = 0; i < UIInventoryEquippedItemsContent.transform.childCount; i++)
        {
            // UIInventoryEquippedItemsContent.transform.GetChild(i).GetComponent<Image>().color = GlobalsScript.RarityToColor(UIInventoryEquippedItemsContent.transform.GetChild(i).GetComponent<ItemScript>().Rarity);
            UIInventoryEquippedItemsContent.transform.GetChild(i).transform.FindChild("Text").GetComponent<Text>().fontSize = 14;
            UIInventoryEquippedItemsContent.transform.GetChild(i).transform.FindChild("Text").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        if (e != null)
        {
            //e.GetComponent<Image>().color = Color.cyan;
            e.transform.FindChild("Text").GetComponent<Text>().fontSize = 16;
            e.transform.FindChild("Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
        }

        UpdateUIInventorySelectedItemLabels(item);
        selectedItemList = itemList;
        selectedItem = item;
    }

    private void UpdateUIInventorySelectedItemLabels(GameObject item)
    {
        //Debug.Log(item);
        //Debug.Log((item != null));
        if (item != null)
        {
            UIInventorySelectedItemNameLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Name;
            UIInventorySelectedItemTypeLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Type.ToString();
            UIInventorySelectedItemDamageLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Damage.ToString("####");
            UIInventorySelectedItemAttackSpeedLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Damage.ToString("####");
            UIInventorySelectedItemCriticalChanceLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().CriticalChance.ToString("####");
            UIInventorySelectedItemCriticalDamageLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().CriticalDamage.ToString("####");
            UIInventorySelectedItemRarityLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().Rarity.ToString();
            UIInventorySelectedItemScrapValueLabel.GetComponent<Text>().text = item.GetComponent<ItemScript>().ScrapValue.ToString();
            UIInventorySelectedItemPanel.SetActive(true);
        }
        else
        {
            UIInventorySelectedItemPanel.SetActive(false);
        }

    }

    public void UIInventoryEquipButtonClick()
    {
        if (selectedItem == null) return;
        /*
        for (int i = 0; i < ActivePlayer.EquippedItemsList.Count; i++)
        {
            if (ActivePlayer.EquippedItemsList[i].GetComponent<ItemScript>().Type == selectedItem.GetComponent<ItemScript>().Type)
            {
                Debug.Log("exists");
                ActivePlayer.InventoryItemsList.Add(ActivePlayer.EquippedItemsList[i]);
                ActivePlayer.EquippedItemsList.Remove(ActivePlayer.EquippedItemsList[i]);
            }
        }
        */
        selectedItemList.Remove(selectedItem);
        ActivePlayer.EquippedItemsList.Add(selectedItem);

        // Make equip visible on player model
        ActivePlayer.EquipItem(selectedItem);

        selectedItemList = null;
        selectedItem = null;
        UpdateInventoryList();
        UpdateUIInventorySelectedItemLabels(null);

        
    }

    public void UIInventoryUnquipButtonClick()
    {
        if (selectedItem == null) return;
        selectedItemList.Remove(selectedItem);
        ActivePlayer.InventoryItemsList.Add(selectedItem);

        selectedItemList = null;
        selectedItem = null;
        UpdateInventoryList();
        UpdateUIInventorySelectedItemLabels(null);
    }

    public void UIInventoryDestroyButtonClick()
    {
        if (selectedItem == null) return;

        ActivePlayer.Scrap += selectedItem.GetComponent<ItemScript>().ScrapValue;

        selectedItemList.Remove(selectedItem);

        selectedItemList = null;
        selectedItem = null;
        UpdateInventoryList();
        UpdateUIInventorySelectedItemLabels(null);
    }

    #endregion





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