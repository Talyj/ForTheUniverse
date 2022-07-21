using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsBehaviour : PlayerStats, IPunObservable
{
    public float dstForXp;
    public float xpAmount;
    bool gaveXp;
    public void Start()
    {
        gaveXp = false;
        Init();
        current = 0;
        pathDone = false;
        if (attackType == AttackType.Ranged)
        {
            SetAttackRange(20f);
        }
        else SetAttackRange(10f);
        SetMoveSpeed(30f);
        SetDegMag(500f);
        SetDegPhys(500f);
        SetViewRange(30f);
        SetAttackSpeed(2f);
        isAttacking = false;
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            HealthBehaviour();
            CheckTarget();

            if (GetCanAct() && GetCanMove())
            {
                GetNearestTarget();
                if (Cible)
                {
                    //StartCoroutine(WalkToward());
                    WalkToward();
                    gameObject.transform.LookAt(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
                }        
                //Movement + attack
                DefaultMinionBehaviour();
            }
            if (GetHealth() <= 0 && !gaveXp)
            {
                gaveXp = true;
                GiveExp();
            }            
        }
    }

    public void GiveExp()
    {


        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, dstForXp);

        if (hitColliders != null)
        {
            foreach (var col in hitColliders)
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    if (col.gameObject.GetComponent<IDamageable>().team != team)
                    {
                        col.gameObject.GetComponent<IDamageable>().SetExp( xpAmount);
                        col.gameObject.GetComponent<IDamageable>().gold += 100;
                        Debug.Log(xpAmount + "<Color=green><a> Xp </a></Color>");
                    }
                    else
                    {
                        Debug.Log("<Color=red><a>Same Team</a></Color>");
                    }
                }

            }
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GetHealth());
        }
        else
        {
            SetHealth((float)stream.ReceiveNext());
        }
    }

    //public void ExpFor()
    //{
    //    Instantiate(xp, transform.position, Quaternion.identity);
    //    Destroy(gameObject);
    //}
}
