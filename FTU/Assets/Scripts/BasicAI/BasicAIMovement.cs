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
    protected Vector3 posToGo;

    public void MovementAI(Transform[] moveTo)
    {
        if (GetCanMove() && GetCanAct())
        {
            if (_navMeshAgent.remainingDistance <= 1f && current < moveTo.Length - 1)
            {
                current++;
            }
            _navMeshAgent.SetDestination(new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z)); 

        }
    }

    public IEnumerator WalkToward()
    {
        while (Cible)
        {
            SetIsMoving(true);
            _navMeshAgent.SetDestination(Cible.transform.position);
            yield return new WaitForSeconds(0.1f);
            if (_navMeshAgent.remainingDistance < GetAttackRange())
            {
                SetIsMoving(false);
                _navMeshAgent.ResetPath();
            }
            if(_navMeshAgent.remainingDistance > 40f)
            {
                Cible = null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
