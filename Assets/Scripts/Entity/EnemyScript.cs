using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : UnitScript
{

    public int ViewRange;



    // Use this for initialization
    new void Start ()
    {
        base.Start();
        Abilities.Add(new AbilityScript("Basic Attack", AbilityType.Basic, 2, 0, 0, 2));

        ViewRange = 15;
    }

    // Update is called once per frame
    new void Update ()
    {
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
            else if (distance > 2.5f)
            {
                SetWaypoint(GameObject.Find("Player"));
            }

            // use basic attack

        }
        else
        {
            if (Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position) < ViewRange)
            {
                SetWaypoint(GameObject.Find("Player"));
            }
        }

    }
}
