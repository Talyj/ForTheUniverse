using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompetences : IHasHealth
{
    public void Start()
    {
        SetHealth(500);
    }
}
