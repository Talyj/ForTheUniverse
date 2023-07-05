using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : VoisterBehaviour, IPunObservable
{
    public void Start()
    {
        BaseInit();
        AISetup();
        VoisterStatsSetup();
        current = 0;



        //Stat to change
        SetDegMag(30f);
        SetDegPhys(30f);
        SetResMag(50f);
        SetResPhys(50f);
        SetAttackSpeed(100f);
        SetAttackRange(30f);
        SetMaxHealth(500f);

        SetMoveSpeed(20f);
        SetViewRange(30f);
        isAttacking = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            HealthBehaviour();
            ExperienceBehaviour();
            VoisterBaseBehaviour();
            //MovementTraining();
            //SurviveTraining();
            CheckTarget();


            if (GetCanAct() && GetCanMove())
            {
                VoisterBaseBehaviour();
                GetNearestTarget();
                //if (Cible)
                //{
                //    WalkToward();
                //    gameObject.transform.LookAt(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
                //    VoisterBasicAttack();
                //}
                //Movement + attack
                //VoisterMovement();
            }
        }
    }

    //public void FixedUpdate()
    //{
    //    if (PhotonNetwork.IsMasterClient) SurviveTraining();
    //}

    public void BouleBoom()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost && Cible != null)
        {
            //buff
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");

            var targ = Cible.GetComponent<IDamageable>();
            var enemyClose = false;
            
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 10f);

            if (hitColliders != null)
            {
                var dmgDealt = GetHealth() <= GetMaxHealth() * 0.2 ? targ.TakeDamage(GetDegMag() * 2f, DamageType.magique, photonView.ViewID) : targ.TakeDamage(GetDegMag() * 1.5f, DamageType.magique, photonView.ViewID);
                //calc dmg
                foreach (var col in hitColliders)
                {
                    if (col.GetComponent<IDamageable>())
                    {
                        if(Vector3.Distance(gameObject.transform.position, col.gameObject.transform.position) < 10f)
                        {
                            enemyClose = true;
                            break;
                        }
                    }
                }

                //deal dmg
                Collider[] hitCollidersTargets = Physics.OverlapSphere(Cible.gameObject.transform.position, 10f);
                if (enemyClose)
                {
                    targ.TakeDamage(GetDegMag() * 2f, DamageType.magique,photonView.ViewID);
                    foreach(var col in hitCollidersTargets)
                    {
                        if (col.GetComponent<IDamageable>())
                        {
                            col.GetComponent<IDamageable>().TakeDamage(GetDegMag() * 1.2f, DamageType.magique,photonView.ViewID);
                        }
                    }
                }
                else
                {
                    targ.TakeDamage(GetDegMag() * 1.5f, DamageType.magique, photonView.ViewID);
                    foreach (var col in hitColliders)
                    {
                        if (col.GetComponent<IDamageable>())
                        {
                            col.GetComponent<IDamageable>().TakeDamage(GetDegMag() * 1f, DamageType.magique, photonView.ViewID);
                        }
                    }
                }
            }

            StartCoroutine(CoolDown(skills[0]));
        }
    }

    
}
