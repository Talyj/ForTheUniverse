using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermaidBehaviour : PlayerStats
{
    //Passive
    private int _passiveCounter;
    private bool isPassiveStart;

    //Skill 1
    public GameObject poissoin;

    //Skill 2
    public float speedPush;
    private float windTimerDefault = 0.2f;
    public GameObject windArea;
    public MermaidBehaviour Instance;

    //Ulti
    private float ultiTimerDefault = 1;
    private float charmSpeed;
    private List<GameObject> charmTargets;
    public GameObject charmArea;

    public void Start()
    {
        Init();
        SetMoveSpeed(60f);
        SetAttackRange(40f);
        SetHealth(500);
        SetMaxHealth(500);
        speedPush = 3;
        charmSpeed = 5;
        charmTargets = new List<GameObject>();
        foreach(var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        Instance = this;
        isPassiveStart = false;
        _passiveCounter = 0;
        CameraWork();
    }

    //Copy that in a new character file
    public void Update()
    {
        if(!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        HealthBehaviour();
        ExperienceBehaviour();
        Passif();

        if (GetCanAct())
        {
            MovementPlayer();
            if(!isAttacking)
            {
                try
                {
                    if (Input.GetMouseButtonDown(0))
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
                catch(Exception e)
                {
                    Debug.Log("No target available");
                }

                if (Input.GetKeyDown(KeyCode.Alpha1) && Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
                {
                    Poissoin(EnemyType.minion, Cible);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && Cible != null)
                {
                    MagicWind(Cible);
                }

                if (Input.GetKeyDown(KeyCode.Alpha3) && GetCanUlt() == true)
                {
                    Ultime();
                }
            }           
        }
    }

    //Copy that in a new character file

    public void Passif()
    {
       if(_passiveCounter >= 3)
        {
            isPassiveStart = true;
            SetDegMag(GetDegMag() * 1.5f) ;
            _passiveCounter = 0;
        }
    }


    //Have to test after each skills to make sure the buff doesn't stay indefinitely
    private void CheckPassive()
    {
        if (isPassiveStart)
        {
            isPassiveStart = false;
            SetDegMag(GetDegMag() / 1.5f);
        }
    }

    public void AddPassive()
    {
        _passiveCounter++;
    }

    //Copy that in a new character file (skill1)
    public void Poissoin(EnemyType typeEnemy, GameObject target)
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
            skills[0].isCooldown = true;

            if (typeEnemy == EnemyType.minion /*||typeEnemy == Targetable.EnemyType.Adversaire*/)
            {
                var proj = PhotonNetwork.Instantiate(poissoin.name, transform.position, Quaternion.identity);
                proj.GetComponent<PoissoinProjBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
                proj.GetComponent<PoissoinProjBehaviour>().target = target;
                proj.GetComponent<PoissoinProjBehaviour>().targetSet = true;
                proj.GetComponent<PoissoinProjBehaviour>().source = Instance;
            }
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
    public void MagicWind(GameObject target)
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            //buff
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lanc�e");

            var targetPos = new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z);
            var pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Quaternion rotation = Quaternion.LookRotation(targetPos - pos);
            Vector3 direction = targetPos - pos;

            var proj = PhotonNetwork.Instantiate(windArea.name, transform.position, rotation);
            proj.GetComponent<WindAreaBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
            proj.GetComponent<WindAreaBehaviour>().direction = direction;
            proj.GetComponent<WindAreaBehaviour>().source = Instance;

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
    public void AddWindedTarget(GameObject target)
    {
        charmTargets.Add(target);
        target.GetComponent<IDamageable>().SetCanMove(false);
        StartCoroutine(GoAway(target));
    }

    public IEnumerator GoAway(GameObject target)
    {
        var timer = windTimerDefault;
        while (timer >= 0)
        {
            Vector3 direction = target.transform.position - transform.position;
            target.transform.position += direction * Time.deltaTime * speedPush;
            //target.transform.position += (transform.position + target.transform.position).normalized * charmSpeed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        target.GetComponent<IDamageable>().SetCanMove(true);
        yield return 0;
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


    //Copy that in a new character file
    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");

            var area = PhotonNetwork.Instantiate(charmArea.name, transform.position, Quaternion.identity);
            area.GetComponent<CharmAreaBehaviour>().source = Instance;

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

    public void AddTCharmedTargets(GameObject target)
    {        
        charmTargets.Add(target);
        target.GetComponent<IDamageable>().SetCanMove(false);
        StartCoroutine(GetNear(target));
    }

    public IEnumerator GetNear(GameObject target)
    {
        var timer = ultiTimerDefault;
        while(timer >= 0)
        {
            //target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, 1.0f);
            target.transform.position += (transform.position - target.transform.position).normalized * charmSpeed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        target.GetComponent<IDamageable>().SetCanMove(true);
        yield return 0;
    }

    //Copy that in a new character file
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
