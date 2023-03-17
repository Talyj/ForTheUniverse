using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAIMovement : BasicAIStats, IPunObservable
{
    //Movement AI
    public int current;
    public bool pathDone;

    public void MovementAI(Transform[] moveTo)
    {
        if (GetCanMove() && GetCanAct())
        {
            if (Vector3.Distance(transform.position, moveTo[current].position) > 10)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z), GetMoveSpeed() * Time.deltaTime);
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
