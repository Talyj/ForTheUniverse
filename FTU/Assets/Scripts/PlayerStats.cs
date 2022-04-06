using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : IDamageable, ISkill
{
    //Animator anim;
    
    
    [Header("Competences")]
    public Passifs passif;
    public Skills[] skills;
    public bool canMove = true;
    //Damage
    public float damageSupp;
    public GameObject ult;




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
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        Passif();
    }
    // Update is called once per frame
    void Update()
    {

        if(Exp >= MaxExp)
        {
            ExperienceBehaviour();
        }


        //attack sys
        if (Cible != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
                {
                    print("Hors d port�e");
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
        // test des touches
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100, "Physique");

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Health = MaxHealth;
            Mana = MaxMana;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(100, "Magique");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(50, "Brut");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Exp = MaxExp + 1;

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Skill1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Skill2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && canUlt == true)
        {
            Ultime();
        }
    }


    IEnumerator AutoAttack()
    {
        while(Cible != null)
        {
            //anim.SetBool("AA", true);
            yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
            MeleeAttack();
            yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
            if ( Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
            {
                //anim.SetBool("AA", false);
                Debug.Log("AA");
            }
        }
        
    }

    IEnumerator RangeAutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            yield return new WaitForSeconds(AttackSpeed + ((100 / AttackSpeed) * 0.01f));
            RangeAttack();
            yield return new WaitForSeconds(AttackSpeed / ((100 / AttackSpeed) * 0.01f));
            if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
            {
                Debug.Log("stop aa");
                //anim.SetBool("AA", false);
            }
        }

    }

    public void MeleeAttack()
    {
        if(Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
        {
            if(IsTargetable(Cible.GetComponent<Targetable>().enemytype))
            {
                Cible.GetComponent<Targetable>().TakeDamage(DegatsPhysique + damageSupp, "Physique");
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
            if (IsTargetable(Cible.GetComponent<Targetable>().enemytype))
            {
                SpawnRangeAttack(Targetable.EnemyType.minion, Cible, damageSupp);
            }
        }
        else
        {
            //anim.SetBool("AA", false);
        }
    }

    public void SpawnRangeAttack(Targetable.EnemyType typeEnemy,GameObject Target, float dmgSupp = 0)
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


    public void Passif()
    {
        switch (lvl)
        {
            case 1:
                ResistanceMagique = ResistanceMagique * passif.Bonus;//augmentation 5%
                ResistancePhysique = ResistancePhysique * passif.Bonus;
                MoveSpeed = MoveSpeed * passif.Malus;//reduction 5%
                break;
            case 6:
                passif.Bonus = 1.075f;
                passif.Malus = 0.925f;
                ResistanceMagique = ResistanceMagique * passif.Bonus;//augmentation 7.5%
                ResistancePhysique = ResistancePhysique * passif.Bonus;
                MoveSpeed = MoveSpeed * passif.Malus;//reduction 7.5%
                break;
            case 12:
                passif.Bonus = 1.1f;
                passif.Malus = 0.8f;
                ResistanceMagique = ResistanceMagique * passif.Bonus;//augmentation 10%
                ResistancePhysique = ResistancePhysique * passif.Bonus;
                MoveSpeed = MoveSpeed * passif.Malus;//reduction s
                break;
            default:
                break;
        }
    }
    public void Skill1()
    {
            if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
            {
                Mana -= skills[0].Cost;
                Debug.Log(skills[0].Name + " lanc�e");
                skills[0].isCooldown = true;
                if (skills[0].isCooldown == true)
                {
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
            Mana -= skills[1].Cost;
            Debug.Log(skills[1].Name + " lanc�e");
            StartCoroutine(skill2());
            skills[1].isCooldown = true;
            if (skills[1].isCooldown == true)
            {
                StartCoroutine(CoolDown(skills[1]));
            }
        }
        else if (skills[1].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < skills[1].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    IEnumerator skill2()
    {
        ResistanceMagique *= 1.1f;
        ResistancePhysique *= 1.1f;
        yield return new WaitForSeconds(skills[1].CastTime * 2);
        ResistanceMagique *= .8f;
        ResistancePhysique *= .8f;
    }
    public void Ultime()
    {
        if (skills[2].isCooldown == false && Mana >= skills[2].Cost)
        {
            Mana -= skills[2].Cost;
            Debug.Log(skills[2].Name + " lanc�e");
            
            StartCoroutine(UltEffect());
            skills[2].isCooldown = true;
            if (skills[2].isCooldown == true)
            {
                StartCoroutine(CoolDown(skills[2]));
            }
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
    IEnumerator UltEffect()
    {
        float baseHealth = Health;
        
        ResistanceMagique += 45;
        ResistancePhysique += 45f;
        yield return new WaitForSeconds(skills[2].CastTime);
        Transform holder = GameObject.Find("Ult Rangeholder").transform;
        GameObject ultime = Instantiate(ult, holder.position, Quaternion.identity);
        ResistanceMagique -= 45;
        ResistancePhysique -= 45;
        float endHealth = baseHealth - Health;
        Debug.Log("<color=blue>Endhealth full: </color>" + endHealth);
        float fulldmg =ultime.GetComponent<Projectile>().degats = (endHealth * 10) / 100;
        skills[2].Damage += fulldmg;
        Collider[] hitColliders = Physics.OverlapSphere(ultime.transform.position, 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log("<color=green> touch: </color>" + hitCollider.name);
            try
            {
                
                if (hitCollider.TryGetComponent(typeof(Targetable), out Component component))
                {
                    if(hitCollider.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion)
                    {
                        hitCollider.GetComponent<Targetable>().TakeDamage(skills[2].Damage, skills[2].degats.ToString());
                    }
                }
            }
            catch
            {
                print("r");
            }
            
        }
        //Debug.Log("<color=yellow>Endhealth 10%: </color>" + fulldmg);
        yield return new WaitForSeconds(.75f);
        Destroy(ultime);
        
        //Debug.Log("<color=red> full damage: </color>" + skills[2].Damage + " inflig�");
        yield return new WaitForSeconds(.05f);
        skills[2].Damage -= fulldmg;
    }
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }

    
}
