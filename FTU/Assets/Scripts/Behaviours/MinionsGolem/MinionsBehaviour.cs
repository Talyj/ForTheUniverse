using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

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

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("No NavMeshAgent attached to " + gameObject.name);
        }
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            HealthBehaviour();
            CheckTarget();

            if (GetHealth() > 0 && GetCanAct() && GetCanMove())
            {
                GetNearestTarget();
                DefaultMovement();
                if (Cible)
                {
                    WalkToward();
                    DefaultAttack();
                    gameObject.transform.LookAt(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
                }
            }
            //TODO this is made for test have to get rid of the lines later
            //cpt += Time.deltaTime;
            //if (cpt >= 30)
            // {
            //    cpt = 0;
            //    PhotonNetwork.Destroy(gameObject);
            //}
        }

    }

    private void DefaultAttack()
    {
        if (Cible == null)
        {
            isAttacking = false;
        }

        if (!isAttacking && Cible != null)
        {
            isAttacking = true;
            BasicAttackIA();
        }
    }

    private void DefaultMovement()
    {       
        if (!pathDone && !isAttacking && Cible == null)
        {
            if (way == Way.up)
            {
                MovementAI(whichTeam(targetsUp));
            }
            else MovementAI(whichTeam(targetsDown));
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GetHealth());
            //stream.SendNext(gameObject.GetComponent<Renderer>().material.color.r);
            //stream.SendNext(gameObject.GetComponent<Renderer>().material.color.g);
            //stream.SendNext(gameObject.GetComponent<Renderer>().material.color.b);
        }
        else
        {
            SetHealth((float)stream.ReceiveNext());
            //gameObject.GetComponent<Renderer>().material.color = new Color((float)stream.ReceiveNext(), (float)stream.ReceiveNext(), (float)stream.ReceiveNext());
        }
    }
}
