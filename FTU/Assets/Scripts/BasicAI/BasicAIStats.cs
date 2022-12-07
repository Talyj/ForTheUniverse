using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAIStats : IDamageable, IPunObservable
{
    //Path Auto
    public Transform[] targetsUp;
    public Transform[] targetsDown;
    private Transform[] tempArray;
    public Way way;

    public enum Way
    {
        up,
        down
    }

    protected void AISetup()
    {
        isAI = true;
    }

    public void GetNearestTarget()
    {
        if (Cible == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, GetViewRange());
            if (hitColliders != null)
            {
                foreach (var col in hitColliders)
                {
                    //If the target is a player
                    if (col.GetComponent<PlayerStats>())
                    {
                        if (col.GetComponent<PlayerStats>().team != team)
                        {
                            Cible = col.gameObject;
                            break;
                        }
                    }//If the target is a minion, a golem or a dd
                    else if (col.GetComponent<BasicAIStats>())
                    {
                        if(col.GetComponent<BasicAIStats>().team != team)
                        {
                            Cible = col.gameObject;
                        }
                    }
                    //else if (col.GetComponent<VoisterStats>())
                    //{
                    //    Cible = col.gameObject;
                    //}
                }
            }
        }
    }

    public void BasicAttackIA()
    {
        WalkToward();
        if (attackType == AttackType.Melee)
        {
            StartCoroutine(AutoAttack());
        }
        if (attackType == AttackType.Ranged)
        {
            StartCoroutine(RangeAutoAttack());
        }
    }

    public Transform[] whichTeam(Transform[] way)
    {
        tempArray = new Transform[way.Length];
        if (team == Team.Veritas)
        {
            for (int i = 0; i < way.Length; i++)
            {
                tempArray[way.Length - 1 - i] = way[i];
            }
            return tempArray;
        }
        return way;
    }

    public void CheckTarget()
    {
        if (Cible == null)
        {
            Cible = null;
        }

        if (Cible != null)
        {
            if (Cible.CompareTag("Player"))
            {
                if (Cible.GetComponent<IDamageable>().GetHealth() <= 0)
                {
                    Cible = null;
                }
            }
        }
    }
}
