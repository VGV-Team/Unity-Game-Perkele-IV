using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : UnitScript {


    

	// Use this for initialization
    new void Start ()
	{
	    base.Start();
        Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 2, 0, 0, 3, 5, GameObject.Find("UISpritesBasicAttack").transform.GetComponent<SpriteRenderer>().sprite));
        Abilities.Add(new AbilityScript("Heal", AbilityType.Heal, 5, 0, 10, 0, 20, GameObject.Find("UISpritesHeal").transform.GetComponent<SpriteRenderer>().sprite));
        Abilities.Add(new AbilityScript("Whatever Ability", AbilityType.RangeAttack, 10, 10, 0, 10, 20, GameObject.Find("UISpritesWhatever").transform.GetComponent<SpriteRenderer>().sprite));

        // Set starting level maxXP
        MaxXp = GlobalsScript.XPCurve[Level - 1];
    }

    // Update is called once per frame
    new void Update ()
	{
        if (Active != true) return;
        base.Update();

        #region Target Checking

        if (Target != null)
        {

            switch (Target.tag)
            {
                case "Enemy":
                    foreach (var ability in Abilities)
                    {
                        if (ability.Type == AbilityType.BasicAttack && ability.Use(this.gameObject, Target))
                        {
                            // TODO: should we click for basic attack? If yes then break
                            break;
                            //Target = null;
                        }
                    }
                    break;

                case "Item":
                    PickUpItem(Target);
                    break;

                case "Chest":
                    OpenChest(Target);
                    break;

                default:
                    Debug.Log("DEFAULT playerscript switch");
                    break;

            }

        }

        #endregion

        
    }

    /*new void OnMouseEnter()
    {
        //override as we don't want the player to block hover object
    }*/

    private void PickUpItem(GameObject item)
    {
        // if in range, try to pick up
        if (item.GetComponent<ItemScript>().PlayerTouching)
        {
            StopMovement();
            InventoryItemsList.Add(item);
            //Destroy(item);
            item.SetActive(false);
            item.GetComponent<ItemScript>().PlayerTouching = false;
        }
    }

    private void OpenChest(GameObject chest)
    {
        Debug.Log("TRYING Opening chest");
        if (chest.GetComponent<ChestScript>().PlayerTouching && !chest.GetComponent<ChestScript>().Opened)
        {
            //player is in range, but does he have enough scrap?
            if (Scrap >= chest.GetComponent<ChestScript>().ScrapRequired)
            {
                Debug.Log("Opening chest");
                Scrap -= chest.GetComponent<ChestScript>().ScrapRequired;
                StopMovement();
                chest.GetComponent<ChestScript>().OpenChest();
            }

        }
    }

    public void EnableTriggerCollider()
    {
        this.GetComponent<Collider>().enabled = true;
    }
    public void DisableTriggerCollider()
    {
        this.GetComponent<Collider>().enabled = false;
    }

    public void EquipItem(GameObject item)
    {


        ItemScript itemScript = item.GetComponent<ItemScript>();

        if (itemScript == null)
        {
            Debug.Log("Invalid equip item");
            return;
        }

        switch (itemScript.Type)
        {
            case ItemType.Melee:

                // Temporary -- TODO: sanitize object when picking up
                GameObject playerWeapon = GameObject.Instantiate(item);
                Component[] comps = playerWeapon.GetComponents(typeof(Component));
                foreach (Component c in comps)
                {
                    if ( ! (c is MeshFilter || c is MeshRenderer || c is ItemScript || c is Transform) )
                    {
                        Destroy(c);
                    }
                }
                playerWeapon.GetComponent<MeshRenderer>().enabled = true;
                playerWeapon.name = "Sword";


                // Finding the old (current) weapon
                Transform[] children = GetComponentsInChildren<Transform>();
                Transform oldWeapon = null;
                Transform template = null;
                foreach (Transform child in children)
                {
                    if (child.CompareTag("Item"))
                    {
                        oldWeapon = child;
                    }
                    if (child.name == "TemplateSword")
                    {
                        template = child;
                    }
                }
                if (oldWeapon == null)
                {
                    Debug.Log("Old weapon not found!");
                    return;
                }
                oldWeapon.gameObject.SetActive(false);
                oldWeapon.name = "SwordOLD";

                // Positioning the new weapon
                // 0,0,0 position is now the center of the player
                playerWeapon.transform.parent = oldWeapon.parent;
                playerWeapon.transform.localPosition = Vector3.zero;

                // Set offset (position) the same as old weapon
                playerWeapon.transform.position = template.transform.position;
                playerWeapon.transform.rotation = template.transform.rotation;

                // Additional offset tweaking (localposition instead of global)
                playerWeapon.transform.localPosition += new Vector3(0, 0, -0.10f); // How much in the hand should the weapon handle go?
                playerWeapon.transform.localRotation =  Quaternion.Euler(playerWeapon.transform.localRotation.eulerAngles.x, playerWeapon.transform.localRotation.eulerAngles.y, 0);
                playerWeapon.gameObject.SetActive(true);

                Debug.Log("DONE!!!!");

                break;

            default:
                Debug.Log("Player:EquipItem - Unimplemented weapon type equip");
                break;
        }
    }

    protected override void DestinationReached()
    {
        
    }
}
