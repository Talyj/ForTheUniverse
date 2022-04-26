using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats :  PlayerMovement
{
    Animator anim;
    
    
    [Header("Competences")]
    public Passifs passif;
    public Skills[] skills;

    //Damage
    public float damageSupp;
    




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
        while(Cible != null)
        {
            //anim.SetBool("AA", true);
            MeleeAttack();
            yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
            if ( Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
            {
                //anim.SetBool("AA", false);
            }
        }
        
    }

    IEnumerator RangeAutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            RangeAttack();
            yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
            if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
            {
                //anim.SetBool("AA", false);
            }
        }

    }

    public void MeleeAttack()
    {
        if(Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
        {
            if(IsTargetable(Cible.GetComponent<IDamageable>().enemytype))
            {
                Cible.GetComponent<IDamageable>().TakeDamage(DegatsPhysique + damageSupp, "Physique");
            }
        }
        else
        {
            //anim.SetBool("AA", false);
        }
    }
    public void RangeAttack()
    {
        if (Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
        {
            if (IsTargetable(Cible.GetComponent<IDamageable>().enemytype))
            {
                SpawnRangeAttack(EnemyType.minion, Cible, damageSupp);
            }
        }
        else
        {
            //anim.SetBool("AA", false);
        }
    }

    public void SpawnRangeAttack(EnemyType typeEnemy,GameObject Target, float dmgSupp = 0)
    {
        float dmg = DegatsMagique;
        Instantiate(projPrefab, SpawnPrefab.transform.position, Quaternion.identity);

        projPrefab.GetComponent<Projectile>().degats = dmg + dmgSupp;
        projPrefab.GetComponent<Projectile>().target = Target;
        projPrefab.GetComponent<Projectile>().targetSet = true;
    }

    public float DamageMultiplier(float dmgSource, float dmgMultiplier)
    {
        var res = dmgSource * dmgMultiplier;
        return res;
    }

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
