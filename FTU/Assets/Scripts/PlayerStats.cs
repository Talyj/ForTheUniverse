using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : PlayerMovement
{
    //Animator anim;

    //Path Auto
    public Transform[] targetsUp;
    public Transform[] targetsDown;
    private Transform[] tempArray;
    public Way way;
    public bool isAttacking;

    public enum Way
    {
        up,
        down
    }

    //[SerializeField]
    //ControlType cc;
    [Header("Competences")]
    public Passifs passif;
    public Skills[] skills;
    //public bool canMove = true;
    //public bool useSkills = true;//pour les cc
    
    //Damage
    public float damageSupp;
    public GameObject ult;

    public bool isAI;
    //public EnemyType enemyType;


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
    void Start()
    {
        //anim = GetComponent<Animator>();
        for(int i =0;i< skills.Length; i++)
        {
            skills[i].isCooldown = false;
        }
    }

    public void AttackSystemAI()
    {
        WalkToward();
        if (attackType == AttackType.Melee)
        {
            StartCoroutine(AutoAttack());
        }
        if (attackType == AttackType.Ranged)
        {
            StartCoroutine(RangeAutoAttack());
        }
    }

    public void DefaultMinionBehaviour()
    {
        if (Cible == null)
        {
            isAttacking = false;
        }

        if (!isAttacking && Cible != null)
        {
            isAttacking = true;
            AttackSystemAI();
        }

        if (!pathDone && !isAttacking && Cible == null)
        {
            if (way == Way.up)
            {
                MovementAI(whichTeam(targetsUp));
            }
            else MovementAI(whichTeam(targetsDown));
        }
    }

    public void DefaultHeroBehaviourAI()
    {
        if (Cible == null)
        {
            isAttacking = false;
        }

        if (!isAttacking && Cible != null)
        {
            isAttacking = true;
        }

        if (!pathDone && !isAttacking && Cible == null)
        {
            if (way == Way.up)
            {
                MovementAI(whichTeam(targetsUp));
            }
            else MovementAI(whichTeam(targetsDown));
        }
    }   

    public Transform[] whichTeam(Transform[] way)
    {
        tempArray = new Transform[way.Length];
        if (team == Team.Veritas)
        {
            for (int i = 0; i < way.Length; i++)
            {
                tempArray[way.Length - 1 - i] = way[i];
            }
            return tempArray;
        }
        return way;
    }


    public IEnumerator AutoAttack()
    {
        while(Cible != null)
        {
            //anim.SetBool("AA", true);
            try
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() || 
                    Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 5 && isAI)
                {
                    if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
                    {
                        Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique);
                    }
                }
                else
                {
                    //anim.SetBool("AA", false);
                }  
                
                if ( Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
                {
                    //anim.SetBool("AA", false);
                }
            }
            catch(NullReferenceException e)
            {
                Cible = null;
            }
            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / +GetAttackSpeed()) * 0.01f));
        }
        
    }

    public IEnumerator RangeAutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            try
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() || 
                    Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 5 && isAI)
                {
                    if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
                    {
                        SpawnRangeAttack(Cible, damageSupp);
                    }
                }
                else
                {
                    //anim.SetBool("AA", false);
                }
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
                {
                    //anim.SetBool("AA", false);
                }
            }
            catch(NullReferenceException e)
            {
                Cible = null;
            }
            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / GetAttackSpeed()) * 0.01f));
        }

    }

    //public void MeleeAttack()
    //{
    //    if(Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() || Cible != null && isAI)
    //    {
    //        if(IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
    //        {
    //            Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique);
    //        }
    //    }
    //    else
    //    {
    //        //anim.SetBool("AA", false);
    //    }
    //}

    //public void RangeAttack()
    //{
    //    if (Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
    //    {
    //        if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
    //        {
    //            SpawnRangeAttack(Cible, damageSupp);
    //        }
    //    }
    //    else
    //    {
    //        //anim.SetBool("AA", false);
    //    }
    //}

    public void SpawnRangeAttack(GameObject Target, float dmgSupp = 0)
    {
        var bullets = PhotonNetwork.Instantiate(projPrefab.name, transform.position, Quaternion.identity);

        bullets.GetComponent<Projectile>().SetDamages(GetDegMag() + dmgSupp, DamageType.magique);
        bullets.GetComponent<Projectile>().target = Target;
        bullets.GetComponent<Projectile>().targetSet = true;
    }

    public float DamageMultiplier(float dmgSource, float dmgMultiplier)
    {
        var res = dmgSource * dmgMultiplier;
        return res;
    }   
}
