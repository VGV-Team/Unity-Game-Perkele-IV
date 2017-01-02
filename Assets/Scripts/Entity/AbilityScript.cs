using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public enum AbilityType
{
    None,
    BasicAttack,
    RangeAttack,
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
    public Sprite ImageToShow;

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
        this.ImageToShow = null;
    }

    public AbilityScript(string name, AbilityType type , double cooldown, int furyRequired, int manaRequired, double range, int basePower, Sprite imageToShow = null)
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
        this.ImageToShow = imageToShow;

    }

    public AbilityScript(string name, AbilityType type, double cooldown, int furyRequired, int manaRequired, int HPRequired, int shieldRequired, double range, int basePower, Sprite imageToShow = null)
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
        this.ImageToShow = imageToShow;
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
            (target == null || Range == 0 || Vector3.Distance(caster.transform.position, target.transform.position) <= Range)
        )
        {
            return true;
        }
        else return false;
    }

    private void LookAtTarget(GameObject caster, GameObject target)
    {
        if (target == null) return;
        caster.transform.LookAt(target.transform.position);
        caster.transform.rotation = Quaternion.Euler(0, caster.transform.eulerAngles.y, 0);
    }

    public bool Use(GameObject caster, GameObject target = null)
    {
        if (CanUseAbility(caster, target))
        {
            //Temporary, move this to approprita ability types
            LookAtTarget(caster, target);
            //caster.GetComponent<UnitScript>().StopMovement();

            switch (Type)
            {
                case AbilityType.BasicAttack:
                    if (target == null) return false;
                    AbilityTypeBasic(caster, target);
                    break;
                case AbilityType.RangeAttack:
                    if (target == null) return false;
                    AbilityTypeRange(caster, target);
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
        else return false;

        return true;
    }


    private void AbilityTypeBasic(GameObject caster, GameObject target)
    {
        caster.GetComponent<UnitScript>().StartBasicAttackAnimation((float) this.Cooldown);
        target.GetComponent<UnitScript>().StartHitAnimation();

        int hp = target.GetComponent<UnitScript>().HP;
        int shield = target.GetComponent<UnitScript>().Shield;

        int casterStrength = caster.GetComponent<UnitScript>().Strength;

        //TODO: monster armor is not takein into account?
        if (shield < BasePower + casterStrength)
        {
            hp -= (BasePower + casterStrength - shield);
            shield = 0;
            if (hp <= 0)
            {
                caster.GetComponent<UnitScript>().Target = null;
                target.GetComponent<UnitScript>().Active = false;
                target.GetComponent<UnitScript>().Die();


                // Loot drops and XP
                if (caster.tag == "Player")
                {

                    // Give XP to caster
                    caster.GetComponent<UnitScript>().Xp += target.GetComponent<UnitScript>().Xp;

                    // Loot drops
                    int numItems = Random.Range(1, 3);

                    for (int i = 0; i < numItems; i++)
                    {
                        GameObject item = GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().GenerateRandomItem();
                        item = GameObject.Instantiate(item);

                        item.transform.position = target.transform.position;
                        item.transform.position += new Vector3(0, 3, 0);
                        item.GetComponent<ItemScript>().Name += " " + Random.Range(1000, 5555);
                        item.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-5, 5), Random.Range(1, 5), Random.Range(-5, 5));
                    }
                }

            }
        }
        else shield -= (BasePower + casterStrength);
        target.GetComponent<UnitScript>().HP = hp;
        target.GetComponent<UnitScript>().Shield = shield;
    }

    private void AbilityTypeRange(GameObject caster, GameObject target)
    {
        caster.GetComponent<UnitScript>().StartRangeAttackAnimation();
        target.GetComponent<UnitScript>().StartHitAnimation();

        int hp = target.GetComponent<UnitScript>().HP;
        int shield = target.GetComponent<UnitScript>().Shield;

        int casterStrength = caster.GetComponent<UnitScript>().Strength;

        if (shield < BasePower + casterStrength)
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
        else shield -= (BasePower + casterStrength);
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
