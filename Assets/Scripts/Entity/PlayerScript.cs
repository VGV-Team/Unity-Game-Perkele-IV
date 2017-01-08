using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : UnitScript
{
	public GameObject pointLight;
	public float pointLightMaxIntensity = 3;

	// Use this for initialization
    new void Start ()
	{
	    base.Start();
        Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 2, 0, 0, 3, 5, GameObject.Find("UISpritesBasicAttack").transform.GetComponent<SpriteRenderer>().sprite));
        Abilities.Add(new AbilityScript("Fireball", AbilityType.Fireball, 5, 0, 10, 10, 20, GameObject.Find("UISpritesFireball").transform.GetComponent<SpriteRenderer>().sprite));
        Abilities.Add(new AbilityScript("Heal", AbilityType.Heal, 5, 0, 10, 0, 20, GameObject.Find("UISpritesHeal").transform.GetComponent<SpriteRenderer>().sprite));
        Abilities.Add(new AbilityScript("Fire Explosion", AbilityType.FireExplosion, 10, 0, 0, 10, 20, GameObject.Find("UISpritesFireExplosion").transform.GetComponent<SpriteRenderer>().sprite));
        Abilities.Add(new AbilityScript("Flamethrower", AbilityType.Flamethrower, 10, 0, 0, 10, 80, GameObject.Find("UISpritesFlamethrower").transform.GetComponent<SpriteRenderer>().sprite));


        // Set starting level maxXP
        MaxXp = GlobalsScript.XPCurve[Level - 1];
    }

    // Update is called once per frame
    new void Update ()
	{
        if (Active != true) return;
        base.Update();

		if (HP <= 0) pointLight.GetComponent<Light>().intensity = 0;
		else pointLight.GetComponent<Light>().intensity = HP / MaxHP * pointLightMaxIntensity;

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
				case "NPC":
		            InteractWithNPC(Target);
					break;
                /*case "Crate":
                    DestroyCrate(Target);
                    break;*/
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

    

    protected override void DestinationReached()
    {
        
    }

}
