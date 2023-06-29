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
            if (_navMeshAgent.remainingDistance <= 1f && current < moveTo.Length -1)
            {
                current++;//= (current + 1)/* % targets.Length*/;
            }
            _navMeshAgent.SetDestination(new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z)); 
            //if (current >= moveTo.Length) current = 0;             
               
            

            /*if(_navMeshAgent.remainingDistance > 10)
            {
                _navMeshAgent.SetDestination(new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z));
            }
            else
            {
                current++;
                if (current >= moveTo.Length) current = 0;
            }*/
        }
    }

    //public void WalkToward()
    //{
    //    try
    //    {
    //        //if (_navMeshAgent.remainingDistance <= GetAttackRange() + 5)
    //        //{
    //        //    _navMeshAgent.ResetPath();
    //        //    SetIsMoving(false);
    //        //}
    //        //else if(Vector3.Distance(transform.position, Cible.transform.position) > GetAttackRange())
    //        //{
    //        //    _navMeshAgent.SetDestination(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
    //        //    SetIsMoving(true);
    //        //}
    //        SetIsMoving(true);
    //        _navMeshAgent.SetDestination(Cible.transform.position);
    //        if(_navMeshAgent.remainingDistance <= 2f)
    //        {
    //            SetIsMoving(false);
    //            _navMeshAgent.ResetPath();
    //        }
    //    }
    //    catch (NullReferenceException e)
    //    {
    //        Cible = null;
    //    }
    //}

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
