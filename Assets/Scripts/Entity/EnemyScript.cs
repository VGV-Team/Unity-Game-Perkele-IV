using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : UnitScript
{

    public int ViewRange;


    private GameObject Player;

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 2, 0, 0, 2, Strenth));

        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    new void Update()
    {
        if (Active != true) return;
        base.Update();


        if (Target != null)
        {
            float distance = Vector3.Distance(this.transform.position, Target.transform.position);

            // out of view range
            
            if (distance > ViewRange)
            {
                Target = null;
                StopMovement();
            }
            else if (distance <= Abilities[0].Range)
            {
                Abilities[0].Use(this.gameObject, Target);
            }
            else if (distance > 2.5f)
            {
                if (CheckVisibility())
                {
                    //Debug.Log("QWE1");
                    SetWaypoint(GameObject.Find("Player"));
                }
                else
                {
                    Target = null;
                }
                    
            }
            
            
            // use basic attack

        }
        else
        {
            if (Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position) < ViewRange)
            {
                if (CheckVisibility())
                {
                    //Debug.Log("QWE2");
                    SetWaypoint(GameObject.Find("Player"));
                }
            }
        }
    }

    // Raycast from enemy position to player postiion to determine if the enemy can see the player
    private bool CheckVisibility()
    {

        RaycastHit hit;

        // If you want to look from this objects head (roughly): this.GetComponent<Collider>().bounds.size.y - 0.05f

        // Current models are y=0 at ground, so we need to add a little to y to evaluate things a little above terrain
        //Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.1f, 0.0f), (Player.transform.position + new Vector3(0.0f, 0.1f, 0.0f) - this.transform.position).normalized, out hit, 100);

        Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.1f, 0.0f), (Player.transform.position + new Vector3(0.0f, this.GetComponent<Collider>().bounds.size.y - 0.1f, 0.0f) - this.transform.position).normalized, out hit, 100);

        //Debug.Log(hit.collider.gameObject.tag);
        if (hit.collider.gameObject.tag.Equals("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
