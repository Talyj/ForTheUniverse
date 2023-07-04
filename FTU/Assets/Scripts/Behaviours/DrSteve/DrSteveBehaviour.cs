using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrSteveBehaviour : PlayerStats
{
    //Passive
    private int _passiveCounter;

    //Skill 1
    private bool serringueActive;
    private float ratioSerringue;

    //Skill 2
    private float rangeHeal;
    private bool vaccinActive;
    private float ratioVaccin;
    private GameObject patient;

    //Ulti
    
    public void Start()
    {
        PlayerStatsSetUp();
        SetUpCharacters(role, true, true);
        BaseInit();
        
        foreach(var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        _passiveCounter = 0;
        serringueActive = false;
        vaccinActive = false;
        rangeHeal = 0.0f;
        
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

        if (vaccinActive)
        {
            CheckPatient();
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
                Debug.Log(GetAttackRange());
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
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Serringue();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Vaccin();
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
       
    }


    //Have to test after each skills to make sure the buff doesn't stay indefinitely
    private void CheckPassive()
    {
        
    }

    public void IncreasePassive()
    {
        _passiveCounter++;
    }
    
    public void DecreasePassive()
    {
        _passiveCounter++;
    }
    
    public new IEnumerator AutoAttack()
    {
        while (Cible != null)
        {
            try
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange())
                {
                    if (Cible.GetComponent<IDamageable>().IsTargetable(team))
                    {
                        Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique,photonView.ViewID);
                    }

                    if (Cible.GetComponent<IDamageable>().team != team && ( Cible.GetComponent<IDamageable>().enemyType == EnemyType.player || Cible.GetComponent<IDamageable>().enemyType == EnemyType.minion))
                    {
                        if (serringueActive)
                        {
                            serringueActive = false;
                            Cible.GetComponent<IDamageable>().TakeDamage(GetDegMag() * ratioSerringue, DamageType.magique,photonView.ViewID);
                        }
                    }
                }
            }
            catch (NullReferenceException e)
            {
                Cible = null;
            }

            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / +GetAttackSpeed()) * 0.01f));
        }

    }

    public void CheckPatient()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            try
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.collider.TryGetComponent(typeof(IDamageable), out Component component))
                    {
                        if (component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.player)
                        {
                            if (hit.collider.GetComponent<IDamageable>().team ==
                                gameObject.GetComponent<IDamageable>().team &&
                                Vector3.Distance(hit.collider.gameObject.transform.position, this.transform.position) <
                                rangeHeal)
                            {
                                patient = hit.collider.gameObject;
                                patient.GetComponent<IDamageable>().SetHealth(patient.GetComponent<IDamageable>().GetHealth() + _passiveCounter * (ratioVaccin * GetDegMag()));
                            }
                        }
                    }
                    else if (hit.collider.GetComponent<IDamageable>() == null)
                    {
                        patient.GetComponent<IDamageable>().Cible = null;
                    }
                }
            }
            catch(Exception ue)
            {
                //:)
            }
        }
    }

    //Copy that in a new character file (skill1)
    public void Serringue()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
            skills[0].isCooldown = true;

            serringueActive = true;
            
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
    public void Vaccin()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            //buff
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lanc�e");
            skills[1].isCooldown = true;

            vaccinActive = true;

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

    [PunRPC]
    public void GoAway(int viewId)
    {
        var target = viewIdToGameObject(viewId);
        Vector3 direction = target.transform.position - transform.position;
        target.GetComponent<Rigidbody>().AddForce(direction.normalized * 200f, ForceMode.VelocityChange);
        target.GetComponent<IDamageable>().SetCanMove(true);
    }


    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");
            skills[2].isCooldown = true;

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
    

    //Copy that in a new character file
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
