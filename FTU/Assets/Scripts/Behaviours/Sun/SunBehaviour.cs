using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehaviour : PlayerStats
{
    public SunBehaviour Instance;
    private float dmgMultiplier;

    //Passif
    private enum Sticks
    {
        AuraStick = 0,
        SpiritSitck = 1
    }
    private Sticks currentStick;

    public new void Start()
    {
        currentStick = Sticks.AuraStick;
        canMove = true;
        Instance = this;
        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        Movement();
        HealthBehaviour();
        ExperienceBehaviour();
        AttackSystem();
        Passif();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Skill1(Cible);
        }
        if (Input.GetKeyDown(KeyCode.E) && Cible != null)
        {
            Skill2(Cible);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canUlt == true)
        {
            Ultime();
        }
    }

    public void Passif()
    {
        if(Cible != null)
        {
            switch (currentStick)
            {
                case Sticks.AuraStick:
                    damageSupp = DamageMultiplier(DegatsPhysique, 0.5f);
                    Debug.Log("Phys");
                    break;
                case Sticks.SpiritSitck:
                    damageSupp = DamageMultiplier(DegatsMagique, 0.5f);
                    Debug.Log("Mag");
                    break;
            }
        }
    }

    private void SwitchStick()
    {
        if(currentStick == Sticks.AuraStick)
        {
            currentStick = Sticks.SpiritSitck;
        }
        else
        {
            currentStick = Sticks.AuraStick;
        }
    }

    public void Skill1(GameObject target)
    {
        if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
        {
            Mana -= skills[0].Cost;
            Debug.Log(skills[0].Name + " lancée");
            skills[0].isCooldown = true;

            float dmg = DegatsMagique;

            SwitchStick();
            if(currentStick == Sticks.AuraStick)
            {
                //TODO Stun voister
            }
            else
            {
                //TODO Slow Champ
            }

            StartCoroutine(CoolDown(skills[0]));
        }
        else if (skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public void Skill2(GameObject target)
    {
        if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
        {
            //buff
            Mana -= skills[1].Cost;
            Debug.Log(skills[1].Name + " lancée");

            Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            Vector3 direction = target.transform.position - transform.position;

            float dmg = DegatsMagique;

            //TODO

            StartCoroutine(CoolDown(skills[1]));
        }
        else if (skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    IEnumerator Buff(Skills skill)
    {
        //while(Time.deltaTime != skill.CastTime)
        //{
        //    ResistanceMagique = ResistanceMagique * 1.25f;
        //}

        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
        skill.isCooldown = false;
    }

    public void Ultime()
    {
        if (skills[2].isCooldown == false && Mana >= skills[2].Cost)
        {
            //buff
            Mana -= skills[2].Cost;
            Debug.Log(skills[2].Name + " lancée");
            //TODO
            float dmg = DegatsMagique;

            //TODO

            StartCoroutine(CoolDown(skills[2]));
        }
        else if (skills[2].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < skills[2].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public void Eveil()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator CoolDown(Skills skill)
    {
        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
        skill.isCooldown = false;
    }
}
