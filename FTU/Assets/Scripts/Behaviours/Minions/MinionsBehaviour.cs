using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsBehaviour : PlayerStats
{
    //order for the path => right to left
    public Transform[] targetsUp;
    public Transform[] targetsDown;

    private Transform[] tempArray;

    private int current;
    private bool pathDone;

    private bool isAttacking;

    public Way way;
    public enum Way
    {
        up,
        down
    }

    public void Start()
    {
        Init();
        current = 0;
        pathDone = false;
        if (attackType == AttackType.Ranged)
        {
            AttackRange = 30f;
        }
        else AttackRange = 10f;
        isAttacking = false;
    }

    public void Update()
    {
        if (!isAttacking && Cible != null)
        {
            isAttacking = true;
            StartCoroutine(AttackSystem());
        }
        if(!pathDone && !isAttacking && Cible == null)
        {
            isAttacking = false;
            if (way == Way.up)
            {
                Movement(whichTeam(targetsUp));
            }
            else Movement(whichTeam(targetsDown));
        }
    }

    public Transform[] whichTeam(Transform[] way)
    {
        tempArray = new Transform[way.Length];
        if (team == Team.Veritas)
        {
            for(int i = 0; i < way.Length; i++)
            {
                tempArray[way.Length - 1 - i] = way[i];
            }
            return tempArray;
        }
        return way;
    }

    public void Movement(Transform[] moveTo)
    {
        if (canMove && canAct && Cible == null)
        {
            if (transform.position != moveTo[current].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTo[current].position, MoveSpeed * Time.deltaTime);
            }
            else current = (current + 1)/* % targets.Length*/;
        }
        if (current == moveTo.Length) pathDone = true;
    }

    public void WalkToward()
    {
        while(transform.position != Cible.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, Cible.transform.position, MoveSpeed * Time.deltaTime);
        }
    }

    public new IEnumerator AttackSystem()
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
        yield return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("minion") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("golem"))
        {
            Cible = other.gameObject;
        }
    }
}
