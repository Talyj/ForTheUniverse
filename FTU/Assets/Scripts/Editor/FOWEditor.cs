using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FogOfWar))]
public class FOWEditor : Editor
{

    private void OnSceneGUI()
    {
        FogOfWar fog =  (FogOfWar)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(fog.transform.position, Vector3.up, Vector3.forward, 360, fog.viewRadius);
        Vector3 viewAngleA = fog.DirFromAngle(-fog.viewAngle / 2, false);
        Vector3 viewAngleB = fog.DirFromAngle(fog.viewAngle / 2, false);

        Handles.DrawLine(fog.transform.position, fog.transform.position + viewAngleA * fog.viewRadius);
        Handles.DrawLine(fog.transform.position, fog.transform.position + viewAngleB * fog.viewRadius);


        Handles.color = Color.red;
        foreach(Transform visibleTarget in fog.visibleTarget)
        {
            Handles.DrawLine(fog.transform.position, visibleTarget.position);
        }
    }
}
