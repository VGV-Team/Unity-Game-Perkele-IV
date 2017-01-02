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
    }

    // Update is called once per frame
    new void Update ()
	{
        if (Active != true) return;
        base.Update();


        if (Target != null && Target.tag == "Enemy")
        {
            foreach (var ability in Abilities)
            {
                if (ability.Type == AbilityType.BasicAttack && ability.Use(this.gameObject, Target))
                {
                    // TODO: should we click for basic attack? If yes then break
                    break;
                    //Target = null;
                }
            }
        }

        if (Target != null && Target.tag == "Item")
        {
            PickUpItem(Target);
        }
    }

    public void PickUpItem(GameObject item)
    {
        // if in range, try to pick up
        if (item.GetComponent<ItemScript>().PlayerTouching)
        {
            StopMovement();
            /// TODO: Add to inventory
            Destroy(item);
        }
    }

    protected override void DestinationReached()
    {
        
    }
}
