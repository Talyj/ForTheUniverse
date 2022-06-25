using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dps1 : PlayerStats
{

    

    Dps1 Instance;



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
        Init();
        SetMoveSpeed(60f);
        SetAttackRange(40f);
        CameraWork();
        Passif();
        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        Instance = this;
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
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        HealthBehaviour();
        ExperienceBehaviour();
        Passif();

        #region test

        // test des touches
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100, DamageType.physique);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetHealth(GetMaxHealth());
            SetMana(GetMaxMana());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(100, DamageType.magique);
        }
        if (Input.GetKeyDown(KeyCode.L))//execute methode
        {
            TakeDamage(9999, DamageType.brut);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetExp(GetMaxExp() + 1);

        }
        #endregion
        //attack sys
        if (GetCanAct())
        {
            //if (GetUseSkills() == true)
            //{
                if (!isAttacking)
                {
                    MovementPlayer();
                    if (Cible != null)
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
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
                    if (Input.GetKeyDown(KeyCode.Alpha3) && GetCanUlt() == true)
                    {
                        Ultime();
                    }
                }
            //}
        }
    }

    IEnumerator AutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / +GetAttackSpeed()) * 0.01f));
            MeleeAttack();
            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / +GetAttackSpeed()) * 0.01f));
            if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
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
            yield return new WaitForSeconds(GetAttackSpeed() + ((100 / GetAttackSpeed()) * 0.01f));
            RangeAttack();
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
        if (Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
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
    }
    public void RangeAttack()
    {
        if (Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
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
        PhotonNetwork.Instantiate(projPrefab.name, SpawnPrefab.transform.position, Quaternion.identity);

        projPrefab.GetComponent<Projectile>().SetDamages(GetDegPhys(), DamageType.physique);
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
                SetDegPhys(GetDegPhys() + 15);
            }
            else
            {
                SetDegPhys(GetDegPhys() - 15);
            }
        }
        
    }

    public void Skill1()
    {
        //double tirs 
        //1er slow
        //2 dmg et +dmg si slow
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
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
        else if (GetMana() < skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    IEnumerator skill1()
    {
        GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp.GetComponent<Transform>().localScale *= 0.25f;
        GameObject tir1 = PhotonNetwork.Instantiate(sp.name,SpawnPrefab.transform.position, Quaternion.identity);
        tir1.AddComponent<Rigidbody>();
        tir1.AddComponent<Ball1>();
        tir1.GetComponent<Rigidbody>().useGravity = false;
        //tir1.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        var dir = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;
        tir1.GetComponent<Rigidbody>().AddForce(dir.normalized * 7.5f, ForceMode.Impulse);
        Destroy(sp);
        yield return new WaitForSeconds(2f );
        //yield return new WaitForSeconds(skills[0].CastTime );



        GameObject sp2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp2.GetComponent<Transform>().localScale *= 0.25f;
        var dir2 = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;
        GameObject tir2 = PhotonNetwork.Instantiate(sp2.name, SpawnPrefab.transform.position, Quaternion.identity);
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
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            SetMana(GetMana() - skills[1].Cost);
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
        else if (GetMana() < skills[1].Cost)
        {
            Debug.Log("pas assez de mana");
        }
        
    }

    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lancée");
            PhotonNetwork.Instantiate(ult.name, SpawnPrefab.transform.position, Quaternion.identity);
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
        else if (GetMana() < skills[2].Cost)
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
            
            col.gameObject.GetComponent<IDamageable>().TakeCC(IDamageable.ControlType.slow,2.55f);
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
            if (col.gameObject.GetComponent<IDamageable>().GetControl()== IDamageable.ControlType.slow)
            {
                dmg *= 1.15f;
                col.gameObject.GetComponent<IDamageable>().TakeDamage(dmg, dps.skills[0].degats);
                Debug.Log("<color=green> damage: </color>" + dmg + " " + dps.skills[0].degats.ToString());
            }
            else
            {
                col.gameObject.GetComponent<IDamageable>().TakeDamage(dmg, dps.skills[0].degats);
                Debug.Log("<color=blue> damage: </color>" + dmg + " " + dps.skills[0].degats.ToString());
            }
            
            Destroy(gameObject);
        }
        
    }
    
}