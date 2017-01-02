using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Melee,
    Ranged,
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

    public static Color RarityToColor(RarityType? rarityType)
    {
        if (rarityType == RarityType.Common) return Color.grey;
        if (rarityType == RarityType.Rare) return Color.cyan;
        if (rarityType == RarityType.Legendary) return Color.magenta;
        if (rarityType == RarityType.Epic) return Color.yellow;
        if (rarityType == RarityType.Special) return Color.green;
        return Color.black;
    }
}
