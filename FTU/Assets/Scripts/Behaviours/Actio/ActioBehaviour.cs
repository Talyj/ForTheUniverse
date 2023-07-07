using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActioBehaviour : PlayerStats
{
    ActioBehaviour Instance;
    public GameObject sp2;
    public GameObject sp;
    public int MaxTrap = 6;
    List<GameObject> traps = new List<GameObject>();

    //Animation
    public ActioAnimation actioAnimation;

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
        PlayerStatsSetUp();
        BaseInit();
        SetUpCharacters(role, true, false);

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
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        HealthBehaviour();
        ExperienceBehaviour();
        //Passif();
        Behaviour();
        UseObject();
    }

    private void Behaviour()
    {
        if (GetCanAct())
        {
            MovementPlayer();
            if (!isAttacking)
            {
                
                if (Cible != null)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
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

                if (Input.GetKeyDown(KeyCode.A))
                {
                    Skill1();
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Skill2();
                }
                if (Input.GetKeyDown(KeyCode.R) && GetCanUlt())
                {
                    Ultime();
                }
            }
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

    IEnumerator RangeAutoAttack2()
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
            if (Cible.GetComponent<IDamageable>().IsTargetable(team)) 
            {
                Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique,photonView.ViewID);
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
            if (Cible.GetComponent<IDamageable>().IsTargetable(team))
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
        projPrefab.GetComponent<Projectile>().SetCreator(this.photonView);
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
            actioAnimation.Skill1Animation();
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
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
        GameObject tir1 = PhotonNetwork.Instantiate(sp.name,SpawnPrefab.transform.position, Quaternion.identity);
        var dir = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;
        tir1.GetComponent<Ball1>().dps = this;
        tir1.GetComponent<Ball1>().SetCreator(photonView);
        tir1.GetComponent<Rigidbody>().AddForce(dir.normalized * 7.5f, ForceMode.Impulse);
        yield return new WaitForSeconds(2f);



        var dir2 = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;
        GameObject tir2 = PhotonNetwork.Instantiate(sp2.name, SpawnPrefab.transform.position, Quaternion.identity);
        tir2.GetComponent<Ball2>().dps = this;
        tir2.GetComponent<Ball2>().SetCreator(photonView);
        tir2.GetComponent<Rigidbody>().AddForce(dir2.normalized * 15f, ForceMode.Impulse);
        
        yield return new WaitForSeconds(2f);
        Destroy(tir1,2f);
        Destroy(tir2,2.5f);
    }
    public void Skill2()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            SetMana(GetMana() - skills[1].Cost);
            //Debug.Log(skills[1].Name + " lanc�e");
            actioAnimation.Skill2Animation();
            _navMeshAgent.ResetPath();
            GetComponent<Rigidbody>().AddForce(-transform.forward * 500f, ForceMode.VelocityChange);
            //gameObject.transform.Translate(Vector3.back * 1000f * Time.deltaTime);
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
        Debug.Log(skills[2].isCooldown);
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            actioAnimation.UltimateAnimation();
            SetMana(GetMana() - skills[2].Cost);
            //Debug.Log(skills[2].Name + " lanc�e");
            //PhotonNetwork.Instantiate(ult.name, SpawnPrefab.transform.position, Quaternion.identity);
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

    [PunRPC]
    public void SpawnTrap()
    {
        GameObject trapchild = PhotonNetwork.Instantiate(ult.name, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
        trapchild.GetComponent<Trap>().source = this;
        //trapchild.transform.parent = transform.parent.transform;
        trapchild.SetActive(true);
        if (traps.Count >= MaxTrap)
        {
            PhotonNetwork.Destroy(traps[0]);
            traps.RemoveAt(0);
        }
        traps.Add(trapchild);
    }

}