using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    //Ulti
    private float ultiTimerDefault = 1;
    private float charmSpeed;
    //private List<GameObject> charmTargets;
    public GameObject charmArea;
    
    //Animation
    public MermaidAnimation mermaidAnimation;
    
    public void Start()
    {
        PlayerStatsSetUp();
        SetUpCharacters(role, true, true);
        BaseInit();

        foreach(var elmt in skills)
        {
            elmt.isCooldown = false;
        }

        speedPush = 3;
        charmSpeed = 5;
        isPassiveStart = false;
        _passiveCounter = 0;

        CameraWork();

        SetCanUlt(true);
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
                //                StartCoroutine(RangeAutoAttack());
                //            }
                //        }
                //    }
                //}
                //catch (Exception e)
                //{
                //    Debug.Log("No target available");
                //}

                CheckRangeAttack();
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Poissoin();
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    MagicWind();
                }

                if (Input.GetKeyDown(KeyCode.R) && GetCanUlt() == true)
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
    public void Poissoin()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            mermaidAnimation.Skill1Animation();
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
            skills[0].isCooldown = true;

            //Modifier le skill ici
            //var proj = PhotonNetwork.Instantiate(poissoin.name, transform.position, Quaternion.identity);

            CheckPassive();   

            //jusqu'à là
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
    public void MagicWind()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            mermaidAnimation.Skill2Animation();
            //buff
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lanc�e");
            skills[1].isCooldown = true;

            /*Quaternion rotation = Quaternion.LookRotation(SpawnPrefab2.position - SpawnPrefab.position);
            Vector3 direction = SpawnPrefab2.position - SpawnPrefab.position;

            var proj = PhotonNetwork.Instantiate(windArea.name, transform.position, rotation);*/
            

            CheckPassive();
            StartCoroutine(CoolDown(skills[1]));
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
    public void AddWindedTarget(GameObject target)
    {
        target.GetComponent<IDamageable>().SetCanMove(false);
        photonView.RPC("GoAway", RpcTarget.All, new object[] { target.GetPhotonView().ViewID });
    }

    [PunRPC]
    public void GoAway(int viewId)
    {
        var target = viewIdToGameObject(viewId);
        target.GetComponent<IDamageable>()._navMeshAgent.ResetPath();
        Vector3 direction = target.transform.position - transform.position;
        target.GetComponent<Rigidbody>().AddForce(direction.normalized * 200f, ForceMode.VelocityChange);
        target.GetComponent<IDamageable>().SetCanMove(true);
    }

    IEnumerator Buff(Skills skill)
    {
        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
        skill.isCooldown = false;
    }


    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            mermaidAnimation.UltimateAnimation();
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");

            //var area = PhotonNetwork.Instantiate(charmArea.name, transform.position, Quaternion.identity);
            

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
        target.GetComponent<IDamageable>().SetCanMove(false);
        photonView.RPC("GetNear", RpcTarget.All, new object[] { target.GetPhotonView().ViewID });
    }

    [PunRPC]
    public void GetNear(int viewId)
    {
        var target = viewIdToGameObject(viewId);
        Vector3 direction = target.transform.position - transform.position;
        //target.GetComponent<Rigidbody>().AddForce(-direction.normalized * 200f, ForceMode.VelocityChange);
        target.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        target.GetComponent<IDamageable>().SetCanMove(true);
    }

    //Copy that in a new character file
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
