using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : UnitScript {


	// Use this for initialization
    new void Start ()
	{
	    base.Start();
		Abilities.Add(new AbilityScript("Basic Attack", AbilityType.Basic, 2, 0, 0, 2));
        Abilities.Add(new AbilityScript("Test Ability", AbilityType.RangeShot, 5, 10, 0, 10));
    }

    // Update is called once per frame
    new void Update ()
	{
        base.Update();

        

	}
}
