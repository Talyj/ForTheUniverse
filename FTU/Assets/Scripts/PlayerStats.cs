using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamegeable, ISkill
{
    Animator anim;
    public enum AttackType { Melee,Ranged}
    public AttackType attackType;
    public GameObject Cible;
    [Header("Stats")]
    [SerializeField]
    float Health = 500,MaxHealth=500;
    [SerializeField]
    float MoveSpeed = 4.5f;
    [SerializeField]
    float AttackSpeed = 0.5f;
    [SerializeField]
    float AttackRange = 1.5f;
    [SerializeField]
    float Mana = 100,MaxMana=100;
    [SerializeField]
    float ResistancePhysique = 40; // calcul des resistance health = health - (DegatsPhysiqueReçu -((ResistancePhysique * DegatsPhysiqueReçu)/100)
    [SerializeField]
    float ResistanceMagique = 40; //calcul des resistance health = health - (DegatsMagiqueReçu - ((ResistanceMagique * DegatsMagiqueReçu) / 100)
    [SerializeField]
    float Exp = 0;
    [SerializeField]
    float MaxExp = 100;
    [SerializeField]
    float ExpRate = 1.75f;//multiplicateur de l'exp max
    [SerializeField]
    float DegatsPhysique = 100;
    [SerializeField]
    float DegatsMagique = 100;
    [SerializeField]
    int lvl = 1;
    [Header("Competences")]
    [SerializeField]
    Passifs passif;
    [SerializeField]
    Skills[] Skills;
    [SerializeField]
    bool canUlt = false;
    [SerializeField]
    bool InCombat = false;
    [SerializeField]
    bool InRegen = false;
    bool IsDead = false;

    [Header("Ranged variables")]
    [SerializeField]
    GameObject projPrefab;
    [SerializeField]
    Transform SpawnPrefab;



    #region Getter and Setter

    #region Getter
    public float GetHealth()
    {
        return Health;
    }
    public float GetMaxHealth()
    {
        return MaxHealth;
    }
    public float GetMana()
    {
        return Mana;
    }
    public float GetMaxMana()
    {
        return MaxMana;
    }
    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }
    public float GetAttackSpeed()
    {
        return AttackSpeed;
    }
    public float GetAttackRange()
    {
        return AttackRange;
    }
    public float GetArmor()
    {
        return ResistancePhysique;
    }
    public float GetRM()
    {
        return ResistanceMagique;
    }
    public float GetExp()
    {
        return Exp;
    }
    public float GetAD()
    {
        return DegatsPhysique;
    }
    public float GetAP()
    {
        return DegatsMagique;
    }
    public int GetLvl()
    {
        return lvl;
    }

    public Skills[] GetSkills()
    {
        return Skills;
    }
    public Skills GetSkill1()
    {
        return Skills[0];
    }
    public Skills GetSkill2()
    {
        return Skills[1];
    }
    public Skills GetUlt()
    {
        return Skills[2];
    }
    #endregion
    #region Setter
    public void SetHealth(float value)
    {
        Health += value;
    }
    public void SetMaxHealth(float value)
    {
        MaxHealth += value;
    }
    public void SetMana(float value)
    {
        Mana += value;
    }
    public void SetMoveSpeed(float value)
    {
        MoveSpeed += value;
    }
    public void SetAttackSpeed(float value)
    {
         AttackSpeed += value;
    }
    public void SetAttackRange(float value)
    {
        AttackRange += value;
    }
    public void SetArmor(float value)
    {
        ResistancePhysique += value;
    }
    public void SetRM(float value)
    {
        ResistanceMagique += value;
    }
    public void SetExp(float value)
    {
        Exp += value;
    }
    public void SetAD(int value)
    {
         DegatsPhysique += value;
    }
    public void SetAP(int value)
    {
        DegatsMagique += value;
    }
    public void SetLvl(int value)
    {
        lvl += value;
    }
    #endregion

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Passif();
    }

    // Update is called once per frame
    void Update()
    {

        //attack sys
        if(Cible != null)
        {
            if (Input.GetMouseButtonDown(1))
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

        if (Health >= MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Mana >= MaxMana)
        {
            Mana = MaxMana;
        }

        //if(InCombat == false && InRegen == false)
        //{
        //    InRegen = true;
        //    Debug.Log("regen");
        //    Regen();
        //}
        //Xp avec calcule des reste d'exp si lvl up

        if (Exp >= MaxExp)
        {
            float reste = Exp - MaxExp;
            lvl += 1;
            Exp = 0 + reste;
            MaxExp = MaxExp * ExpRate;
            print("lvl up");
            Passif();
            if (lvl == 6)
            {
                canUlt = true;
            }
            // augmentation des stats a faire
            //test en dur a rendre plus automatique par scriptableobject surement
            MaxHealth += 106;
            MaxMana += 65;
            AttackSpeed += .12f;
            DegatsPhysique += 3.75f;
            DegatsMagique += 2.75f;
            ResistanceMagique += 2.25f;
            ResistancePhysique += 2.25f;
            MoveSpeed += 0.75f;
        }





        // test des touches
        if (Input.GetKeyDown(KeyCode.K))
        {
            //TakeDamage(Skills[1].Damage,Skills[1].degats.ToString());
            
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
            Exp += 150;
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Skill1();
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
        anim.SetBool("AA", true);
        yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
        if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
        {
            anim.SetBool("AA", false);
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

    IEnumerator CoolDown(Skills skill)
    {
        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
        skill.isCooldown = false;
    }

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

    public void Passif()
    {
        // test generique
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
        if(Skills[0].isCooldown == false && Mana >= Skills[0].Cost)
        {
            Mana -= Skills[0].Cost;
            Debug.Log(Skills[0].Name + " lancée");
            Skills[0].isCooldown = true;
            if (Skills[0].isCooldown == true)
            {
                StartCoroutine(CoolDown(Skills[0]));
            }
        }
        else if (Skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if ( Mana < Skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public void Skill2()
    {
        if (Skills[1].isCooldown == false && Mana >= Skills[1].Cost)
        {
            //buff
            Mana -= Skills[1].Cost;
            Debug.Log(Skills[1].Name + " lancée");
            StartCoroutine(Buff(Skills[1]));
            Skills[1].isCooldown = true;

            if (Skills[1].isCooldown == true)
            {
                StartCoroutine(CoolDown(Skills[1]));
            }
        }
        else if (Skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < Skills[0].Cost)
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
