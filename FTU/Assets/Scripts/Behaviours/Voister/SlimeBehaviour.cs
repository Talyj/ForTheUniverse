using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : VoisterBehaviour, IPunObservable
{
    // Start is called before the first frame update
    public void Start()
    {
        current = 0;
        BaseInit();
        VoisterStatsSetup();
        AISetup();
        //Stat to change
        SetDegMag(50f);
        SetDegPhys(50f);
        SetResMag(20f);
        SetResPhys(20f);
        SetAttackSpeed(10f);
        SetAttackRange(20f);
        SetMaxHealth(200f);

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
                GetNearestTarget();
                if (Cible)
                {
                    WalkToward();
                    gameObject.transform.LookAt(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
                    VoisterBasicAttack();
                }
                //Movement + attack
                VoisterMovement();
            }
        }
    }

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
                var dmgDealt = GetHealth() <= GetMaxHealth() * 0.2 ? targ.TakeDamage(GetDegMag() * 2f, DamageType.magique) : targ.TakeDamage(GetDegMag() * 1.5f, DamageType.magique);
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
                    targ.TakeDamage(GetDegMag() * 2f, DamageType.magique);
                    foreach(var col in hitCollidersTargets)
                    {
                        if (col.GetComponent<IDamageable>())
                        {
                            col.GetComponent<IDamageable>().TakeDamage(GetDegMag() * 1.2f, DamageType.magique);
                        }
                    }
                }
                else
                {
                    targ.TakeDamage(GetDegMag() * 1.5f, DamageType.magique);
                    foreach (var col in hitColliders)
                    {
                        if (col.GetComponent<IDamageable>())
                        {
                            col.GetComponent<IDamageable>().TakeDamage(GetDegMag() * 1f, DamageType.magique);
                        }
                    }
                }
            }

            StartCoroutine(CoolDown(skills[0]));
        }
    }
}
