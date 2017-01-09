using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	

	public float MaxHP = 100;
    public float HP = 100;
    public float HPChange = 1;
    public float MaxShield = 20;
    public float Shield = 20;
    public float ShieldChange = 2;
    public float MaxFury = 50;
    public float Fury = 0;
    public float FuryChange = -2;
    public float MaxMana = 50;
    public float Mana = 50;
    public float ManaChange = 1;

    public float Armor = 1;
    public float Strength = 10;
    public float AttackSpeed = 100;
	public float CriticalChance = 10;
    public float Discovery = 20;
	public float MovementSpeed = 100;

    public int Level = 1;
    public int Xp = 0;
    public int MaxXp = 10;
    public int XPWorth = 10;

    public int Scrap = 0;
	public int Gold = 0;
    public int AbilityPoints = 0;

    protected bool fallingBack = false;
    public float FallbackDistance = 6.0f;


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

	public List<QuestScript> QuestList = new List<QuestScript>();

	public new void Start()
    {
        base.Start();

        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        MovementInit();
    }

    //private double timePassed = 0;
    public void Update()
    {
        UpdateAbilities();

        UpdateMovement();

        //timePassed += Time.deltaTime;
        //if (timePassed > 1)
        //{
        UpdateStatsRefresh();
        //    timePassed -= 1;
       // }

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

	        if (this.tag=="Player" && Level == 2)
	        {
				Abilities.Add(new AbilityScript("Fire Explosion", AbilityType.FireExplosion, 15, 10, 0, 15, 20, GameObject.Find("UISpritesFireExplosion").transform.GetComponent<SpriteRenderer>().sprite));
				Abilities.LastOrDefault().Description = "Special magnificent fire explosion that will kill something";
			}
			if (this.tag == "Player" && Level == 3)
			{
				Abilities.Add(new AbilityScript("Flamethrower", AbilityType.Flamethrower, 20, 10, 0, 20, 80, GameObject.Find("UISpritesFlamethrower").transform.GetComponent<SpriteRenderer>().sprite));
				Abilities.LastOrDefault().Description = "Flame and freedom all in one magnificent skil";
			}

		}
    }


    // This should be called once every second
    private void UpdateStatsRefresh()
    {
        HP += HPChange * Time.deltaTime;
        if (HP > MaxHP) HP = MaxHP;
        if (HP < 0) HP = 0;

        Shield += ShieldChange * Time.deltaTime;
        if (Shield > MaxShield) Shield = MaxShield;
        if (Shield < 0) Shield = 0;

        Fury += FuryChange * Time.deltaTime;
        if (Fury > MaxFury) Fury = MaxFury;
        if (Fury < 0) Fury = 0;

        Mana += ManaChange * Time.deltaTime;
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
    protected bool walkAnim;
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
        model.GetComponent<NavMeshAgent>().speed = MovementSpeed/100;

        if (waypoint)
        {
            if (Target != null && !fallingBack)
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
        waypoint = null;
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

    public void AnimationEventFunctionRelay(string type)
    {
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
            case "AbilityFireball":
                aType = AbilityType.Fireball;
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
        if (waypoint) Destroy(waypoint);

	    if (this.gameObject.tag == "Player")
	    {
		    GlobalsScript.IsPlayerAlive = false;
	    }

    }

    public void StartBasicAttackAnimation(float attackCooldown = -1)
    {
        // TODO: this
        StopMovement();
        if (attackCooldown != -1)
        {
            attackCooldown = attackCooldown / (AttackSpeed / 100.0f);
            AnimationState state = model.FindChild("Model").GetComponent<Animation>()["attack"];
            state.speed = (state.length+4*animationFadeFactor) / attackCooldown;
            model.FindChild("Model").GetComponent<Animation>().CrossFade("attack", animationFadeFactor);
           
        }

        
    }
    
    public void StartFireballAnimation()
    {
        model.FindChild("Model").GetComponent<Animation>().CrossFade("fireball", animationFadeFactor);
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
        waypointTmp.GetComponent<Renderer>().material.color = GlobalsScript.WaypointColor;
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



    protected AudioManagerScript AudioManager;

	protected void PickUpItem(GameObject item)
	{
		// if in range, try to pick up
		if (item.GetComponent<ItemScript>().PlayerTouching)
		{
            AudioManager.PlayPickupItemAudio(this.GetComponent<AudioSource>());

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
				this.Target = null;
			}

		}
	}


    protected void InteractWithNPC(GameObject NPC)
	{
		// TODO: change to player touching
		
		if (NPC.GetComponent<NPCScript>().PlayerTouching)
		{
			Debug.Log("Starting NPC conversation");
			Debug.Log("Starting NPC conversation");
			StopMovement();

			NPC.GetComponent<NPCScript>().StartConversation(this.gameObject);

			this.Target = null;
		}
		
		/*
		if (Vector3.Distance(NPC.transform.position, GameObject.Find("Player").transform.position) < 2)
		{
			Debug.Log("Starting NPC conversation");
			StopMovement();

			NPC.GetComponent<NPCScript>().StartConversation(this.gameObject);


			this.Target = null;
		}
		*/

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

        Component[] comps;
        Transform[] children;
        Transform oldItem = null;
        Transform template = null;

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
				comps = playerWeapon.GetComponents(typeof(Component));
				foreach (Component c in comps)
				{
					if (!(c is MeshFilter || c is MeshRenderer || c is ItemScript || c is Transform))
					{
						Destroy(c);
					}
				}
                Destroy(playerWeapon.transform.FindChild("Minimap Marker").gameObject);
				//playerWeapon.GetComponent<MeshRenderer>().enabled = true;
				playerWeapon.name = "Sword";
                playerWeapon.gameObject.layer = 11;
                playerWeapon.transform.FindChild("Model").gameObject.layer = 11;

                // Finding the old (current) weapon
                children = GetComponentsInChildren<Transform>();
                oldItem = null;
				template = null;
				foreach (Transform child in children)
				{
					if (child.CompareTag("Item") && child.name == "Sword")
					{
                        oldItem = child;
					}
					if (child.name == "TemplateSword")
					{
						template = child;
					}
				}
				if (template == null)
				{
					Debug.Log("Template not found!");
					return;
				}
                if (oldItem != null) Destroy(oldItem.gameObject);

                // Positioning the new weapon
                // 0,0,0 position is now the center of the player
                playerWeapon.transform.parent = template.parent;
				playerWeapon.transform.localPosition = Vector3.zero;

				// Set offset (position) the same as old weapon
				playerWeapon.transform.position = template.transform.position;
				playerWeapon.transform.rotation = template.transform.rotation;

				// Additional offset tweaking (localposition instead of global)
				//playerWeapon.transform.localPosition += new Vector3(0, 0, -0.10f); // How much in the hand should the weapon handle go?
				//playerWeapon.transform.localRotation = Quaternion.Euler(playerWeapon.transform.localRotation.eulerAngles.x, playerWeapon.transform.localRotation.eulerAngles.y, 0);
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

                // Temporary -- TODO: sanitize object when picking up
                GameObject playerShield = GameObject.Instantiate(item);
                comps = playerShield.GetComponents(typeof(Component));
                foreach (Component c in comps)
                {
                    if (!(c is MeshFilter || c is MeshRenderer || c is ItemScript || c is Transform))
                    {
                        Destroy(c);
                    }
                }
                Destroy(playerShield.transform.FindChild("Minimap Marker").gameObject);
                //playerShield.GetComponent<MeshRenderer>().enabled = true;
                playerShield.name = "Shield";
                playerShield.gameObject.layer = 11;
                playerShield.transform.FindChild("Model").gameObject.layer = 11;

                // Finding the old (current) weapon
                children = GetComponentsInChildren<Transform>();
                oldItem = null;
                template = null;
                foreach (Transform child in children)
                {
                    if (child.CompareTag("Item") && child.name == "Shield")
                    {
                        oldItem = child;
                    }
                    if (child.name == "TemplateShield")
                    {
                        template = child;
                    }
                }
                if (template == null)
                {
                    Debug.Log("Old shield not found!");
                    return;
                }
                if (oldItem != null)  Destroy(oldItem.gameObject);

                // Positioning the new weapon
                // 0,0,0 position is now the center of the player
                playerShield.transform.parent = template.parent;
                playerShield.transform.localPosition = Vector3.zero;
                playerShield.transform.localRotation = Quaternion.Euler(Vector3.zero);

                // Set offset (position) the same as old weapon
                playerShield.transform.position = template.transform.position;
                playerShield.transform.rotation = template.transform.rotation;

                // Additional offset tweaking (localposition instead of global)
                //playerShield.transform.localPosition += new Vector3(0, 0, -0.10f); // How much in the hand should the weapon handle go?
                //playerShield.transform.localRotation = Quaternion.Euler(playerShield.transform.localRotation.eulerAngles.x, playerShield.transform.localRotation.eulerAngles.y, 0);
                playerShield.gameObject.SetActive(true);

                Debug.Log("DONE SHIELD!!!!");

                break;

            case ItemType.Amulet:
                if (item.transform.FindChild("Minimap Marker") != null)
                    Destroy(item.transform.FindChild("Minimap Marker").gameObject);
                if (EquippedItems.AmuletSlot != null)
                {
                    InventoryItemsList.Add(EquippedItems.AmuletSlot);
                }
                EquippedItems.AmuletSlot = item;
                InventoryItemsList.Remove(item);

                Transform amuletLight = this.transform.FindChild("Amulet Light");
                if (amuletLight != null) amuletLight.gameObject.SetActive(true);

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
        Component[] children;
        switch (itemScript.Type)
		{
			case ItemType.Melee:
				InventoryItemsList.Add(item);
				EquippedItems.WeaponSlot = null;
                children = GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.CompareTag("Item") && child.name == "Sword")
                    {
                        Destroy(child.gameObject);
                        break;
                    }
                }
                break;

			case ItemType.Shield:
				InventoryItemsList.Add(item);
				EquippedItems.ShieldSlot = null;
                children = GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.CompareTag("Item") && child.name == "Shield")
                    {
                        Destroy(child.gameObject);
                        break;
                    }
                }
                break;

            case ItemType.Amulet:
                InventoryItemsList.Add(item);
                EquippedItems.AmuletSlot = null;

                Transform amuletLight = this.transform.FindChild("Amulet Light");
                if (amuletLight != null) amuletLight.gameObject.SetActive(false);

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
			case "CriticalChance":
				CriticalChance = (int)(CriticalChance * 1.1);
				break;
			case "Discovery":
				Discovery = (int)(Discovery * 1.1);
				break;
		}
	}

}
