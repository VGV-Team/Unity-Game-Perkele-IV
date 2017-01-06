using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class UnitScript : EntityScript
{
	
	public struct EquippedItemsStruct
	{
		public GameObject WeaponSlot;
		public GameObject ShieldSlot;
		public GameObject AmuletSlot;
	}
	

	public int MaxHP = 100;
    public int HP = 100;
    public int HPChange = 1;
    public int MaxShield = 20;
    public int Shield = 20;
    public int ShieldChange = 2;
    public int MaxFury = 50;
    public int Fury = 0;
    public int FuryChange = -2;
    public int MaxMana = 50;
    public int Mana = 50;
    public int ManaChange = 1;

    public int Armor = 1;
    public int Strength = 10;
    public int AttackSpeed = 100;
    public int Discovery = 20;
	public int MovementSpeed = 100;

    public int Level = 1;
    public int Xp = 0;
    public int MaxXp = 10;
    public int XPWorth = 10;

    public int Scrap = 0;
	public int Gold = 0;
    public int AbilityPoints = 0;

    // need to add critical
    // need to add discovery
    // abilities should check for this values
    // abilities should also check for equipped items


    public GameObject Target;

    // public Waypoint DestinationWaypoint
    public List<AbilityScript> Abilities = new List<AbilityScript>();

    // TODO: this
    // Inventory
    public List<GameObject> InventoryItemsList = new List<GameObject>();
    // Equiped items
    //public List<GameObject> EquippedItemsList = new List<GameObject>();
	public EquippedItemsStruct EquippedItems;
	
	
	public new void Start()
    {
        base.Start();
        
        MovementInit();
    }

    private double timePassed = 0;
    public void Update()
    {
        UpdateAbilities();

        UpdateMovement();

        timePassed += Time.deltaTime;
        if (timePassed > 1)
        {
            UpdateStatsRefresh();
            timePassed -= 1;
        }

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (Xp >= MaxXp)
        {
            int overheadXP = Xp - MaxXp;

            Debug.Log("LEVEL UP!");

            if (Level >= GlobalsScript.XPCurve.Length)
            {
                // TODO: MAX LEVEL
            }
            else
            {
                MaxXp = GlobalsScript.XPCurve[Level];
                Level++;
                Xp = overheadXP;
            }

            AbilityPoints += 1;
        }
    }


    // This should be called once every second
    private void UpdateStatsRefresh()
    {
        HP += HPChange;
        if (HP > MaxHP) HP = MaxHP;
        if (HP < 0) HP = 0;

        Shield += ShieldChange;
        if (Shield > MaxShield) Shield = MaxShield;
        if (Shield < 0) Shield = 0;

        Fury += FuryChange;
        if (Fury > MaxFury) Fury = MaxFury;
        if (Fury < 0) Fury = 0;

        Mana += ManaChange;
        if (Mana > MaxMana) Mana = MaxMana;
        if (Mana < 0) Mana = 0;
    }


    public void UpdateAbilities()
    {
        foreach (var ability in Abilities)
        {
            ability.Update();
        }
    }





    
    public GameObject waypoint;
    bool SampleWaypointPosition = false;
    bool walkAnim;
    Transform model; // TODO: Should we move model or player?
    //private Animation animation;
    float animationFadeFactor;

    void MovementInit()
    {
        Target = null;
        waypoint = null;
        walkAnim = false;
        animationFadeFactor = 0.15f;
        model = this.gameObject.transform;
        //animation = model.GetComponent<Animation>();
    }

    // TODO: split logic for movement and attacking/actions
    void UpdateMovement()
    {
        if (waypoint)
        {
            if (Target != null)
            {
                waypoint.transform.position = Target.transform.position;
            }
            model.GetComponent<NavMeshAgent>().SetDestination(waypoint.transform.position);
            


            if (!walkAnim)
            {
                StartMovement();
            }

            //if (Vector3.Distance(model.position, waypoint.transform.position) < 1.5f)
            if (Vector3.Distance(model.position, waypoint.transform.position) < 1.5f) 
            {
                StopMovement();
                DestinationReached();
            }
        }
        else
        {
            model.GetComponent<NavMeshAgent>().velocity = new Vector3(0, 0, 0);
        }
    }

    protected virtual void DestinationReached()
    {
        
    }

    public void StopMovement()
    {

        model.FindChild("Model").GetComponent<Animation>().CrossFade("idle", animationFadeFactor);
        this.GetComponent<NavMeshAgent>().ResetPath();

        walkAnim = false;
        Destroy(waypoint);
    }

    public void StartMovement()
    {

        model.FindChild("Model").GetComponent<Animation>().CrossFade("run", animationFadeFactor);
        walkAnim = true;
    }

    public void StartDeathAnimation()
    {

        model.FindChild("Model").GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
        model.FindChild("Model").GetComponent<Animation>().CrossFade("die", animationFadeFactor);
        
    }

    public void AnimationEventFunction(string type)
    {
        Debug.Log("Anim event!");
        TriggerAbilityImpact(type);

    }

    private void TriggerAbilityImpact(string abilityName)
    {

        AbilityType aType = AbilityType.BasicAttack;

        // String to Enum :)
        switch (abilityName)
        {
            case "Heal":
                aType = AbilityType.Heal;
                break;
            case "RangedAttack":
                aType = AbilityType.RangeAttack;
                break;
            default:
                break;
        }

        foreach (var ability in Abilities)
        {
            if (ability.Type == aType)
            {
                ability.AbilityImpact(this.gameObject, Target);
            }
        }
    }

    public void Die()
    {
        StartDeathAnimation();
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.transform.FindChild("Minimap Marker").gameObject.SetActive(false);

    }

    public void StartBasicAttackAnimation(float attackCooldown = -1)
    {
        // TODO: this
        StopMovement();
        if (attackCooldown != -1)
        {

            AnimationState state = model.FindChild("Model").GetComponent<Animation>()["attack"];
            state.speed = state.length / attackCooldown;
            model.FindChild("Model").GetComponent<Animation>().CrossFade("attack", animationFadeFactor);
           
        }

        
    }

    public void StartRangeAttackAnimation()
    {
        // TODO: this
    }

    public void StartHealAnimation()
    {
        // TODO: this
        model.FindChild("Model").GetComponent<Animation>().CrossFade("dance", animationFadeFactor);
    }

    public void StartHitAnimation()
    {
        // TODO: this
        //model.FindChild("Model").GetComponent<Animation>().Blend("gethit", animationFadeFactor);
    }

    public void SetWaypoint(GameObject target)
    {
        SetWaypoint(target.transform.position);
        this.Target = target;
    }

    public void SetWaypoint(Vector3 point)
    {
        Target = null;
        if (this.waypoint != null) Destroy(this.waypoint);

        // Set a waypoint, player update will move the player to the waypoint
        GameObject waypointTmp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        waypointTmp.name = "Waypoint from " + Name;
        waypointTmp.GetComponent<Renderer>().material.color = Color.red;
        waypointTmp.GetComponent<Collider>().enabled = false;
        waypointTmp.transform.position = point;

        this.waypoint = waypointTmp;
    }

    public void MoveTo(RaycastHit hit)
    {
        NavMeshHit nmHit;
        if (SampleWaypointPosition)
        {
            NavMesh.SamplePosition(hit.point, out nmHit, 10, 1 << NavMesh.GetAreaFromName("Walkable"));
            SetWaypoint(nmHit.position);
        }
        else
        {
            SetWaypoint(hit.point);
        }
    }





	protected void PickUpItem(GameObject item)
	{
		// if in range, try to pick up
		if (item.GetComponent<ItemScript>().PlayerTouching)
		{
			StopMovement();
			InventoryItemsList.Add(item);
			//Destroy(item);
			item.SetActive(false);
			if (GameObject.Find("InputHandlerObject").GetComponent<InputHandlerScript>().HoveredObject == Target)
			{
				GameObject.Find("InputHandlerObject").GetComponent<InputHandlerScript>().HoveredObject = null;
			}
			Target = null;
			item.GetComponent<ItemScript>().PlayerTouching = false;
		}
	}

	protected void OpenChest(GameObject chest)
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
				if (EquippedItems.WeaponSlot != null)
				{
					InventoryItemsList.Add(EquippedItems.WeaponSlot);
				}
				EquippedItems.WeaponSlot = item;
				InventoryItemsList.Remove(item);

				// Temporary -- TODO: sanitize object when picking up
				GameObject playerWeapon = GameObject.Instantiate(item);
				Component[] comps = playerWeapon.GetComponents(typeof(Component));
				foreach (Component c in comps)
				{
					if (!(c is MeshFilter || c is MeshRenderer || c is ItemScript || c is Transform))
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
				playerWeapon.transform.localRotation = Quaternion.Euler(playerWeapon.transform.localRotation.eulerAngles.x, playerWeapon.transform.localRotation.eulerAngles.y, 0);
				playerWeapon.gameObject.SetActive(true);

				Debug.Log("DONE!!!!");

				break;

			case ItemType.Shield:
				if (EquippedItems.ShieldSlot != null)
				{
					InventoryItemsList.Add(EquippedItems.ShieldSlot);
				}
				EquippedItems.ShieldSlot = item;
				InventoryItemsList.Remove(item);
				break;

			default:
				Debug.Log("Player:EquipItem - Unimplemented weapon type equip");
				break;
		}
	}

	public void UnequipItem(GameObject item)
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
				InventoryItemsList.Add(item);
				EquippedItems.WeaponSlot = null;
				break;

			case ItemType.Shield:
				InventoryItemsList.Add(item);
				EquippedItems.ShieldSlot = null;
				break;

			default:
				Debug.Log("Player:EquipItem - Unimplemented weapon type equip");
				break;
		}
	}

	public void DestroyItem(GameObject item)
	{
		Scrap += item.GetComponent<ItemScript>().ScrapValue;
		InventoryItemsList.Remove(item);
	}

	public void StatsUpgrade(string statName)
	{
		if (AbilityPoints <= 0) return;
		AbilityPoints--;

		switch (statName)
		{
			case "MaxHP":
				MaxHP = (int)(MaxHP * 1.1);
				break;
			case "HPChange":
				HPChange = (int)(HPChange * 1.1);
				break;
			case "MaxShield":
				MaxShield = (int)(MaxShield * 1.1);
				break;
			case "ShieldChange":
				ShieldChange = (int)(ShieldChange * 1.1);
				break;
			case "MaxFury":
				MaxFury = (int)(MaxFury * 1.1);
				break;
			case "FuryChange":
				FuryChange = (int)(FuryChange * 0.9);
				break;
			case "MaxMana":
				MaxMana = (int)(MaxMana * 1.1);
				break;
			case "ManaChange":
				ManaChange = (int)(ManaChange * 1.1);
				break;
			case "Armor":
				Armor = (int)(Armor * 1.1);
				break;
			case "Strength":
				Strength = (int)(Strength * 1.1);
				break;
			case "AttackSpeed":
				AttackSpeed = (int)(AttackSpeed * 1.1);
				break;
			case "Discovery":
				Discovery = (int)(Discovery * 1.1);
				break;
		}
	}

}
