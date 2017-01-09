using DigitalRuby.PyroParticles;
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
    Fireball,
    FireExplosion,
    Flamethrower,
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

	public string Description;

    public int BasePower;

    private float CasterAttackSpeed;

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
        if (this.Type == AbilityType.BasicAttack)
        {
            if (TimeToReady < 0) TimeToReady = 0;
            else TimeToReady -= Time.deltaTime * (CasterAttackSpeed / 100.0f);  
        }
        else
        {
            if (TimeToReady > 0) TimeToReady -= Time.deltaTime;
            if (TimeToReady < 0) TimeToReady = 0;
        }
       
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
        //Debug.Log("Trying to use ability: " + Name + " type: " + Type);

        if (CanUseAbility(caster, target))
        {
            //Temporary, move this to approprita ability types
            LookAtTarget(caster, target);
            

            switch (Type)
            {
                case AbilityType.BasicAttack:
                    if (target == null) return false;
                    CasterAttackSpeed = caster.GetComponent<UnitScript>().AttackSpeed;
                    AbilityTypeBasic(caster, target);
                    break;
                case AbilityType.RangeAttack:
                    // TODO temporary?
                    if (target == null) return false;
                    AbilityTypeRange(caster, target);
                    break;
                case AbilityType.Fireball:
                    caster.GetComponent<UnitScript>().StopMovement();
                    AbilityTypeFireball(caster);
                    break;
                case AbilityType.FireExplosion:
                    caster.GetComponent<UnitScript>().StopMovement();
                    AbilityTypeFireExplosion(caster);
                    break;
                case AbilityType.Flamethrower:
                    caster.GetComponent<UnitScript>().StopMovement();
                    AbilityTypeFlamethrower(caster);
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

    public void AbilityImpact(GameObject caster, GameObject target)
    {
        Debug.Log(Name);
        switch (Type)
        {
            case AbilityType.BasicAttack:
                if (target == null) return;
                AbilityTypeBasicImpact(caster, target);
                break;
            case AbilityType.RangeAttack:
                if (target == null) return;
                AbilityTypeRangeImpact(caster, target);
                break;
            case AbilityType.Fireball:
                AbilityTypeFireballCast(caster);
                break;
            case AbilityType.Heal:
                AbilityTypeHealImpact(caster);
                break;
        }
    }

    private void AbilityTypeFlamethrower(GameObject caster)
    {
        AbilityTypeFlamethrowerCast(caster);
    }
    private void AbilityTypeFlamethrowerCast(GameObject caster)
    {
        Debug.Log("Casting flamethrower!");

        Vector3 targetPosition, sourcePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {

            targetPosition = hit.point;
            sourcePosition = caster.transform.position;
            targetPosition.y = sourcePosition.y;

            caster.transform.LookAt(targetPosition);


            //Finally cast fireball in the direction of source to target
            GameObject flame = GameObject.Instantiate(GameObject.Find("EffectLoader").GetComponent<EffectLoaderScript>().FlamethrowerEffect);
            Vector3 pos = caster.transform.position;
            pos.y += 2.0f;

            Vector3 direction = (targetPosition - sourcePosition).normalized;
            flame.transform.parent = caster.transform;
            flame.transform.position = Vector3.zero;
            flame.transform.localPosition = Vector3.zero;
            //flame.transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));

            flame.GetComponent<FireBaseScript>().Ability = this;
            flame.GetComponent<FireBaseScript>().Caster = caster;
            flame.GetComponent<FlamethrowerCollisionScript>().Ability = this;
            flame.GetComponent<FlamethrowerCollisionScript>().Caster = caster;
            flame.transform.position = pos + direction.normalized/2;
        }
    }
    public void AbilityTypeFlamethrowerImpact(GameObject caster, GameObject target)
    {
        Debug.Log("FLAMETHROWER IMPACT");
        if (target.tag == "Enemy" || target.tag == "Player")
        {
            float hp = target.GetComponent<UnitScript>().HP;
            float shield = target.GetComponent<UnitScript>().Shield;

            float damage = BasePower * Time.deltaTime;

            //Armor
            float armor = target.GetComponent<UnitScript>().Armor;
            if (target.tag == "Player")
            {
                if (target.GetComponent<UnitScript>().EquippedItems.ShieldSlot)
                    armor += target.GetComponent<UnitScript>().EquippedItems.ShieldSlot.GetComponent<ItemScript>().Armor;
            }
            while (armor >= 100)
            {
                armor -= 100;
                damage *= 0.5f;
            }
            damage -= ((damage / 2) * (armor / 100));


            //TODO: monster armor is not taken into account?
            if (shield < damage)
            {
                hp -= (damage - shield); // no casterstrength
                shield = 0;
                if (hp <= 0)
                {
                    if (target.name == "Crate")
                        target.GetComponent<CrateScript>().DestroyCrate();
                    else
                    {
                        caster.GetComponent<UnitScript>().Target = null;
                        target.GetComponent<UnitScript>().Active = false;
                        target.GetComponent<UnitScript>().Die();



                        // Loot drops and XP
                        if (caster.tag == "Player")
                        {

                            // Give XP to caster
                            caster.GetComponent<UnitScript>().Xp += target.GetComponent<UnitScript>().XPWorth;



                            GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().LootDrop(
                                caster,
                                0,
                                50,
                                30,
                                20,
                                target.transform);
                         }


                    }
                }
            }
            else shield -= (damage);
            target.GetComponent<UnitScript>().HP = hp;
            target.GetComponent<UnitScript>().Shield = shield;
        }

    }

    private void AbilityTypeFireExplosion(GameObject caster)
    {
        AbilityTypeFireExplosionCast(caster);
    }
    private void AbilityTypeFireExplosionCast(GameObject caster)
    {
        //Particle effect
        Debug.Log("Casting Fire explosion!");
        GameObject explosionEffect = GameObject.Instantiate(GameObject.Find("EffectLoader").GetComponent<EffectLoaderScript>().FireExplosionEffect);
        explosionEffect.transform.parent = caster.transform;
        explosionEffect.transform.position = Vector3.zero;
        explosionEffect.transform.localPosition = Vector3.zero;
        explosionEffect.GetComponent<FireBaseScript>().Caster = caster;
        explosionEffect.GetComponent<FireBaseScript>().Ability = this;
    }
    public void AbilityTypeFireExplosionImpact(GameObject caster, Collider[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if ((caster.tag != "Player" && objects[i].gameObject.tag == "Player") ||  //if enemy used ability (no friendly fire)
                (caster.tag == "Player" && objects[i].gameObject.tag == "Enemy") ) //if player used ability and hit an enemy
            {

                GameObject target = objects[i].gameObject;

                float hp = target.GetComponent<UnitScript>().HP;
                float shield = target.GetComponent<UnitScript>().Shield;

                float casterStrength = caster.GetComponent<UnitScript>().Strength;

                float damage = BasePower + casterStrength;

                //Armor
                float armor = target.GetComponent<UnitScript>().Armor;
                if (target.tag == "Player")
                {
                    if (target.GetComponent<UnitScript>().EquippedItems.ShieldSlot)
                        armor += target.GetComponent<UnitScript>().EquippedItems.ShieldSlot.GetComponent<ItemScript>().Armor;
                }
                while (armor >= 100)
                {
                    armor -= 100;
                    damage *= 0.5f;
                }
                damage -= ((damage / 2) * (armor / 100));

                //TODO: monster armor is not taken into account?
                if (shield < damage)
                {
                    hp -= (damage - shield);
                    shield = 0;
                    if (hp <= 0)
                    {
                        if (target.name == "Crate")
                            target.GetComponent<CrateScript>().DestroyCrate();
                        else
                        {
                            caster.GetComponent<UnitScript>().Target = null;
                            target.GetComponent<UnitScript>().Active = false;
                            target.GetComponent<UnitScript>().Die();



                            // Loot drops and XP
                            if (caster.tag == "Player")
                            {

                                // Give XP to caster
                                caster.GetComponent<UnitScript>().Xp += target.GetComponent<UnitScript>().XPWorth;



                            GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().LootDrop(
                                caster,
                                0,
                                50,
                                30,
                                20,
                                target.transform);
                        }


                        }
                    }
                }
                else shield -= (damage);
                target.GetComponent<UnitScript>().HP = hp;
                target.GetComponent<UnitScript>().Shield = shield;
            }
        }
    }

    private void AbilityTypeFireball(GameObject caster)
    {
        caster.GetComponent<UnitScript>().StartFireballAnimation();
    }
    private void AbilityTypeFireballCast(GameObject caster)
    {
        Debug.Log("Casting fireball!");

        Vector3 targetPosition, sourcePosition;
        GameObject fireball;

        if (caster.tag == "Player")
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {

                targetPosition = hit.point;
                sourcePosition = caster.transform.position;
                targetPosition.y = sourcePosition.y;
                fireball = GameObject.Instantiate(GameObject.Find("EffectLoader").GetComponent<EffectLoaderScript>().FireballEffect);
            }
            else
                return;
        }
        else
        {
            GameObject Target = caster.GetComponent<UnitScript>().Target;
            if (Target == null) return;
            targetPosition = Target.transform.position;
            sourcePosition = caster.transform.position;
            targetPosition.y = sourcePosition.y;
            fireball = GameObject.Instantiate(GameObject.Find("EffectLoader").GetComponent<EffectLoaderScript>().FireballEnemyEffect);
        }

        caster.transform.LookAt(targetPosition);

        //Finally cast fireball in the direction of source to target
        
        Vector3 pos = caster.transform.position;
        pos.y += 1.0f;
        Vector3 direction = (targetPosition - sourcePosition).normalized;
        fireball.GetComponent<FireProjectileScript>().ProjectileDirection = direction;
        fireball.GetComponent<FireProjectileScript>().Ability = this;
        fireball.GetComponent<FireProjectileScript>().Caster = caster;
        fireball.transform.position = pos + direction.normalized * 2;
        fireball.transform.FindChild("FireboltCollider").GetComponent<Collider>().enabled = true;


    }
    public void AbilityTypeFireballImpact(GameObject target, GameObject caster)
    {
        if (target == null) return;
        if (caster.tag == "Enemy" && target.tag == "Enemy") return; //no friendly fire
        if (target.tag == "Enemy" || target.tag == "Player")
        {
            float hp = target.GetComponent<UnitScript>().HP;
            float shield = target.GetComponent<UnitScript>().Shield;

            float casterStrength = caster.GetComponent<UnitScript>().Strength;


            float damage = casterStrength + BasePower;

            //Armor
            float armor = target.GetComponent<UnitScript>().Armor;
            if (target.tag == "Player")
            {
                if (target.GetComponent<UnitScript>().EquippedItems.ShieldSlot)
                    armor += target.GetComponent<UnitScript>().EquippedItems.ShieldSlot.GetComponent<ItemScript>().Armor;
            }
            while (armor >= 100)
            {
                armor -= 100;
                damage *= 0.5f;
            }
            damage -= ((damage / 2) * (armor / 100));


            //TODO: monster armor is not taken into account?
            if (shield < damage)
            {
                hp -= (damage - shield);
                shield = 0;
                if (hp <= 0)
                {
                    if (target.name == "Crate")
                        target.GetComponent<CrateScript>().DestroyCrate();
                    else
                    {
                        caster.GetComponent<UnitScript>().Target = null;
                        target.GetComponent<UnitScript>().Active = false;
                        target.GetComponent<UnitScript>().Die();
                        // Loot drops and XP
                        if (caster.tag == "Player")
                        {
                            // Give XP to caster
                            caster.GetComponent<UnitScript>().Xp += target.GetComponent<UnitScript>().XPWorth;
                            GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().LootDrop(
                                caster,
                                0,
                                50,
                                30,
                                20,
                                target.transform);
                        }
                    }
                }
                else if (target.tag == "Enemy")
                {
                    //target.GetComponent<UnitScript>().Target = caster;
                    target.GetComponent<UnitScript>().SetWaypoint(caster.transform.position);
                }
            }
            else shield -= (damage); 

            target.GetComponent<UnitScript>().HP = hp;
            target.GetComponent<UnitScript>().Shield = shield;
        }
    }

    private void AbilityTypeBasic(GameObject caster, GameObject target)
   { 
        caster.GetComponent<UnitScript>().StartBasicAttackAnimation((float) this.Cooldown);
        target.GetComponent<UnitScript>().StartHitAnimation();
    }
    public void AbilityTypeBasicImpact(GameObject caster, GameObject target)
    {
		float hp = target.GetComponent<UnitScript>().HP;
		float shield = target.GetComponent<UnitScript>().Shield;

		float casterStrength = caster.GetComponent<UnitScript>().Strength;

        if (Vector3.Distance(caster.transform.position, target.transform.position) > Range)
        {
            // Target too far away - miss
            Debug.Log(caster.name + ": Basic attack MISS!");
            return;
        }

        float damage = BasePower + casterStrength;
        float criticalChance = caster.GetComponent<UnitScript>().CriticalChance;

        //If player, check items
        if (caster.tag == "Player")
        {
            if (caster.GetComponent<UnitScript>().EquippedItems.WeaponSlot)
            {
                damage += caster.GetComponent<UnitScript>().EquippedItems.WeaponSlot.GetComponent<ItemScript>().Damage;
                criticalChance += caster.GetComponent<UnitScript>().EquippedItems.WeaponSlot.GetComponent<ItemScript>().CriticalChance;
            }
        }

        //Armor
        float armor = target.GetComponent<UnitScript>().Armor;
        if (target.tag == "Player")
        {
            if (target.GetComponent<UnitScript>().EquippedItems.ShieldSlot)
                armor += target.GetComponent<UnitScript>().EquippedItems.ShieldSlot.GetComponent<ItemScript>().Armor;
        }
        while (armor >= 100)
        {
            armor -= 100;
            damage *= 0.5f;
        }
        damage -= ((damage / 2) * (armor / 100));

        int r = Random.Range(0, 100);
        if (criticalChance > r) damage *= 2;

        //TODO: monster armor is not takein into account?
        if (shield < damage)
        {
            hp -= (damage - shield);
            shield = 0;
            if (hp <= 0)
            {
                if (target.name == "Crate")
                    target.GetComponent<CrateScript>().DestroyCrate();
                else
                { 
                    caster.GetComponent<UnitScript>().Target = null;
                    target.GetComponent<UnitScript>().Active = false;
                    target.GetComponent<UnitScript>().Die();


                    // Loot drops and XP
                    if (caster.tag == "Player")
                    {

                        // Give XP to caster
                        caster.GetComponent<UnitScript>().Xp += target.GetComponent<UnitScript>().XPWorth;

                        GameObject.Find("ItemPool").GetComponent<ItemPoolScript>().LootDrop(
                            caster,
                            0,
                            50,
                            30,
                            20,
                            target.transform);
                    }
                }
            }
        }
        else shield -= (damage);
        target.GetComponent<UnitScript>().HP = hp;
        target.GetComponent<UnitScript>().Shield = shield;
    }


    private void AbilityTypeRange(GameObject caster, GameObject target)
    {
        Debug.Log("Using ranged ability: " + this.Name);
        caster.GetComponent<UnitScript>().StartRangeAttackAnimation();
        //target.GetComponent<UnitScript>().StartHitAnimation();
    }
    private void AbilityTypeRangeImpact(GameObject caster, GameObject target)
    {
        // NOT USED
        float hp = target.GetComponent<UnitScript>().HP;
		float shield = target.GetComponent<UnitScript>().Shield;

		float casterStrength = caster.GetComponent<UnitScript>().Strength;

        if (shield < BasePower + casterStrength)
        {
            hp -= (BasePower + casterStrength - shield);
            shield = 0;
            if (hp <= 0)
            {
                if (target.name == "Crate")
                    target.GetComponent<CrateScript>().DestroyCrate();
                else
                {
                    caster.GetComponent<UnitScript>().Target = null;
                    target.GetComponent<UnitScript>().Active = false;
                    target.GetComponent<UnitScript>().StartDeathAnimation();
                }
            }
        }
        else shield -= (BasePower + casterStrength);
        target.GetComponent<UnitScript>().HP = hp;
        target.GetComponent<UnitScript>().Shield = shield;
    }

    private void AbilityTypeHeal(GameObject caster)
    {
        AbilityTypeHealImpact(caster);
    }
    private void AbilityTypeHealImpact(GameObject caster)
    {
        caster.GetComponent<UnitScript>().HP += BasePower;
        if (caster.GetComponent<UnitScript>().HP > caster.GetComponent<UnitScript>().MaxHP)
        {
            caster.GetComponent<UnitScript>().HP = caster.GetComponent<UnitScript>().MaxHP;
        }

        //Particle effect
        GameObject healEffect = GameObject.Instantiate(GameObject.Find("EffectLoader").GetComponent<EffectLoaderScript>().HealEffect);
        healEffect.transform.parent = caster.transform;
        healEffect.transform.position = Vector3.zero;
        healEffect.transform.localPosition = Vector3.zero;

    }

}
