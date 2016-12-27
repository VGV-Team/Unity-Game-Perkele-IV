using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum AbilityType
{
    None,
    Basic,
    RangeShot
}

public class AbilityScript
{

    public string Name;
    public AbilityType Type;
    public double Cooldown;
    public double TimeToReady;
    public int FuryRequired;
    public int ManaRequired;
    public int HPRequired;
    public int ShieldRequired;
    public double Range;

    public AbilityScript()
    {
        this.Name = "";
        this.Type = AbilityType.None;
        this.Cooldown = 0;
        this.TimeToReady = 0;
        this.FuryRequired = 0;
        this.ManaRequired = 0;
        this.HPRequired = 0;
        this.ShieldRequired = 0;
        this.Range = 0;

    }

    public AbilityScript(string name, AbilityType type , double cooldown, int furyRequired, int manaRequired, double range)
    {
        this.Name = name;
        this.Type = type;
        this.Cooldown = cooldown;
        this.TimeToReady = 0;
        this.FuryRequired = furyRequired;
        this.ManaRequired = manaRequired;
        this.HPRequired = 0;
        this.ShieldRequired = 0;
        this.Range = range;

    }

    public AbilityScript(string name, AbilityType type, double cooldown, int furyRequired, int manaRequired, int HPRequired, int shieldRequired, double range)
    {
        this.Name = name;
        this.Type = type;
        this.Cooldown = cooldown;
        this.TimeToReady = 0;
        this.FuryRequired = furyRequired;
        this.ManaRequired = manaRequired;
        this.HPRequired = HPRequired;
        this.ShieldRequired = shieldRequired;
        this.Range = range;

    }
    
    public void Update()
    {
        if (TimeToReady > 0) TimeToReady -= Time.deltaTime;
        if (TimeToReady < 0) TimeToReady = 0;
    }

    public bool Use()
    {
        if (TimeToReady > 0) return false;

        TimeToReady = Cooldown;
        return true;
    }
}
