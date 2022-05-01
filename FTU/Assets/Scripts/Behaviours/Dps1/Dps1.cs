using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dps1 : IDamageable, ISkill
{

    [Header("Competences")]
    public Passifs passif;
    public Skills[] skills;

    public float damageSupp;
    public GameObject ult;

    UIDps ui;



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
        ui = GetComponent<UIDps>();
        Passif();
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].isCooldown = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region CC

        //switch (cc)
        //{
        //    case ControlType.none:
        //        canMove = true;
        //        useSkills = true;
        //        break;
        //    case ControlType.stun:
        //        canMove = false;
        //        useSkills = false;
        //        break;
        //    case ControlType.bump:
        //        canMove = false;
        //        useSkills = false;
        //        break;
        //    case ControlType.charme:
        //        canMove = false;
        //        useSkills = false;
        //        break;
        //    case ControlType.root:
        //        canMove = false;
        //        break;
        //    case ControlType.slow:
        //        canMove = true;
        //        useSkills = true;
        //        break;

        //}

        #endregion

        if (Exp >= MaxExp)
        {
            ExperienceBehaviour();
        }

        #region test

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
        #endregion
        //attack sys
        if (useSkills == true)
        {


            if (Cible != null)
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
    }

    IEnumerator AutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
            MeleeAttack();
            yield return new WaitForSeconds(AttackSpeed / ((100 / +AttackSpeed) * 0.01f));
            if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > AttackRange)
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
        if (Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
        {
            if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
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
            if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
            {
                SpawnRangeAttack(EnemyType.minion, Cible, damageSupp);
            }
        }
        else
        {
            //anim.SetBool("AA", false);
        }
    }

    public void SpawnRangeAttack(EnemyType typeEnemy, GameObject Target, float dmgSupp = 0)
    {
        float dmg = DegatsPhysique;
        Instantiate(projPrefab, transform.position, Quaternion.identity);

        projPrefab.GetComponent<Projectile>().degats = dmg + dmgSupp;
        projPrefab.GetComponent<Projectile>().target = Target;
        projPrefab.GetComponent<Projectile>().targetSet = true;
        projPrefab.GetComponent<Projectile>().vitesse = 15f;
    }


    public void Eveil()
    {
        throw new System.NotImplementedException();
    }

    public void Passif()
    {
        if( Cible != null)
        {
            if (IsControl(Cible.GetComponent<IDamageable>().GetEnemyType(), Cible.GetComponent<IDamageable>().GetControl()))
            {
                DegatsPhysique += 15;
            }
            else
            {
                DegatsPhysique -= 15;
            }
        }
        
    }

    public void Skill1()
    {
        //double tirs 
        //1er slow
        //2 dmg et +dmg si slow
        if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
        {
            Mana -= skills[0].Cost;
            Debug.Log(skills[0].Name + " lancée");
            StartCoroutine(skill1());
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

    IEnumerator skill1()
    {
        GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp.GetComponent<Transform>().localScale *= 0.25f;
        GameObject tir1 = Instantiate(sp,transform.position, Quaternion.identity);
        tir1.AddComponent<Rigidbody>();
        tir1.AddComponent<Ball1>();
        tir1.GetComponent<Rigidbody>().useGravity = false;
        tir1.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        var dir = SpawnPrefab2.transform.position - transform.position;
        tir1.GetComponent<Rigidbody>().AddForce(dir.normalized * 7.5f, ForceMode.Impulse);
        Destroy(sp);
        yield return new WaitForSeconds(2f );
        //yield return new WaitForSeconds(skills[0].CastTime );



        GameObject sp2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp2.GetComponent<Transform>().localScale *= 0.25f;
        var dir2 = SpawnPrefab2.transform.position - transform.position;
        GameObject tir2 = Instantiate(sp2, transform.position, Quaternion.identity);
        tir2.AddComponent<Rigidbody>();
        tir2.AddComponent<Ball2>();
        tir2.GetComponent<Ball2>().dps = this;
        tir2.GetComponent<Rigidbody>().useGravity = false;
        tir2.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        tir2.GetComponent<Rigidbody>().AddForce(dir2.normalized * 15f, ForceMode.Impulse);
        Destroy(sp2);
        
        yield return new WaitForSeconds(2f);
        Destroy(tir1);
        Destroy(tir2);
    }
    public void Skill2()
    {
        if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
        {
            Mana -= skills[1].Cost;
            Debug.Log(skills[1].Name + " lancée");
            gameObject.transform.Translate(Vector3.back * 1000f * Time.deltaTime);
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

    public void Ultime()
    {
        if (skills[2].isCooldown == false && Mana >= skills[2].Cost)
        {
            Mana -= skills[2].Cost;
            Debug.Log(skills[2].Name + " lancée");
            Instantiate(ult, transform.position, Quaternion.identity);
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

}
[System.Serializable]
public class Ball1 : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<IDamageable>())
        {
            
            col.gameObject.GetComponent<IDamageable>().TakeCC(ControlType.slow,2.55f);
            Destroy(gameObject);
        }
        
    }
}

[System.Serializable]
public class Ball2 : MonoBehaviour
{
    public Dps1 dps;
    private void OnCollisionEnter(Collision col)
    {
        float dmg = dps.skills[0].Damage;
        if (col.gameObject.GetComponent<IDamageable>())
        {
            if (col.gameObject.GetComponent<IDamageable>().GetControl()== ControlType.slow)
            {
                dmg *= 1.15f;
                col.gameObject.GetComponent<IDamageable>().TakeDamage(dmg, dps.skills[0].degats.ToString());
                Debug.Log("<color=green> damage: </color>" + dmg + " " + dps.skills[0].degats.ToString());
            }
            else
            {
                col.gameObject.GetComponent<IDamageable>().TakeDamage(dmg, dps.skills[0].degats.ToString());
                Debug.Log("<color=blue> damage: </color>" + dmg + " " + dps.skills[0].degats.ToString());
            }
            
            Destroy(gameObject);
        }
        
    }
    
}