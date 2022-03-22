using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermaidBehaviour : PlayerStats
{
    private int _passiveCounter;
    public GameObject poissoin;
    private float poissoinDmg;

    public void Start()
    {
        _passiveCounter = 0;
    }

    public void Update()
    {
        Passif();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
            {
                Skill1(Targetable.EnemyType.minion, Cible);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Skill2();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && canUlt == true)
        {
            Ultime();
        }
    }

    public void Passif()
    {
       if(_passiveCounter >= 3)
        {

            _passiveCounter = 0;
        }
    }

    public void AddPassive()
    {
        _passiveCounter++;
    }

    public void Skill1(Targetable.EnemyType typeEnemy, GameObject Target)
    {
        if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
        {
            Mana -= skills[0].Cost;
            Debug.Log(skills[0].Name + " lancée");
            skills[0].isCooldown = true;
            if (skills[0].isCooldown == true)
            {
                //TODO
                float dmg = poissoinDmg;
                Instantiate(projPrefab, SpawnPrefab.transform.position, Quaternion.identity);
                if (typeEnemy == Targetable.EnemyType.minion)
                {
                    projPrefab.GetComponent<Projectile>().degats = dmg;
                    projPrefab.GetComponent<Projectile>().target = Target;
                    projPrefab.GetComponent<Projectile>().targetSet = true;
                }
                StartCoroutine(CoolDown(skills[0]));
            }
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

    public void Skill2()
    {
        if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
        {
            //buff
            Mana -= skills[1].Cost;
            Debug.Log(skills[1].Name + " lancée");
            StartCoroutine(Buff(skills[1]));
            skills[1].isCooldown = true;

            if (skills[1].isCooldown == true)
            {
                StartCoroutine(CoolDown(skills[1]));
            }
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
        Debug.Log("ULT");
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
