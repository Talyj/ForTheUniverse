using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabKingBehaviour : KingsBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        followersTag = "crab";
        BaseSetupKing();
        BaseInit();
        AISetup();

        //Stat to change
        SetDegMag(30f);
        SetDegPhys(30f);
        SetResMag(50f);
        SetResPhys(50f);
        SetAttackSpeed(10f);
        SetAttackRange(10f);
        SetMaxHealth(500f);

        SetMoveSpeed(20f);
        SetViewRange(30f);
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
