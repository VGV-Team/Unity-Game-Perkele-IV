using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitScript : EntityScript
{
    public int MaxHP;
    public int HP;
    public int HPChange;
    public int MaxShield;
    public int Shield;
    public int ShieldChange;
    public int MaxFury;
    public int Fury;
    public int FuryChange;
    public int MaxMana;
    public int Mana;
    public int ManaChange;

    public int Armor;
    public int Strength;
    public int AttackSpeed;
    public int Discovery;

    public int Level;
    public int Xp = 0;
    public int MaxXp = 10;
    public int XPWorth = 10;

    public int Scrap = 0;
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
    public List<GameObject> EquippedItemsList = new List<GameObject>();


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
}
