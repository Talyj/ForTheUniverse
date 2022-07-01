using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamiBehaviour : PlayerStats
{
    //skill 2
    public Transform grabPoint;
    public RaycastHit hit;
    public LayerMask surfaces;
    public LineRenderer lr;
    public int maxDst = 10;
    public float speedGrab = 25;
    public Vector3 location;
    TamiBehaviour Instance;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        SetMoveSpeed(60f);
        SetAttackRange(10f);
        SetHealth(500000);
        SetMaxHealth(500000);
        SetResPhys(40);
        SetResMag(40);
        CameraWork();
        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        Instance = this;
        Passif();
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

                if (Input.GetKeyDown(KeyCode.Alpha1) && Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
                {
                    Stonk();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    VienLa();
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

            Cible.GetComponent<IDamageable>().TakeDamage(skills[0].Damage, skills[0].degats);
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
            var dir = SpawnPrefab2.transform.position - SpawnPrefab.transform.position;
            lr.enabled = true;
            lr.SetPosition(1, grabPoint.transform.position);
            if (Physics.Raycast(grabPoint.transform.position, dir.normalized, out hit, maxDst, surfaces))
            {
                lr.enabled = true;
                lr.SetPosition(1, hit.collider.gameObject.transform.position);
                MoveToSpot();
            }
            //lr.enabled = false;
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
    


    #endregion

    public void MoveToSpot()
    {
        
        hit.collider.gameObject.GetComponent<IDamageable>().TakeCC(ControlType.stun, .75f);
        hit.collider.gameObject.transform.position = Vector3.Lerp(transform.position, location, speedGrab * Time.deltaTime / Vector3.Distance(transform.position, location));
        lr.SetPosition(0, grabPoint.position);
        lr.enabled = false;
    }

    #region Ultime
    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lancée");

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
