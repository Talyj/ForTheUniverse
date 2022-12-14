using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoisterBehaviour : BasicAIMovement, IPunObservable
{
    protected void VoisterStatsSetup()
    {
        SetEnemyType(EnemyType.voister);
        SetMaxMana(500);
        SetMana(500);
        SetMaxExp(100);
        ExpRate = 1.85f;
        SetExp(0);
        SetLvl(1);
    }

    protected void VoisterBaseAction()
    {
        Regen();
    }

    protected void VoisterMovement()
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

    protected void VoisterBasicAttack()
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
}
