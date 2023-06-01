using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxBehaviour : VoisterBehaviour, IPunObservable
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
            VoisterBaseAction();
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

    public void Stab()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost && Cible != null)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");

            var targ = Cible.GetComponent<IDamageable>();
            var dmgDealt = targ.GetHealth() <= targ.GetMaxHealth()/2 ? targ.TakeDamage(GetDegPhys() * 2.5f, DamageType.brut) : targ.TakeDamage(GetDegPhys() * 1.5f, DamageType.brut);
            Debug.Log(dmgDealt);

            StartCoroutine(CoolDown(skills[0]));
        }
    }
}
