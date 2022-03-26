using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats :  PlayerMovement
{
    Animator anim;
    
    
    [Header("Competences")]
    public Passifs passif;
    public Skills[] skills;
    




    #region Getter and Setter

    #region Getter


    public Skills[] GetSkills()
    {
        return skills;
    }
    public Skills GetSkill1()
    {
        return skills[0];
    }
    public Skills GetSkill2()
    {
        return skills[1];
    }
    public Skills GetUlt()
    {
        return skills[2];
    }
    #endregion

    #endregion


    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
    }    

    public void AttackSystem()
    {
        if (Cible != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
                {
                    print("Hors d portée");
                }
                else
                {
                    if (attackType == AttackType.Melee)
                    {
                        StartCoroutine(AutoAttack());
                    }
                    if (attackType == AttackType.Ranged)
                    {
                        StartCoroutine(RangeAutoAttack());
                    }
                }
            }

        }
    }
    

    IEnumerator AutoAttack()
    {
        anim.SetBool("AA", true);
        yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
        if ( Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
        {
            anim.SetBool("AA", false);
        }
        
    }

    IEnumerator RangeAutoAttack()
    {
        //anim.SetBool("AA", true);
        RangeAttack();
        yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
        if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
        {
            //anim.SetBool("AA", false);
        }

    }

    public void MeleeAttack()
    {
        if(Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
        {
            if(Cible.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion)
            {
                Cible.GetComponent<EnnemyStats>().TakeDamage(DegatsPhysique, "Physique");
            }
        }
        else
        {
            anim.SetBool("AA", false);
        }
    }
    public void RangeAttack()
    {
        if (Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
        {
            if (Cible.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion)
            {
                SpawnRangeAttack(Targetable.EnemyType.minion, Cible);
            }
        }
        else
        {
            anim.SetBool("AA", false);
        }
    }

    public void SpawnRangeAttack(Targetable.EnemyType typeEnemy,GameObject Target)
    {
        float dmg = DegatsMagique;
        Instantiate(projPrefab, SpawnPrefab.transform.position, Quaternion.identity);
        if(typeEnemy == Targetable.EnemyType.minion)
        {
            projPrefab.GetComponent<Projectile>().degats = dmg;
            projPrefab.GetComponent<Projectile>().target = Target;
            projPrefab.GetComponent<Projectile>().targetSet = true;
        }
    }

    //IEnumerator CoolDown(Skills skill)
    //{
    //    yield return new WaitForSeconds(skill.Cooldown);
    //    Debug.Log("fin des cd");
    //    skill.isCooldown = false;
    //}

    //function de regen mana et vie

    public void Regen()
    {
        StartCoroutine(RegenHealAndMana());
    }

    IEnumerator RegenHealAndMana()
    {
        
            if (Health < MaxHealth)
            {
                float val = Mathf.FloorToInt(MaxHealth * 0.05f);
                Health += val;
                Debug.Log("+ " + val);
            }
        
            
        
        yield return new WaitForSeconds(1.5f);
        
    }

    //public void Passif()
    //{
    //    // test generique
    //    switch (lvl)
    //    {
    //        case 1:
    //            ResistanceMagique = ResistanceMagique * passif.Bonus;//augmentation 5%
    //            ResistancePhysique = ResistancePhysique * passif.Bonus;
    //            MoveSpeed = MoveSpeed * passif.Malus;//reduction 5%
    //            break;
    //        case 6:
    //            passif.Bonus = 1.075f;
    //            passif.Malus = 0.925f;
    //            ResistanceMagique = ResistanceMagique * passif.Bonus;//augmentation 7.5%
    //            ResistancePhysique = ResistancePhysique * passif.Bonus;
    //            MoveSpeed = MoveSpeed * passif.Malus;//reduction 7.5%
    //            break;
    //        case 12:
    //            passif.Bonus = 1.1f;
    //            passif.Malus = 0.8f;
    //            ResistanceMagique = ResistanceMagique * passif.Bonus;//augmentation 10%
    //            ResistancePhysique = ResistancePhysique * passif.Bonus;
    //            MoveSpeed = MoveSpeed * passif.Malus;//reduction s
    //            break;
    //        default:
    //            break;
    //    }


        
    //}

    //public void Skill1()
    //{
    //    if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
    //    {
    //        Mana -= skills[0].Cost;
    //        Debug.Log(skills[0].Name + " lancée");
    //        skills[0].isCooldown = true;
    //        if (skills[0].isCooldown == true)
    //        {
    //            StartCoroutine(CoolDown(skills[0]));
    //        }
    //    }
    //    else if (skills[0].isCooldown == true)
    //    {
    //        Debug.Log("en cd");
    //    }
    //    else if (Mana < skills[0].Cost)
    //    {
    //        Debug.Log("pas assez de mana");
    //    }
    //}

    //public void Skill2()
    //{
    //    if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
    //    {
    //        //buff
    //        Mana -= skills[1].Cost;
    //        Debug.Log(skills[1].Name + " lancée");
    //        StartCoroutine(Buff(skills[1]));
    //        skills[1].isCooldown = true;

    //        if (skills[1].isCooldown == true)
    //        {
    //            StartCoroutine(CoolDown(skills[1]));
    //        }
    //    }
    //    else if (skills[0].isCooldown == true)
    //    {
    //        Debug.Log("en cd");
    //    }
    //    else if (Mana < skills[0].Cost)
    //    {
    //        Debug.Log("pas assez de mana");
    //    }
    //}

    //IEnumerator Buff(Skills skill)
    //{
    //    //while(Time.deltaTime != skill.CastTime)
    //    //{
    //    //    ResistanceMagique = ResistanceMagique * 1.25f;
    //    //}

    //    yield return new WaitForSeconds(skill.Cooldown);
    //    Debug.Log("fin des cd");
    //    skill.isCooldown = false;
    //}

    //public void Ultime()
    //{
    //    Debug.Log("ULT");
    //}

    //public void Eveil()
    //{
    //    throw new System.NotImplementedException();
    //}
    public void TakeDamage(float DegatsRecu, string type)
    {
        //application des res, a modifier pour les differents type de degats
        if (type == "Physique")
        {
            Health = Health - (DegatsRecu - ((ResistancePhysique * DegatsRecu) / 100)); // physique
        }
        else if (type == "Magique")
        {
            Health = Health - (DegatsRecu - ((ResistanceMagique * DegatsRecu) / 100)); // magique
        }
        else if (type == "Brut")
        {
            Health -= DegatsRecu;
        }



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
