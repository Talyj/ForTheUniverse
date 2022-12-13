using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : PlayerMovement
{
    //Animator anim;

    public float gold;
    [HideInInspector] public Vector3 respawnPos;
    [HideInInspector] public Vector3 deathPos;
    private float respawnCooldown;

    public List<ItemBehaviours> items = new List<ItemBehaviours>(4);
    public Role role;
    //[SerializeField]



    #region Getter and Setter

    #region Getter


    public Skills[] GetSkills()
    {
        return skills;
    }
    public Skills GetSkill1()
    {
        return skills[0];
    }
    public Skills GetSkill2()
    {
        return skills[1];
    }
    public Skills GetUlt()
    {
        return skills[2];
    }    
    #endregion
    #region Setter
    
    #endregion

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();

    }

    public void PlayerStatsSetUp()
    {
        SetMaxExp(100);
        ExpRate = 1.85f;
        SetExp(0);
        SetLvl(1);
        respawnCooldown = 10.0f;
        SetEnemyType(EnemyType.player);

        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].isCooldown = false;
        }
    }    

    public new void HealthBehaviour()
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
            photonView.RPC("GiveExperience", RpcTarget.All, new object[] { });
            var rend = GetComponents<Renderer>();
            if (rend != null)
            {
                for (int i = 0; i < rend.Length; i++)
                {
                    rend[i].enabled = false;
                }

            }
            StartCoroutine(Spawn(rend));
        }
        Regen();
    }

    IEnumerator Spawn(Renderer[] rend)
    {
        if (transform.position != deathPos)
        {
            transform.position = deathPos;
            yield return new WaitForSeconds(respawnCooldown);

            SetDefault(rend);
            transform.position = respawnPos;
        }
    }

    public void ItemEquip()
    {
        foreach(var item in items)
        {
            SetMaxHealth(GetMaxHealth()  +item.health);
            SetMaxMana(GetMaxMana()  +item.mana);
            SetAttackSpeed(GetAttackSpeed() + item.attackSpeed);
            SetResMag(GetResMag() + item.resMag);
            SetResPhys(GetResPhys() + item.resPhys);
            SetDegPhys(GetDegPhys() + item.dmgPhys);
            SetDegMag(GetDegMag() + item.dmgMag);
        }
    }    

    public float DamageMultiplier(float dmgSource, float dmgMultiplier)
    {
        var res = dmgSource * dmgMultiplier;
        return res;
    }

    public void SetDefault(Renderer[] rend)
    {
        SetHealth(GetMaxHealth());
        SetMana(GetMaxMana());
        //deathEffect.SetActive(false);
        //for (int i = 0; i < disableOnDeath.Length; i++)
        //{
        //    disableOnDeath[i].enabled = true;
        //}
        for (int i = 0; i < rend.Length; i++)
        {
            rend[i].enabled = true;
        }
    }

    //SPE = true => Magique
    protected void SetUpCharacters(Role role, bool range, bool spe)
    {
        SetMaxMana(500f);
        switch (role)
        {
            case Role.dps:
                SetMaxHealth(500f);
                SetAttackSpeed(2f);

                SetupAttackRangeAndSpeed(range);

                if (spe)
                {
                    SetResMag(50f);
                    SetResPhys(50f);

                    SetDegMag(100f);
                    SetDegPhys(50f);
                }
                else
                {
                    SetResMag(50f);
                    SetResPhys(50f);

                    SetDegMag(50f);
                    SetDegPhys(100f);
                }
                break;
            case Role.tank:
                SetMaxHealth(750f);
                SetAttackSpeed(1f);

                SetupAttackRangeAndSpeed(range);

                if (spe)
                {
                    SetResMag(100f);
                    SetResPhys(60f);

                    SetDegMag(50f);
                    SetDegPhys(50f);
                }
                else
                {
                    SetResMag(50f);
                    SetResPhys(60f);

                    SetDegMag(50f);
                    SetDegPhys(60f);
                }
                break;
            case Role.support:
                SetMaxHealth(450f);
                SetAttackSpeed(1f);

                SetupAttackRangeAndSpeed(range);

                if (spe)
                {
                    SetResMag(60f);
                    SetResPhys(50f);

                    SetDegMag(60f);
                    SetDegPhys(50f);
                }
                else
                {
                    SetResMag(50f);
                    SetResPhys(60f);

                    SetDegMag(50f);
                    SetDegPhys(60f);
                }
                break;
        }        
    }

    private void SetupAttackRangeAndSpeed(bool range)
    {
        if (range)
        {
            SetAttackRange(30f);
            SetMoveSpeed(15f);
        }
        else
        {
            SetAttackRange(10f);
            SetMoveSpeed(20f);
        }
    }

    public void SetupDegResMagPhys(bool spe)
    {

    }
}

//Enum To setup characters
public enum Role
{
    dps = 0,
    support = 1,
    tank = 2
}
