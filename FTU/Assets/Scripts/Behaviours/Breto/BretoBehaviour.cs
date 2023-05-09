﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BretoBehaviour : PlayerStats
{
    //Passive
    //private int _passiveCounter;


    //Skill 1
    [SerializeField] private GameObject dashHitbox;

    //Skill 2

    //Ulti
    
    //private List<GameObject> charmTargets;
    //public GameObject charmArea;

    public void Start()
    {
        PlayerStatsSetUp();
        SetUpCharacters(role, false, false);
        BaseInit();

        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        //_passiveCounter = 0;
        CameraWork();

        SetCanUlt(true);
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
                CheckRangeAttack();
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
        }
    }

    //Copy that in a new character file

    public void Passif()
    {
        //if (_passiveCounter >= 3)
        //{
        //    isPassiveStart = true;
        //    SetDegMag(GetDegMag() * 1.5f);
        //    _passiveCounter = 0;
        //}
    }


    //Have to test after each skills to make sure the buff doesn't stay indefinitely
    private void CheckPassive()
    {
        //if (isPassiveStart)
        //{
        //    isPassiveStart = false;
        //    SetDegMag(GetDegMag() / 1.5f);
        //}
    }

    public void AddPassive()
    {
        //_passiveCounter++;
    }

    //Copy that in a new character file (skill1)
    public void Skill1()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
            skills[0].isCooldown = true;

            Dash();

            StartCoroutine(SecondDash());
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

    public void Dash()
    {
        var proj = PhotonNetwork.Instantiate(dashHitbox.name, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        var dir = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;

        proj.GetComponent<DashBehaviour>().source = this;
        gameObject.GetComponent<Rigidbody>().AddForce(dir * 150f, ForceMode.VelocityChange);
    }

    public IEnumerator SecondDash()
    {
        yield return new WaitForSeconds(1);
        var cpt = 0f;

        while (!Input.GetKeyDown(KeyCode.Alpha1))
        {
            cpt += Time.deltaTime;
            if(cpt >= 3f)
            {
                StartCoroutine(CoolDown(skills[0]));
                yield return null;
            }
            yield return new WaitForFixedUpdate();
        }
        cpt = Time.deltaTime;

        Dash();
        StartCoroutine(CoolDown(skills[0]));
        yield return null;
    }

    //Copy that in a new character file (skill2)
    public void Skill2()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            //buff
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lanc�e");         

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
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");

            //var area = PhotonNetwork.Instantiate(charmArea.name, transform.position, Quaternion.identity);
            //area.GetComponent<CharmAreaBehaviour>().source = this;

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

    //TODO reuse that to make the ult
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
        target.GetComponent<Rigidbody>().AddForce(-direction.normalized * 200f, ForceMode.VelocityChange);
        target.GetComponent<IDamageable>().SetCanMove(true);
    }

    //Copy that in a new character file
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}