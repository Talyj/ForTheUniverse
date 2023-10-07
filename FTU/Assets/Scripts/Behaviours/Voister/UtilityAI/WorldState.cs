using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    //QTY REQUIRED
    public static List<int> FOODREQUIRED = new List<int>() {3, 5, 15, 20};
    public static List<int> FEEDERREQUIRED = new List<int>() { 0, 3, 5, 7 };
    public int foodNeeded;

    //OBJECTIVE QTY
    //15, 10, 5, 5
    public static List<int> OBJECTIVEQTY = new List<int>() { 15, 10, 5, 5 };

    //CURRENT QTY
    public List<int> currentQty = new List<int>() { 0, 0, 0, 0 };



    public int food;
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
        //DomVerKill = (0.5f, 0.5f);
    }

    public void IncrementValue(int id)
    {
        switch (id)
        {
            //Feeder
            case 0:
                currentQty[(int)VoisterAction.FEED]++;
                break;
            //Gard
            case 1:
                currentQty[(int)VoisterAction.GARD]++;
                break;
            //Patrol
            case 2:
                currentQty[(int)VoisterAction.PATROL]++;
                break;
            //Attacker
            case 3:
                currentQty[(int)VoisterAction.ATTACK]++;
                break;
            //food
            case 10:
                food += Random.Range(1, 3);
                break;
        }
        DecreaseValue(id);
    }

    public void DecreaseValue(int ValueIncremented)
    {
        if (ValueIncremented == 10) return;

        food -= FOODREQUIRED[ValueIncremented];
    }

    public int GetValue(int id)
    {
        switch (id)
        {
            //Feeder
            case 0:
                return currentQty[(int)VoisterAction.FEED];
            //Gard
            case 1:
                return currentQty[(int)VoisterAction.GARD];
            //Patrol
            case 2:
                return currentQty[(int)VoisterAction.PATROL];
            //Attacker
            case 3:
                return currentQty[(int)VoisterAction.ATTACK];
            //food
            case 10:
                return food;
            default:
                return 0;
        }
    }
}
