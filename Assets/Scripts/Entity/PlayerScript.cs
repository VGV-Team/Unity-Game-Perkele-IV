using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : UnitScript {


	// Use this for initialization
    new void Start ()
	{
	    base.Start();
        Abilities.Add(new AbilityScript("Basic Attack", AbilityType.BasicAttack, 2, 0, 0, 2, Strenth));
        Abilities.Add(new AbilityScript("Heal", AbilityType.RangeAttack, 5, 10, 0, 10, 20));
        Abilities.Add(new AbilityScript("Test Ability", AbilityType.Heal, 10, 0, 10, 0, 20));
        
    }

    // Update is called once per frame
    new void Update ()
	{
        if (Active != true) return;
        base.Update();

    }
}
