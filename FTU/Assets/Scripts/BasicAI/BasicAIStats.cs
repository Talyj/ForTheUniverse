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
        SetupForAI();
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
                    if (col.gameObject == this.gameObject) return;
                    //If the target is a player
                    if (col.GetComponent<PlayerStats>())
                    {
                        if (col.GetComponent<PlayerStats>().team.Code != team.Code)
                        {
                            Cible = col.gameObject;
                            break;
                        }
                    }//If the target is a minion, a golem, a voisters or a dd
                    else if (col.GetComponent<BasicAIStats>())
                    {
                        if(col.GetComponent<BasicAIStats>().team.Code != team.Code)
                        {
                            Cible = col.gameObject;
                        }
                    }
                }
            }
        }
        CheckTarget();
    }

    public void BasicAttackIA()
    {
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
        if (team.Code == 1)
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
        var isMissing = ReferenceEquals(Cible, null) ? false : (Cible ? false : true);
        if (isMissing) Cible = null;

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
