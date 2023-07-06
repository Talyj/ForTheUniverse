using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : PlayerMovement
{
    //Animator anim;

    public float gold;
    public Vector3 respawnPos;
    public Vector3 deathPos;
    public PlayerManager playerManage;
    private float respawnCooldown;
    private bool isDead = false;

    [Header("KDA")]
    public float kill ;
    public float death;
    public float assist;
    public float  creep;

    public List<ItemStats> items = new List<ItemStats>(4);
    public Role role;



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

    public float GetGold()
    {
        return gold;
    }
    #endregion
    #region Setter
    public void SetGold(float value)
    {
        gold += value;
    }
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
        SetGold(500);
        respawnCooldown = 10.0f;
        SetEnemyType(EnemyType.player);
        inFight = false;
        isDead = false;
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].isCooldown = false;
        }
    }

    public new void HealthBehaviour()
    {
        if(GetHealth() < GetMaxHealth())
        {
            healthDecreaseTimer += Time.deltaTime;
        }
        inFight = (healthDecreaseTimer >= 6f || healthDecreaseTimer <= -1f ) ? false : true;

        if (inFight == false)
        {
            Regen();
        }
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
            //photonView.RPC("GiveExperience", RpcTarget.All, new object[] { });
            var rend = GetComponents<Renderer>();
            foreach (Transform child in transform)
            {
                Renderer rChild;
                if (child.TryGetComponent<Renderer>(out rChild))
                {
                    rChild.enabled = false;
                }
            }
            if (rend != null)
            {
                for (int i = 0; i < rend.Length; i++)
                {
                    rend[i].enabled = false;
                }

                if (gameObject.CompareTag("dd"))
                {
                    //Victory
                }
                else if (gameObject.GetComponent<BasicAIStats>())
                {
                    //userId = gameObject.name;
                    //PhotonView.Get(this).RPC("SendKillfeed", RpcTarget.AllBuffered,  Cible.name, userId);
                    //PhotonView.Get(this).RPC("RPC_ReceiveKillfeed", RpcTarget.All,userId, Cible.name);
                    PhotonNetwork.Destroy(gameObject);
                }
                else if (gameObject.GetComponent<PlayerStats>())
                {
                    if (!isDead)
                    {
                        if (idLastDamageTaken > 0)
                        {
                            photonView.RPC("RPC_SendKillfeed", RpcTarget.AllBuffered, PhotonView.Find(idLastDamageTaken).gameObject.name, gameObject.name);
                        }
                        else
                        {
                            photonView.RPC("RPC_SendKillfeed2", RpcTarget.AllBuffered, gameObject.name);
                        }
                    }
                    //todo envoie de bon killer 
                    //Debug.Log("kill");
                    //if (Cible != null)
                    //{

                    //    photonView.RPC("RPC_SendKillfeed", RpcTarget.AllBuffered, this.userId, Cible.GetComponent<IDamageable>().userId);
                    //}
                    //else
                    //{
                    //    photonView.RPC("RPC_SendKillfeed2", RpcTarget.AllBuffered, GetComponent<PhotonView>().ViewID);

                    //}
                }
                StartCoroutine(Death());
            }
            else
            {
                //Regen();
            }
            isDead = true;
        }
    }


    IEnumerator Death()
    {
        //Debug.Log(respawnCooldown);
        //gameObject.SetActive(false);
        SetDeath(true);
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.transform.position = deathPos;
        yield return new WaitForSeconds(respawnCooldown);
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
        SetHealth(GetMaxHealth());
        SetMana(GetMaxMana());
        _navMeshAgent.ResetPath();
        transform.position = respawnPos;
        SetDeath(false);
        gameObject.SetActive(true);

    }

    [PunRPC]
    public void RPC_SendKillfeed(string killerId, string victimId)
    {
        //Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerId);
        //Player victim = PhotonNetwork.CurrentRoom.GetPlayer(victimId);
        Debug.Log(killerId + " a kill " + victimId);
        // Mettre � jour votre UI pour afficher les informations dans le killfeed
    }

    [PunRPC]
    public void RPC_SendKillfeed2(string killerId)
    {
        //Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerId);
        //Player victim = PhotonNetwork.CurrentRoom.GetPlayer(victimId);
        Debug.Log(killerId + " is dead");
        // Mettre � jour votre UI pour afficher les informations dans le killfeed
    }
    IEnumerator Spawn(Renderer[] rend)
    {
        //if (transform.position != deathPos)
        //{
        //    transform.position = deathPos;
        isDead = false;
        GetComponent<CameraWork>().isFollowing = false;
        GetComponent<IDamageable>()._navMeshAgent.ResetPath();
        yield return new WaitForSeconds(respawnCooldown);
        transform.position = respawnPos;
        foreach (Transform child in transform)
        {
            Renderer rChild;
            if (child.TryGetComponent<Renderer>(out rChild))
            {
                rChild.enabled = false;
            }
        }
        GetComponent<CameraWork>().isFollowing = true;

        SetDefault(rend);
        //}
    }

    public void CallItemEquip()
    {
        photonView.RPC(nameof(ItemEquip), RpcTarget.All);
    }

    [PunRPC]
    public void ItemEquip()
    {
        foreach (var item in items)
        {
            SetMaxHealth(GetMaxHealth() + item.health);
            SetMaxMana(GetMaxMana() + item.mana);
            SetAttackSpeed(GetAttackSpeed() + item.attackSpeed);
            SetResMag(GetResMag() + item.resMag);
            SetResPhys(GetResPhys() + item.resPhys);
            SetDegPhys(GetDegPhys() + item.dmgPhys);
            SetDegMag(GetDegMag() + item.dmgMag);
            FindObjectOfType<ItemPassifs>().StartPassif(gameObject, item.idPassif);
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
        //SetMaxMana(500f);
        switch (role)
        {
            case Role.dps:
                SetMaxHealth(5000f);
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

