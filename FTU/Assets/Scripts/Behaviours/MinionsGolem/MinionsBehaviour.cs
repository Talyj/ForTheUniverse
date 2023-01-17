using Photon.Pun;
using UnityEngine;

public class MinionsBehaviour : BasicAIMovement, IPunObservable
{
    private float cpt = 0;

    public void Start()
    {
        BaseInit();
        AISetup();
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
        SetMoveSpeed(15f);
        SetDegMag(20f);
        SetDegPhys(20f);
        SetViewRange(30f);
        SetAttackSpeed(2f);
        SetEnemyType(EnemyType.minion);
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
                    WalkToward();
                    gameObject.transform.LookAt(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
                }        
                //Movement + attack
                if(GetHealth() > 0)
                {
                    DefaultMinionBehaviour();
                }
            }
            //TODO this is made for test have to get rid of the lines later
            cpt += Time.deltaTime;
            if (cpt >= 30)
             {
                cpt = 0;
                PhotonNetwork.Destroy(gameObject);
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
