using Photon.Pun;
using UnityEngine;

public class TrainingBehaviours : BasicAIMovement, IPunObservable
{
    public void Start()
    {
        BaseInit();
        current = 0;
        pathDone = true;
        if (attackType == AttackType.Ranged)
        {
            SetAttackRange(20f);
            SetMaxHealth(250f);
        }
        else
        {
            SetAttackRange(10f);
            SetMaxHealth(300f);
            SetHealth(300f);
        }
        SetMoveSpeed(20f);
        SetDegMag(20f);
        SetDegPhys(20f);
        SetViewRange(30f);
        SetAttackSpeed(2f);
        SetEnemyType(EnemyType.minion);
        isAttacking = false;
    }

    public void Update()
    {
       
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
