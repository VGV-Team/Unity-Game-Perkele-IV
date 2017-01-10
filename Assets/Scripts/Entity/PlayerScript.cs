using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerScript : UnitScript
{
	public GameObject pointLight;
	public float pointLightMaxIntensity = 3;


    private NavMeshAgent agent;

    // Use this for initialization
    new void Start ()
	{
	    base.Start();
        Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 1, -10, 0, 3, 5, GameObject.Find("UISpritesBasicAttack").transform.GetComponent<SpriteRenderer>().sprite));
		Abilities.LastOrDefault().Description = "Exceptionally magnificent skill that owns everything";
		Abilities.Add(new AbilityScript("Heal", AbilityType.Heal, 10, 0, 25, 0, 50, GameObject.Find("UISpritesHeal").transform.GetComponent<SpriteRenderer>().sprite));
		Abilities.LastOrDefault().Description = "Magnificent heal for magnificent owner";

		

		

		

		// Set starting level maxXP
		MaxXp = GlobalsScript.XPCurve[Level - 1];

        agent = GetComponent<NavMeshAgent>();
    }

    public bool basicAttackClick = false;

    // Update is called once per frame
    new void Update ()
	{
        if (Active != true) return;
        base.Update();

		if (HP <= 0) pointLight.GetComponent<Light>().intensity = 0;
		else pointLight.GetComponent<Light>().intensity = HP / MaxHP * pointLightMaxIntensity;


        #region Out Of Bounds Fix
        // OUT OF BOUNDS WAYPOINT FIX
        NavMeshPath path = new NavMeshPath();
        if (waypoint)
        {
            
            AudioManager.PlayFootstepAudio(this.GetComponent<AudioSource>());

            agent.CalculatePath(waypoint.transform.position, path);
            if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
            {
                //Debug.Log("Out of bounds!");
                //if waypoint not reachable, stop
                StopMovement();
            }
        }

        #endregion


        #region Target Checking

        if (Target != null)
        {

            float distance = Vector3.Distance(this.transform.position, Target.transform.position);
            //Debug.Log(Target.tag); 
            switch (Target.tag)
            {
                case "Enemy":
                    foreach (var ability in Abilities)
                    {
                        //if (ability.Type == AbilityType.BasicAttack && ability.Use(this.gameObject, Target))
                        if (ability.Type == AbilityType.BasicAttack && distance <= ability.Range)
                        {
                            Destroy(waypoint);
                            waypoint = null;
                            //Debug.Log(attackAnimEnd);
                            if (ability.Use(this.gameObject, Target))
                            {
                                basicAttackClick = false;
                                break;
                            }
                            else if (ability.TimeToReady > 0 && basicAttackClick)
                            {
                                //Debug.Log("STOPPING");
                                StopMovement();
                            }

                            // TODO: should we click for basic attack? If yes then break

                            //break;
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
                case "NPC":
                    InteractWithNPC(Target);
                    break;
                /*case "Crate":
                    DestroyCrate(Target);
                    break;*/
                default:
                    //Debug.Log("DEFAULT playerscript switch");
                    break;

            }

        }

        #endregion

        
    }

    /*new void OnMouseEnter()
    {
        //override as we don't want the player to block hover object
    }*/

    

    protected override void DestinationReached()
    {
        
    }

}
