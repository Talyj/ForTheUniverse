using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldState;

public class Action
{
    public float utilityScore;
    public string name;
    public VoisterAction action;

    public Action() {    }

    public Action(string actionName, VoisterAction actionValue)
    {
        this.name = actionName;
        action = actionValue;
    }

    public bool CanDo(WorldState ws)
    {
        if (ws.GetValue((int)VoisterAction.FOOD) > FOODREQUIRED[(int)action] && ws.GetValue((int)VoisterAction.FEED) >= FEEDERREQUIRED[(int)action]) return true;

        return false;
    }

    public float UpdateValue(WorldState ws, VoisterAction action) 
    { 
        //var test = (((float)ws.GetValue((int)action) / 100) -1) * -1;
        //var test1 = ((float)OBJECTIVEQTY[(int)action] / 100) / 2;
        return (((float)ws.GetValue((int)action) / 100) - 1) * -1 + ((float)OBJECTIVEQTY[(int)action] / 100) / 2;
    }
        

}
