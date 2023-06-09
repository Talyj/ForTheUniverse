using Photon.Pun;
using System;
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
        if (GetCanMove() && GetCanAct())
        {
            //if (_navMeshAgent.remainingDistance <= 10f)
            //{
            //    current = (current + 1)/* % targets.Length*/;
            //}
            //if (current >= moveTo.Length) current = 0;             
            //_navMeshAgent.SetDestination(new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z));    
            

            if(_navMeshAgent.remainingDistance > 10)
            {
                _navMeshAgent.SetDestination(new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z));
            }
            else
            {
                current++;
                if (current >= moveTo.Length) current = 0;
            }
        }
    }

    public void WalkToward()
    {
        try
        {
            if (_navMeshAgent.remainingDistance <= GetAttackRange() - 1)
            {
                _navMeshAgent.ResetPath();
                SetIsMoving(false);
            }
            else if(Vector3.Distance(transform.position, Cible.transform.position) > GetAttackRange())
            {
            }
                _navMeshAgent.SetDestination(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
                SetIsMoving(true);
        }
        catch (NullReferenceException e)
        {
            Cible = null;
        }
    }
}
