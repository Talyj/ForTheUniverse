using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsBehaviour : BasicAIStats
{
    //0 = Life, 1 = Mana
    public int asteroidType;
    private float range;
    private List<IDamageable> targets;
    private float cpt;
    private VoidManager VM;

    // Start is called before the first frame update
    void Start()
    {
        targets = new List<IDamageable>();
        range = asteroidType == 0 ? 20 : 40;
        BaseInit();
        SetupForAI();
        cpt = 0;
        VM = FindObjectOfType<VoidManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cpt -= Time.deltaTime;
            HealthBehaviour();
            if (GetHealth() > 0 && GetCanAct() && GetCanMove())
            {
                if(cpt <= 0)
                {
                    cpt = 1f;
                    var colliders = Physics.OverlapSphere(transform.position, range);
                    RefreshTargets(colliders);
                    if(asteroidType == 0)
                    {
                        DamageLife();
                    }
                    else { DamageMana(); }
                }
            }
        }
    }

    public override void HealthBehaviour()
    {
        if (GetHealth() >= GetMaxHealth())
        {
            SetHealth(GetMaxHealth());
        }
        if (GetMana() >= GetMaxMana())
        {
            SetMana(GetMaxMana());
        }

        if (GetHealth() <= 0)
        {
            VM.nbAsteroide--;
            //PhotonView.Get(this).RPC("SendKillfeed", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, Cible.name);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void DamageLife()
    {
        foreach (var target in targets)
        {
            target.GetComponent<IDamageable>().TakeDamage(50, DamageType.brut);
        }
    }

    private void DamageMana()
    {
        foreach (var target in targets)
        {
            target.GetComponent<IDamageable>().TakeDamage(50, DamageType.brut, true);
        }
    }

    private void RefreshTargets(Collider[] hitColliders)
    {
        var tempList = new List<IDamageable>();
        if (hitColliders != null)
        {
            foreach (var col in hitColliders)
            {
                if (col.gameObject == this.gameObject) break;
                //If the target is a player
                if (col.GetComponent<PlayerStats>())
                {
                    tempList.Add(col.GetComponent<IDamageable>());
                }
            }
        }
        targets = tempList;
    }
}
