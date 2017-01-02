using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : EntityScript
{

    public ItemType Type = ItemType.Melee;

    public float Damage = 0;
    public float CriticalChance = 0;
    public float CriticalDamage = 0;

    public RarityType Rarity = RarityType.Common;

    public int ScrapValue = 0;

}
