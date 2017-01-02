using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : EntityScript
{

    public ItemType Type = ItemType.MeleeWeapon;

    public float Damage;
    public float CriticalChance;
    public float CriticalDamage;

    public RarityType Rarity;

    public int ScrapValue = 0;

}
