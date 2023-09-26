using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    //QTY REQUIRED
    public static List<int> FOODREQUIRED = new List<int>() {2, 10, 15, 20};
    //public static List<int> FEEDERREQUIRED = new List<int>() { 0, 2, 0, 0 };
    //public static List<int> GARDREQUIRED = new List<int>() { 0, 0, 3, 0 };
    //public static List<int> PATROLREQUIRED = new List<int>() { 0, 0, 0, 3 };

    //OBJECTIVE QTY
    public const int MAXFEED = 10;
    public const int MAXGARD = 5;
    public const int MAXPATROL = 10;
    public const int MAXATTACK = 20;
    public static List<int> OBJECTIVEQTY = new List<int>() { 10, 5, 10, 20 };



    public int food, elderFeeder, elderGard, elderPatrol, elderAttack;
    //public (float, float) DomVerKill;

    //public List<(float, float)> DOMVERKILL = new List<(float, float)>();

    public enum VoisterAction
    {
        FEED = 0,
        GARD = 1,
        PATROL = 2,
        ATTACK = 3,
        FOOD = 10,
        NONE = 999
    }

    public WorldState(int foodValue)
    {
        //Init world values
        food = foodValue;
        elderFeeder = 0;
        elderGard = 0;
        elderPatrol = 0;
        elderAttack = 0;
        //DomVerKill = (0.5f, 0.5f);
    }

    public void IncrementValue(int id)
    {
        switch (id)
        {
            //Feeder
            case 0:
                elderFeeder++;
                break;
            //Gard
            case 1:
                elderGard++;
                break;
            //Patrol
            case 2:
                elderPatrol++;
                break;
            //Attacker
            case 3:
                elderAttack++;
                break;
            //food
            case 10:
                food++;
                break;
        }
        DecreaseValue(id);
    }

    public void DecreaseValue(int ValueIncremented)
    {
        if (ValueIncremented > FOODREQUIRED.Count) return;

        food -= FOODREQUIRED[ValueIncremented];
        //elderFeeder -= FEEDERREQUIRED[ValueIncremented];
        //elderGard -= GARDREQUIRED[ValueIncremented];
        //elderPatrol -= PATROLREQUIRED[ValueIncremented];
    }

    public int GetValue(int id)
    {
        switch (id)
        {
            //Feeder
            case 0:
                return elderFeeder;
            //Gard
            case 1:
                return elderGard;
            //Patrol
            case 2:
                return elderPatrol;
            //Attacker
            case 3:
                return elderAttack;
            //food
            case 10:
                return food;
            default:
                return 0;
        }
    }
}
