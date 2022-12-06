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
            SetMaxHealth(250f);
        }
        else 
        {
            SetAttackRange(10f);
            SetMaxHealth(350);
        } 
        SetMoveSpeed(20f);
        SetDegMag(20f);
        SetDegPhys(20f);
        SetViewRange(30f);
        SetAttackSpeed(2f);
        isAttacking = false;
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            HealthBehaviour();
            if(GetHealth() > 20)
            {
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
            }        
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GetHealth());
            stream.SendNext(gameObject.GetComponent<Renderer>().material.color.r);
            stream.SendNext(gameObject.GetComponent<Renderer>().material.color.g);
            stream.SendNext(gameObject.GetComponent<Renderer>().material.color.b);
        }
        else
        {
            SetHealth((float)stream.ReceiveNext());
            gameObject.GetComponent<Renderer>().material.color = new Color((float)stream.ReceiveNext(), (float)stream.ReceiveNext(), (float)stream.ReceiveNext());
        }
    }
}
