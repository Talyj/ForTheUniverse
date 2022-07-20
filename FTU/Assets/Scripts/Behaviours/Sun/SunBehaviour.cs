using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehaviour : PlayerStats
{
    //Passive
    private float dmgMultiplier;

    //Skill2
    public GameObject stickTP;
    public int step;
    [HideInInspector] public Vector3 tpPos;
    public float defaultTimer = 10;
    public bool isTouched;

    //Ulti
    public GameObject bigStick;

    public SunBehaviour Instance;

    //Passif
    private enum Sticks
    {
        AuraStick = 0,
        SpiritSitck = 1
    }
    private Sticks currentStick;

    public void Start()
    {
        Init();
        SetMoveSpeed(60f);
        SetAttackRange(20f);
        SetAttackSpeed(1.95f);
        SetHealth(500000);
        SetMaxHealth(500000);
        SetResPhys(40);
        SetResMag(40);
        CameraWork();
        currentStick = Sticks.AuraStick;
        step = 0;
        Instance = this;
        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        HealthBehaviour();
        ExperienceBehaviour();
        if (Cible != null)
        {
            Passif();
        }
        Behaviour();

        
    }

    private void Behaviour()
    {
        if (GetCanAct())
        {
            MovementPlayer();
            if (!isAttacking)
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
                catch (Exception e)
                {
                    Debug.Log("No target available");
                }


                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Swap(Cible);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    StickTeleportation();
                }

                if (Input.GetKeyDown(KeyCode.Alpha3) && GetCanUlt() == true)
                {
                    Ultime();
                }
                
            }
        }
    }

    public void Passif()
    {
        switch (currentStick)
        {
            case Sticks.AuraStick:
                if(Cible.GetComponent<IDamageable>().enemyType == EnemyType.voister)
                {
                    damageSupp = DamageMultiplier(GetDegPhys(), 0.5f);
                }
                break;
            case Sticks.SpiritSitck:
                if (Cible.GetComponent<IDamageable>().enemyType == EnemyType.joueur)
                {
                    damageSupp = DamageMultiplier(GetDegMag(), 0.5f);
                }
                break;
        }
    }

    private void SwitchStick()
    {
        if(currentStick == Sticks.AuraStick)
        {
            currentStick = Sticks.SpiritSitck;
        }
        else
        {
            currentStick = Sticks.AuraStick;
        }
    }

    public void Swap(GameObject target)
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
            skills[0].isCooldown = true;

            SwitchStick();
            if(target != null)
            {
                if(currentStick == Sticks.AuraStick && target.GetComponent<IDamageable>().enemyType == EnemyType.voister)
                {
                    target.GetComponent<IDamageable>().SetCanAct(false);
                    StartCoroutine(SwapCooldown(target));
                }
                else if(currentStick == Sticks.SpiritSitck && target.GetComponent<IDamageable>().enemyType == EnemyType.joueur)
                {
                    var speedTemp = target.GetComponent<IDamageable>().GetMoveSpeed();
                    target.GetComponent<IDamageable>().SetMoveSpeed(GetMoveSpeed()/2);
                    StartCoroutine(SwapCooldown(target, speedTemp));
                }
            }

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

    public IEnumerator SwapCooldown(GameObject target, float defaultSpeed = 0)
    {
        yield return new WaitForSeconds(3);
        if(defaultSpeed != 0)
        {
            target.GetComponent<IDamageable>().SetMoveSpeed(defaultSpeed);
        }
        else
        {
            target.GetComponent<IDamageable>().SetCanAct(true);
        }
        
    }

    public void StickTeleportation()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            if(step == 0)
            {
                //buff
                SetMana(GetMana() - skills[1].Cost);
                Debug.Log(skills[1].Name + " lanc�e");

                Vector3 direction = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;

                var proj = PhotonNetwork.Instantiate(stickTP.name, transform.position, Quaternion.identity);
                proj.GetComponent<ThrowStickBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
                proj.GetComponent<ThrowStickBehaviour>().targetSet = true;
                proj.GetComponent<ThrowStickBehaviour>().source = Instance;
                proj.GetComponent<Rigidbody>().AddForce(direction.normalized * 30f, ForceMode.Impulse);


                StartCoroutine(StickTeleportationSecondPart());
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

    public IEnumerator StickTeleportationSecondPart()
    {
        step = 1;
        yield return new WaitForSeconds(5f);
        step = 0;
        skills[1].isCooldown = true;
        StartCoroutine(CoolDown(skills[1]));
    }

    public void TP(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (step == 1)
        {
            if (other.GetComponent<IDamageable>())
            {
                if (other.GetComponent<IDamageable>().team != team)
                {
                    other.GetComponent<IDamageable>().TakeDamage(GetDegMag(), DamageType.physique);
                }
            }
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
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");
            //TODO

            Quaternion rotation = Quaternion.LookRotation(SpawnPrefab2.transform.position - SpawnPrefab.transform.position);
            Vector3 direction = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;

            //TODO
            var proj = PhotonNetwork.Instantiate(bigStick.name, transform.position, rotation);
            proj.GetComponent<BigStickBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
            proj.GetComponent<BigStickBehaviour>().direction = direction;
            proj.GetComponent<BigStickBehaviour>().team = team;

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

    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
