using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitScript : EntityScript
{
    public int MaxHP;
    public int HP;
    public int HPRegen;
    public int Armor;
    public int MaxShield;
    public int Shield;
    public int Strenth;
    public int MaxFury;
    public int Fury;
    public int FuryDecay;
    public int MaxMana;
    public int Mana;
    public int ManaRegen;
    
    public int Level;
    public int Xp;
    public int MaxXp;

    public GameObject Target;

    // public Waypoint DestinationWaypoint
    public List<AbilityScript> Abilities;

    public new void Start()
    {
        base.Start();

        Abilities = new List<AbilityScript>();
        
        MovementInit();
    }

    public void Update()
    {
        UpdateAbilities();

        UpdateMovement();
   
    }


    public void UpdateAbilities()
    {
        foreach (var ability in Abilities)
        {
            ability.Update();
        }
    }

    
    public GameObject waypoint;
    bool SampleWaypointPosition;
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

            if (Vector3.Distance(model.position, waypoint.transform.position) < 1.5f)
            {
                StopMovement();
            }
        }
        else
        {
            model.GetComponent<NavMeshAgent>().velocity = new Vector3(0, 0, 0);
        }
    }

    public void StartDeathAnimation()
    {
        // TODO: this
        Destroy(this.gameObject);
    }

    public void StopMovement()
    {
        //this.GetComponent<Animator>().CrossFade("idle", 0.15f);
        model.FindChild("Model").GetComponent<Animation>().CrossFade("idle", animationFadeFactor);
        walkAnim = false;
        Destroy(waypoint);
    }

    public void StartMovement()
    {
        //this.GetComponent<Animator>().CrossFade("walk", 0.15f);
        model.FindChild("Model").GetComponent<Animation>().CrossFade("run", animationFadeFactor);
        walkAnim = true;
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
