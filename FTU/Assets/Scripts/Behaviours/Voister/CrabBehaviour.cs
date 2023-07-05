using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabBehaviour : VoisterBehaviour, IPunObservable
{
    // Start is called before the first frame update
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
                //MovementTraining();
                //SurviveTraining();
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

    public void LifeSteal()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost && Cible != null)
        {
            //buff
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");

            var targ = Cible.GetComponent<IDamageable>();
            var dmgDealt = GetHealth() <= GetMaxHealth() * 0.2 ? targ.TakeDamage(GetDegMag() * 2f, DamageType.magique, photonView.ViewID) : targ.TakeDamage(GetDegMag() * 1.5f, DamageType.magique, photonView.ViewID);
            SetHealth(GetHealth() + (dmgDealt / 2));

            StartCoroutine(CoolDown(skills[0]));
        }
    }
}
