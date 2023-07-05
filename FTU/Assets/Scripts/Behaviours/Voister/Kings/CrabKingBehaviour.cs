using Photon.Pun;
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
        cpt = 10;

        //Training
        //InitNetworks();
        //InvokeRepeating(nameof(CreateVoisters), 0.1f, timeframe);//repeating function
    }

    // Update is called once per frame
    void Update()
    {
        BaseBehaviourKings();

        if (PhotonNetwork.IsMasterClient)
        {
            cpt -= Time.deltaTime;
            if (cpt <= 0)
            {
                cpt = delaySpawn;
                var randQty = Random.Range(1, 3);
                for (int i = 0; i < randQty; i++)
                {
                    SpawnVoisters(voister);
                }
            }
        }
    }
}
