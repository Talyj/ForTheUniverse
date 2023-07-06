using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.VFX;

public class ConsBehaviour : PlayerStats
{
    //Passive
    [SerializeField]
    public int _passiveCounter;
    private bool isPassiveStart;
    //public GameObject[] spawns;
    //public GameObject[] lights;
    //Skill 1
    public GameObject beam;
    public GameObject sp;

    
    //Ulti
    private float ultiTimerDefault = 1;
    private float slow;
    public GameObject ultArea;
    
    //Animation
    public ConsAnimation consAnimation;

    public void Start()
    {
        PlayerStatsSetUp();
        BaseInit();
        characterID = 3;
        SetUpCharacters(role, true, true);
        SetMoveSpeed(40f);
        slow = 5;
        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        isPassiveStart = false;
        _passiveCounter = 10;
        CameraWork();
    }

    //Copy that in a new character file
    public void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        HealthBehaviour();
        ExperienceBehaviour();
        Passif();
        Behaviour();
    }

    private void Behaviour()
    {
        if (GetCanAct())
        {
            MovementPlayer();
            if (!isAttacking)
            {
                //try
                //{
                //    if (Input.GetMouseButtonDown(0))
                //    {
                //        if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
                //        {
                //            print("Hors d portée");
                //        }
                //        else
                //        {
                //            if (attackType == AttackType.Melee)
                //            {
                //                StartCoroutine(AutoAttack());
                //            }
                //            if (attackType == AttackType.Ranged)
                //            {
                //                StartCoroutine(RangeAutoAttackCons());
                //            }
                //        }
                //    }
                //}
                //catch (Exception e)
                //{
                //    Debug.Log("No target available");
                //}
                CheckRangeAttack();
                if (Input.GetKeyDown(KeyCode.K))
                {
                    //photonView.RPC("DealDamages",RpcTarget.All, new object[] { 9999 });
                    TakeDamage(999f, DamageType.physique, photonView.ViewID);
                }
                    if (Input.GetKeyDown(KeyCode.A))
                {
                    BeamUltra();
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Vvs();
                }

                if (Input.GetKeyDown(KeyCode.R) && GetCanUlt() == true)
                {
                    Ultime();
                }
            }
        }
    }

    //essaie AA cons
    public IEnumerator RangeAutoAttackCons()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            try
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() ||
                    Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 5 && isAI)
                {
                    if (Cible.GetComponent<IDamageable>().IsTargetable(team))
                    {
                        SpawnRangeAttackCons(Cible, damageSupp);
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
            catch (NullReferenceException e)
            {
                Cible = null;
            }
            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / GetAttackSpeed()) * 0.01f));
        }

    }
    public void SpawnRangeAttackCons(GameObject Target, float dmgSupp = 0)
    {
        //var r = UnityEngine.Random.Range(0, spawns.Length);
        //foreach(var l in lights)
        //{
        //    l.SetActive(false);
        //}
        //lights[r].SetActive(true);
        var bullets = PhotonNetwork.Instantiate(projPrefab.name, sp.transform.position, Quaternion.identity);

        bullets.GetComponent<Projectile>().SetDamages(GetDegMag() + dmgSupp, DamageType.magique);
        bullets.GetComponent<Projectile>().target = Target;
        bullets.GetComponent<Projectile>().targetSet = true;
    }

    //Copy that in a new character file

    public void Passif()
    {
        if (_passiveCounter > 10)
        {
            isPassiveStart = true;
            SetDegPhys(GetDegPhys() * 1.5f);            
        }
    }


    //Have to test after each skills to make sure the buff doesn't stay indefinitely
    private void CheckPassive()
    {
        if (isPassiveStart)
        {
            isPassiveStart = false;
            SetDegPhys(GetDegPhys() / 1.5f);
            _passiveCounter = 0;
        }
    }

    public void AddPassive()
    {
        _passiveCounter++;
    }

    //Copy that in a new character file (skill1)
    public void BeamUltra()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            consAnimation.Skill1Animation();
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");

            skills[0].isCooldown = true;
            /*var r = UnityEngine.Random.Range(0, spawns.Length);
            foreach (var l in lights)
            {
                l.SetActive(false);
            }
            lights[r].SetActive(true);

            Transform targ;
            if (Cible)
            {
                targ = Cible.transform;
            }
            else
            {
                targ = SpawnPrefab2;
            }

            var proj = PhotonNetwork.Instantiate(beam.name, spawns[r].transform.position, Quaternion.identity);
            var dir = targ.transform.position - spawns[r].transform.position;
            proj.GetComponent<ProjCons>().SetDamages(GetDegPhys(), DamageType.physique);
            proj.GetComponent<ProjCons>().source = this;
            proj.GetComponent<ProjCons>().teams = team;
            proj.GetComponent<Rigidbody>().AddForce(dir.normalized * 30f, ForceMode.Impulse);*/

            CheckPassive();
            StartCoroutine(CoolDown(skills[0]));
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

    //Copy that in a new character file (skill2)
    public void Vvs()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost && _passiveCounter >= 1)
        {
            consAnimation.Skill2Animation();
            //buff
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lanc�e");

            //StartCoroutine(skill2());

            CheckPassive();
            StartCoroutine(CoolDown(skills[1]));
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


    //Copy that in a new character file
    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            consAnimation.UltimateAnimation();
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");
            StartCoroutine(Ult());
            //var area = PhotonNetwork.Instantiate(ultArea.name, transform.position, Quaternion.identity);
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, GetAttackRange());
            if (hitColliders != null)
            {
                foreach (var col in hitColliders)
                {
                    if (col.gameObject.GetComponent<IDamageable>())
                    {
                        if(col.gameObject.GetComponent<IDamageable>().team != team)
                        {
                            if(col.gameObject.GetComponent<IDamageable>().enemyType == EnemyType.golem)
                            {
                                return;
                            }
                            else
                            {
                                var degMult = _passiveCounter;
                                if(degMult <= 0)
                                {
                                    degMult = 1;
                                }
                                col.gameObject.GetComponent<IDamageable>().TakeDamage(GetDegPhys() * degMult, DamageType.magique, photonView.ViewID);
                            }
                        }
                    }

                }
            }



            StartCoroutine(CoolDown(skills[2]));
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

    private IEnumerator Ult()
    {
        yield return new WaitForSeconds(1.05f);
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, GetAttackRange());
        if (hitColliders != null)
        {
            foreach (var col in hitColliders)
            {
                if (col.gameObject.GetComponent<IDamageable>())
                {
                    if (col.gameObject.GetComponent<IDamageable>().enemyType == EnemyType.golem) yield return 0;
                    if (col.gameObject.GetComponent<IDamageable>().team != team)
                    {
                        var degMult = _passiveCounter < 1 ? 1 : _passiveCounter;
                        col.gameObject.GetComponent<IDamageable>().TakeDamage(GetDegPhys() * degMult, DamageType.magique, photonView.ViewID);
                    }
                }

            }
        }
    }

    

    //Copy that in a new character file
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
