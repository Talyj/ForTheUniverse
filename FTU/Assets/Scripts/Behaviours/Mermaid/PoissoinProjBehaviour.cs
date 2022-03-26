using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissoinProjBehaviour : Projectile
{
    public MermaidBehaviour source;

    public new void Update()
    {
        Behaviour();
        if(touched == true)
        {
            source.AddPassive();
        }
    }
}
