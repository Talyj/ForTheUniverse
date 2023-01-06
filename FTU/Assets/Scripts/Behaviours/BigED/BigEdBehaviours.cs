using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class BigEdBehaviours : PlayerStats
{

    public GameObject ultime, skill1;

    BigEdBehaviours Instance;

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

                if (Input.GetKeyDown(KeyCode.Alpha1) && Cible != null )
                {
                    HeadImpact();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    EnPlace();
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
        switch (GetLvl())
        {
            case 1:
                SetResMag(GetResMag() * passif.Bonus);//augmentation 5%
                SetResPhys(GetResPhys() * passif.Bonus);
                SetMoveSpeed(GetMoveSpeed() * passif.Malus);//reduction 5%
                break;
            case 6:
                passif.Bonus = 1.075f;
                passif.Malus = 0.925f;
                SetResMag(GetResMag() * passif.Bonus);//augmentation 7.5%
                SetResPhys(GetResPhys() * passif.Bonus);
                //SetMoveSpeed(GetMoveSpeed() * passif.Malus);//reduction 7.5%
                break;
            case 12:
                passif.Bonus = 1.1f;
                passif.Malus = 0.8f;
                SetResMag(GetResMag() * passif.Bonus);//augmentation 10%
                SetResPhys(GetResPhys() * passif.Bonus);
                //SetMoveSpeed(GetMoveSpeed() * passif.Malus);//reduction s
                break;
            default:
                break;
        }
    }


    #region skill1
    public void HeadImpact()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lancée");

            GameObject headImp = PhotonNetwork.Instantiate(skill1.name, SpawnPrefab2.transform.position, Quaternion.identity);
            headImp.AddComponent<HeadImpact>();
            headImp.GetComponent<HeadImpact>().bg = this;
            Destroy(headImp,2.5f);

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
    public void EnPlace()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lancée");
            StartCoroutine(skill2());
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
    IEnumerator skill2()
    {
        SetResMag(GetResMag() * 1.1f);
        SetResPhys(GetResPhys() * 1.1f);
        yield return new WaitForSeconds(skills[1].CastTime * 2);
        SetResMag(GetResMag() / 1.1f);
        SetResPhys(GetResPhys() / 1.1f);
    }


    #endregion
   
    #region Ultime
    public void Ultime()
    {
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");

            StartCoroutine(UltEffect());
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
    IEnumerator UltEffect()
    {
        float baseHealth = GetHealth();

        SetResMag(GetResMag() + 15);
        SetResPhys(GetResPhys() + 15);
        yield return new WaitForSeconds(skills[2].CastTime);
        Transform holder = GameObject.Find("Ult Rangeholder").transform;
        GameObject ultime = PhotonNetwork.Instantiate(ult.name, holder.position, Quaternion.identity);
        SetResMag(GetResMag() - 15);
        SetResPhys(GetResPhys() - 15);
        float endHealth = baseHealth - GetHealth();
        //Debug.Log("<color=blue>Endhealth full: </color>" + endHealth);
        float fulldmg = (endHealth * 10) / 100;
        skills[2].Damage += fulldmg;
        Collider[] hitColliders = Physics.OverlapSphere(ultime.transform.position, 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log("<color=green> touch: </color>" + hitCollider.name);
            try
            {

                if (hitCollider.TryGetComponent(typeof(IDamageable), out Component component))
                {
                    if (IsTargetable(hitCollider.GetComponent<IDamageable>().GetEnemyType()))
                    {
                        hitCollider.GetComponent<IDamageable>().TakeDamage(skills[2].Damage, skills[2].degats);
                    }
                }
            }
            catch
            {
                print("r");
            }

        }
        //Debug.Log("<color=yellow>Endhealth 10%: </color>" + fulldmg);
        yield return new WaitForSeconds(.75f);
        Destroy(ultime);

        //Debug.Log("<color=red> full damage: </color>" + skills[2].Damage + " inflig�");
        yield return new WaitForSeconds(.05f);
        skills[2].Damage -= fulldmg;
    }

    #endregion
}

[System.Serializable]
public class HeadImpact : MonoBehaviourPun
{
    public BigEdBehaviours bg;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<IDamageable>())
        {

            col.gameObject.GetComponent<IDamageable>().TakeCC(IDamageable.ControlType.slow, 2.55f);
            col.gameObject.GetComponent<IDamageable>().TakeDamage(bg.skills[0].Damage, bg.skills[0].degats);
            PhotonNetwork.Destroy(gameObject);
        }

    }
}
