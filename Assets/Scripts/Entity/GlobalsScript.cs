using System.Collections;
using System.Collections.Generic;

public enum RarityType
{
    Common,
    Rare,
    Legendary,
    Epic,
    Special
}

public enum ItemType
{
    MeleeWeapon,
    RangedWeapon,
    Shield,
    Armor,
    Belt,
    Gloves,
    Boots,
    Ring,
    Amulet
}


public class GlobalsScript{

    public static int[] XPCurve = { 250, 1000, 2500, 5000, 10000, 25000, 100000 };

}
