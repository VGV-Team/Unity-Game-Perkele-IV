using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum AbilityType
{
    None,
    BasicAttack,
    RangeShot,
    Heal
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

    public int BasePower;

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
        this.BasePower = 0;
    }

    public AbilityScript(string name, AbilityType type , double cooldown, int furyRequired, int manaRequired, double range, int basePower)
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
        this.BasePower = basePower;

    }

    public AbilityScript(string name, AbilityType type, double cooldown, int furyRequired, int manaRequired, int HPRequired, int shieldRequired, double range, int basePower)
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
        this.BasePower = basePower;
    }
    
    public void Update()
    {
        if (TimeToReady > 0) TimeToReady -= Time.deltaTime;
        if (TimeToReady < 0) TimeToReady = 0;
    }

    public bool CanUseAbility(GameObject caster, GameObject target)
    {
        if (TimeToReady <= 0 &&
            caster != null &&
            caster.GetComponent<UnitScript>().Fury >= FuryRequired &&
            caster.GetComponent<UnitScript>().Mana >= ManaRequired &&
            caster.GetComponent<UnitScript>().HP >= HPRequired &&
            caster.GetComponent<UnitScript>().Shield >= ShieldRequired &&
            (target == null || Vector3.Distance(caster.transform.position, target.transform.position) <= Range)
        )
        {
            return true;
        }
        else return false;
    }

    public void Use(GameObject caster, GameObject target = null)
    {
        if (CanUseAbility(caster, target))
        {
            switch (Type)
            {
                case AbilityType.BasicAttack:
                    if (target == null) return;
                    AbilityTypeBasic(caster, target);
                    break;
                case AbilityType.Heal:
                    AbilityTypeHeal(caster);
                    break;

            }

            caster.GetComponent<UnitScript>().Fury -= FuryRequired;
            caster.GetComponent<UnitScript>().Mana -= ManaRequired;
            caster.GetComponent<UnitScript>().HP -= HPRequired;
            caster.GetComponent<UnitScript>().Shield -= ShieldRequired;
            TimeToReady = Cooldown;
        }   
    }


    private void AbilityTypeBasic(GameObject caster, GameObject target)
    {
        caster.GetComponent<UnitScript>().StartBasicAttackAnimation((float) this.Cooldown);
        target.GetComponent<UnitScript>().StartHitAnimation();

        int hp = target.GetComponent<UnitScript>().HP;
        int shield = target.GetComponent<UnitScript>().Shield;

        int casterStrength = caster.GetComponent<UnitScript>().Strength;

        //TODO: monster armor is not takein into account?
        if (shield < BasePower)
        {
            hp -= (BasePower + casterStrength - shield);
            shield = 0;
            if (hp <= 0)
            {
                caster.GetComponent<UnitScript>().Target = null;
                target.GetComponent<UnitScript>().Active = false;
                target.GetComponent<UnitScript>().StartDeathAnimation();
            }
        }
        else shield -= BasePower;
        target.GetComponent<UnitScript>().HP = hp;
        target.GetComponent<UnitScript>().Shield = shield;
    }

    private void AbilityTypeHeal(GameObject caster)
    {
        caster.GetComponent<UnitScript>().StartHealAnimation();

        caster.GetComponent<UnitScript>().HP += BasePower;
        if (caster.GetComponent<UnitScript>().HP > caster.GetComponent<UnitScript>().MaxHP)
        {
            caster.GetComponent<UnitScript>().HP = caster.GetComponent<UnitScript>().MaxHP;
        }
        
    }
}
