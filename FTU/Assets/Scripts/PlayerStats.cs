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

    public void WalkToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, Cible.transform.position, GetMoveSpeed() * Time.deltaTime);
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
            var test = GetAttackSpeed() / ((100 / + GetAttackSpeed()) * 0.01f);
            yield return new WaitForSeconds(test);
            //MeleeAttack();
            if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() || 
                Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 5 && isAI)
            {
                try
                {
                    if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
                    {
                        Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique);
                    }
                }
                catch(NullReferenceException e)
                {
                    Cible = null;
                }
            }
            else
            {
                //anim.SetBool("AA", false);
            }
            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / +GetAttackSpeed()) * 0.01f));
            if ( Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
            {
                //anim.SetBool("AA", false);
                Debug.Log("AA");
            }
        }
        
    }

    public IEnumerator RangeAutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            yield return new WaitForSeconds(GetAttackSpeed() + ((100 / GetAttackSpeed()) * 0.01f));
            //RangeAttack();
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
            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / GetAttackSpeed()) * 0.01f));
            if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
            {
                Debug.Log("stop aa");
                //anim.SetBool("AA", false);
            }
        }

    }

    public void MeleeAttack()
    {
        if(Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() || Cible != null && isAI)
        {
            if(IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
            {
                Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique);
            }
        }
        else
        {
            //anim.SetBool("AA", false);
        }
    }
    public void RangeAttack()
    {
        if (Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
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
    }

    public void SpawnRangeAttack(GameObject Target, float dmgSupp = 0)
    {
        PhotonNetwork.Instantiate(projPrefab.name, transform.position, Quaternion.identity);

        projPrefab.GetComponent<Projectile>().SetDamages(GetDegMag() + dmgSupp, DamageType.magique);
        projPrefab.GetComponent<Projectile>().target = Target;
        projPrefab.GetComponent<Projectile>().targetSet = true;
    }

    public float DamageMultiplier(float dmgSource, float dmgMultiplier)
    {
        var res = dmgSource * dmgMultiplier;
        return res;
    }   

    //public void Passif()
    //{
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
    //        if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
    //        {
    //            Mana -= skills[0].Cost;
    //            Debug.Log(skills[0].Name + " lanc�e");

    //            StartCoroutine(skill1());
            
    //            skills[0].isCooldown = true;
    //            if (skills[0].isCooldown == true)
    //            {
    //                StartCoroutine(CoolDown(skills[0]));
    //            }
    //        }
    //        else if (skills[0].isCooldown == true)
    //        {
    //            //Debug.Log("en cd");
    //        }
    //        else if (Mana < skills[0].Cost)
    //        {
    //            Debug.Log("pas assez de mana");
    //        }
    //}

    //IEnumerator skill1()
    //{
    //    GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //    sp.GetComponent<Transform>().localScale *= 2.5f;
    //    GameObject skill1 = Instantiate(sp, SpawnPrefab2.transform.position, Quaternion.identity);
    //    Destroy(sp);
    //    Collider[] hitColliders = Physics.OverlapSphere(skill1.transform.position,2f);
    //    foreach (var hitCollider in hitColliders)
    //    {
    //        //Debug.Log("<color=green> touch: </color>" + hitCollider.name);
    //        try
    //        {

    //            if (hitCollider.TryGetComponent(typeof(IDamageable), out Component component))
    //            {
    //                if (IsTargetable(hitCollider.GetComponent<IDamageable>().GetEnemyType()))
    //                {
    //                    hitCollider.GetComponent<IDamageable>().TakeDamage(skills[0].Damage, skills[0].degats);
    //                    hitCollider.GetComponent<IDamageable>().TakeCC(ControlType.stun,1.5f);
    //                    Debug.Log("<color=green> damage: </color>" + skills[0].Damage+" "+ skills[0].degats);
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            print("r");
    //        }

    //    }
    //    yield return new WaitForSeconds(skills[0].CastTime);
    //    Destroy(skill1);
        
    //}

    //public void Skill2()
    //{
    //    if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
    //    {
    //        Mana -= skills[1].Cost;
    //        Debug.Log(skills[1].Name + " lancee");
    //        StartCoroutine(skill2());
    //        skills[1].isCooldown = true;
    //        if (skills[1].isCooldown == true)
    //        {
    //            StartCoroutine(CoolDown(skills[1]));
    //        }
    //    }
    //    else if (skills[1].isCooldown == true)
    //    {
    //        Debug.Log("en cd");
    //    }
    //    else if (Mana < skills[1].Cost)
    //    {
    //        Debug.Log("pas assez de mana");
    //    }
    //}

    //IEnumerator skill2()
    //{
    //    ResistanceMagique *= 1.1f;
    //    ResistancePhysique *= 1.1f;
    //    yield return new WaitForSeconds(skills[1].CastTime * 2);
    //    ResistanceMagique *= .8f;
    //    ResistancePhysique *= .8f;
    //}
    //public void Ultime()
    //{
    //    if (skills[2].isCooldown == false && Mana >= skills[2].Cost)
    //    {
    //        Mana -= skills[2].Cost;
    //        Debug.Log(skills[2].Name + " lanc�e");

    //        //StartCoroutine(UltEffect());
    //        skills[2].isCooldown = true;
    //        if (skills[2].isCooldown == true)
    //        {
    //            StartCoroutine(CoolDown(skills[2]));
    //        }
    //    }
    //    else if (skills[2].isCooldown == true)
    //    {
    //        Debug.Log("en cd");
    //    }
    //    else if (Mana < skills[2].Cost)
    //    {
    //        Debug.Log("pas assez de mana");
    //    }
    //}
    //IEnumerator UltEffect()
    //{
    //    float baseHealth = Health;

    //    ResistanceMagique += 45;
    //    ResistancePhysique += 45f;
    //    yield return new WaitForSeconds(skills[2].CastTime);
    //    Transform holder = GameObject.Find("Ult Rangeholder").transform;
    //    GameObject ultime = Instantiate(ult, holder.position, Quaternion.identity);
    //    ResistanceMagique -= 45;
    //    ResistancePhysique -= 45;
    //    float endHealth = baseHealth - Health;
    //    Debug.Log("<color=blue>Endhealth full: </color>" + endHealth);
    //    float fulldmg = ultime.GetComponent<Projectile>().degats = (endHealth * 10) / 100;
    //    skills[2].Damage += fulldmg;
    //    Collider[] hitColliders = Physics.OverlapSphere(ultime.transform.position, 1.5f);
    //    foreach (var hitCollider in hitColliders)
    //    {
    //        //Debug.Log("<color=green> touch: </color>" + hitCollider.name);
    //        try
    //        {

    //            if (hitCollider.TryGetComponent(typeof(IDamageable), out Component component))
    //            {
    //                if (IsTargetable(hitCollider.GetComponent<IDamageable>().GetEnemyType()))
    //                {
    //                    hitCollider.GetComponent<IDamageable>().TakeDamage(skills[2].Damage, skills[2].degats);
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            print("r");
    //        }

    //    }
    //    //Debug.Log("<color=yellow>Endhealth 10%: </color>" + fulldmg);
    //    yield return new WaitForSeconds(.75f);
    //    Destroy(ultime);

    //    //Debug.Log("<color=red> full damage: </color>" + skills[2].Damage + " inflig�");
    //    yield return new WaitForSeconds(.05f);
    //    skills[2].Damage -= fulldmg;
    //}    //public void Eveil()
    //{
    //    throw new System.NotImplementedException();
    //}


    
}
