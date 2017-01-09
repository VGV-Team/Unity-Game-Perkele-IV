using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : UnitScript
{

    public int ViewRange;

    private GameObject Player;

    public bool MeleeAttack = true;
    public bool RangedAttack = false;
    public bool Necromancer = false;
    

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        if (MeleeAttack) Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 2, 0, 0, 2.5, 20)); //20 <-- Strength
        else if (RangedAttack) Abilities.Add(new AbilityScript("Fireball", AbilityType.Fireball, 1, 0, 0, ViewRange, 150, GameObject.Find("UISpritesFireball").transform.GetComponent<SpriteRenderer>().sprite));
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    new void Update()
    {
        if (Active != true) return;
        base.Update();

        if (Necromancer)
        {
            Target = null;
            waypoint = null;
            return;
        }

        if (Target != null)
        {
            float distance = Vector3.Distance(this.transform.position, Target.transform.position);
            Debug.Log(distance);
            // out of view range
            bool canSee = CheckVisibility();
            if (distance > ViewRange || !canSee)
            {
                if (distance > ViewRange)
                {
                    // For mage so canSee doesen't trigger this block
                    SetWaypoint(Target.transform.position);
                }
                Target = null;
                //StopMovement(); If this is commented, enemy will move to last known location upon losing LoS
            }
            else
            {

           
                if (RangedAttack)
                {
                    
                    // IF this unit is ranged and player too close, run away!
                    if (distance < FallbackDistance)
                    {
                        Debug.Log("Fall back");
                        Vector3 fallbackWaypoint;
                        fallbackWaypoint = this.transform.position - Player.transform.position;
                        fallbackWaypoint = this.transform.position + fallbackWaypoint * 1.0f;
                        SetWaypoint(fallbackWaypoint);
                        fallingBack = true;
                    }
                    else
                    {
                        fallingBack = false;
                    }

                    
                }

                if (fallingBack) return;
                

                bool ok = false;
                foreach (var ability in Abilities)
                {
                    //if(ability.Type == AbilityType.BasicAttack) ability.Use(this.gameObject, Target);
                    if (ability.Range >= distance) // use all possible abilities for now
                    {
                        if (MeleeAttack && ability.Type != AbilityType.BasicAttack) continue;
                        if (RangedAttack && ability.Type != AbilityType.Fireball) continue;
                        Debug.Log(ability.Name + " " + distance);
                        ok = true;
                        ability.Use(this.gameObject, Target);
                        break;
                    }
                }
                if (!ok)
                {
                    if (MeleeAttack)
                    {
                        // If this unit is melee oriented, chase the player
                        SetWaypoint(Target);
                    }
                }
                

            }

        }
        else
        {
            if (Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position) < ViewRange)
            {
                if (CheckVisibility())
                {
                    Debug.Log("***********************************");
                    //SetWaypoint(GameObject.Find("Player"));
                    Target = GameObject.Find("Player");
                }
            }
        }
    }

    protected override void DestinationReached()
    {

    }

    // Raycast from enemy position to player postiion to determine if the enemy can see the player
    private bool CheckVisibility()
    {
        RaycastHit hit;

        // If you want to look from this objects head (roughly): this.GetComponent<Collider>().bounds.size.y - 0.05f

        // Current models are y=0 at ground, so we need to add a little to y to evaluate things a little above terrain
        //Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.1f, 0.0f), (Player.transform.position + new Vector3(0.0f, 0.1f, 0.0f) - this.transform.position).normalized, out hit, 100);

        LayerMask layerMask = ~(1 << 11); // Raycast ALL (!) layers || EXCEPT FIRELAYER (so mage doesen't lose visibility when firing)

        //Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.1f, 0.0f), (Player.transform.position + new Vector3(0.0f, 2.0f, 0.0f) - this.transform.position).normalized, out hit, 100, layerMask);

        Vector3 origin = this.transform.position + new Vector3(0, 0.5f, 0);
        Vector3 direction = Player.transform.position + new Vector3(0, 0.5f, 0) - origin;
        Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.1f, 0.0f), (Player.transform.position + new Vector3(0.0f, 2.0f, 0.0f) - this.transform.position).normalized, out hit, 100, layerMask);

        if (hit.collider!=null && hit.collider.gameObject.tag.Equals("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
