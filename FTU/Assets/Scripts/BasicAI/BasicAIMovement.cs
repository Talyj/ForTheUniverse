using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAIMovement : BasicAIStats, IPunObservable
{
    //Movement AI
    public int current;
    public bool pathDone;
    protected NavMeshAgent _navMeshAgent;
    protected Vector3 posToGo;

    public void MovementAI(Transform[] moveTo)
    {
        //if (GetCanMove() && GetCanAct())
        //{
        //    if (Vector3.Distance(transform.position, moveTo[current].position) > 10)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z), GetMoveSpeed() * Time.deltaTime);
        //    }
        //    else current = (current + 1)/* % targets.Length*/;
        //}
        //if (current == moveTo.Length) current = 0;

        if (GetCanMove() && GetCanAct())
        {
            if (_navMeshAgent.remainingDistance < 5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z), GetMoveSpeed() * Time.deltaTime);
                _navMeshAgent.SetDestination(new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z));
            }
            else current = (current + 1)/* % targets.Length*/;
        }
        if (current == moveTo.Length) current = 0;
    }

    public void DefaultMinionBehaviour()
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

        if (!pathDone && !isAttacking && Cible == null)
        {
            if (way == Way.up)
            {
                MovementAI(whichTeam(targetsUp));
            }
            else MovementAI(whichTeam(targetsDown));
        }
    }
}
