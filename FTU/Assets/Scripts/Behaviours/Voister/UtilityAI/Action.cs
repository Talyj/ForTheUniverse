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
        if (ws.GetValue((int)VoisterAction.FOOD) > FOODREQUIRED[(int)action]) return true;

        return false;
    }

    public float UpdateValue(WorldState ws, VoisterAction action) => ((((float)ws.GetValue((int)action) / 100) - 1) * -1) + ((float)(FOODREQUIRED[(int)action] / 100) / 2);

}
