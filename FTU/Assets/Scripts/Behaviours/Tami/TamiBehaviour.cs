using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamiBehaviour : PlayerStats
{
    //Ulti
    public GameObject spear;
    public Transform[] ultTransform;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStatsSetUp();
        BaseInit();
        SetUpCharacters(role, false, false);

        CameraWork();

        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        Passif();
        SetCanUlt(true);
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

                if (Input.GetKeyDown(KeyCode.Alpha1) && Cible && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
                {
                    Stonk();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && Cible && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 3)
                {
                    VienLa();
                }

                if (Input.GetKeyDown(KeyCode.Alpha3) && GetCanUlt() == true && Cible)
                {
                    Ultime();
                }
            }
        }
    }


    public void Passif()
    {
        float bonus = .25f;
        SetResMag(GetResMag() + bonus);
        SetResPhys(GetResPhys() + bonus);
    }


    #region skill1
    public void Stonk()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lancée");

            Cible.GetComponent<IDamageable>().TakeDamage(GetDegMag() * 1.75f, skills[0].degats);
            if(Cible.GetComponent<IDamageable>().GetHealth() <= 0)
            {
                Passif();
            }

            skills[0].isCooldown = true;
            if (skills[0].isCooldown == true)
            {
                StartCoroutine(CoolDown(skills[0]));
            }
        }
        else if (skills[0].isCooldown == true)
        {
            //Debug.Log("en cd");
        }
        else if (GetMana() < skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    #endregion

    #region skill2
    public void VienLa()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lancée");
            #region base
            //var dir = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;
            //lr.enabled = true;
            //lr.SetPosition(1, grabPoint.transform.position);
            //if (Physics.Raycast(grabPoint.transform.position, dir.normalized, out hit, maxDst, surfaces))
            //{
            //    lr.enabled = true;
            //    lr.SetPosition(1, hit.collider.gameObject.transform.position);
            //}
            //MoveToSpot(Cible);
            //lr.enabled = false;
            photonView.RPC("MoveToSpot", RpcTarget.All, new object[] { Cible.GetPhotonView().ViewID});
            #endregion


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
    
    [PunRPC]
    public void MoveToSpot(int viewId)
    {
        var target = viewIdToGameObject(viewId);
        Vector3 direction = target.transform.position - transform.position;
        target.GetComponent<Rigidbody>().AddForce(-direction.normalized * 200f, ForceMode.VelocityChange);
    }

    #endregion


    #region Ultime
    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lancée");

            foreach(var pos in ultTransform)
            {
                Quaternion rotation = Quaternion.LookRotation(Cible.transform.position - pos.position);
                var proj = PhotonNetwork.Instantiate(spear.name, pos.position, rotation);
                var dir = Cible.transform.position - SpawnPrefab.transform.position;
                proj.GetComponent<SpearBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
                proj.GetComponent<SpearBehaviour>().source = this;
                proj.GetComponent<SpearBehaviour>().team = team;
                proj.GetComponent<Rigidbody>().AddForce(dir.normalized * 30f, ForceMode.Impulse);
            }

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
    

    #endregion
}
