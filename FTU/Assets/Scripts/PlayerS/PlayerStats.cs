using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float kill;
    public float death;
    public float assist;
    public float creep;

    public Dictionary<ItemStats, int> items = new Dictionary<ItemStats, int>();
    public Role role;

    private ItemPassifs itemPassifs;

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
        itemPassifs = FindObjectOfType<ItemPassifs>();
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
        if (GetHealth() < GetMaxHealth())
        {
            healthDecreaseTimer += Time.deltaTime;
        }
        inFight = (healthDecreaseTimer >= 6f || healthDecreaseTimer <= -1f) ? false : true;

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
                else if (gameObject.GetComponent<PlayerStats>() && GetDeath() == false)
                {
                    if (!isDead)
                    {
                        if (idLastDamageTaken > 0)
                        {
                            photonView.RPC("RPC_SendKillfeed", RpcTarget.All, idLastDamageTaken, photonView.ViewID);
                            idLastDamageTaken = -1;
                        }
                        else
                        {
                            photonView.RPC("RPC_SendKillfeed2", RpcTarget.All, photonView.ViewID);
                        }
                    }
                    Debug.Log("kill");
                    SetDeath(true);
                    gameObject.GetComponent<PlayerStats>().death += 1;
                    StartCoroutine(Death());
                }
            }
            else
            {
                //Regen();
            }
        }
    }


    IEnumerator Death()
    {
        isDead = true;
        //Debug.Log(respawnCooldown);
        //gameObject.SetActive(false);
        //death += 1;
        healthDecreaseTimer = 0f;
        _navMeshAgent.ResetPath();
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.layer != LayerMask.NameToLayer("UI"))
            {
                child.gameObject.SetActive(false);
            }

        }
        gameObject.transform.position = respawnPos;
        yield return new WaitForSeconds(respawnCooldown);
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
        SetHealth(GetMaxHealth());
        SetMana(GetMaxMana());
        //Debug.Log(respawnPos);
        gameObject.transform.position = respawnPos;
        SetDeath(false);
        healthDecreaseTimer = 0f;
        gameObject.SetActive(true);
        isDead = false;

    }

    [PunRPC]
    public void RPC_SendKillfeed(int killerId, int victimId)
    {

        var killer = PhotonView.Find(killerId);
        var victim = PhotonView.Find(victimId);
        //PhotonView.Find(idLastDamageTaken).gameObject.name, gameObject.name
        //Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerId);
        //Player victim = PhotonNetwork.CurrentRoom.GetPlayer(victimId);
        Debug.Log(killer.gameObject.name + " a kill " + victim.gameObject.name);
        // Mettre � jour votre UI pour afficher les informations dans le killfeed
        GetComponentInChildren<UI>().DisplayFeed(killer, victim);
    }

    [PunRPC]
    public void RPC_SendKillfeed2(int victimId)
    {
        var victim = PhotonView.Find(victimId);
        //Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerId);
        //Player victim = PhotonNetwork.CurrentRoom.GetPlayer(victimId);
        Debug.Log(victim.gameObject.name + " is dead");
        // Mettre � jour votre UI pour afficher les informations dans le killfeed
        GetComponentInChildren<UI>().DisplayFeed(null, victim);
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


    //Items
    #region items
    protected void UseObject()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            LaunchObjectEffect(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LaunchObjectEffect(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LaunchObjectEffect(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LaunchObjectEffect(3);
        }
    }

    private void LaunchObjectEffect(int posObject)
    {
        if (posObject < items.Count())
        {
            itemPassifs.StartPassif(gameObject, items.ElementAt(posObject).Key.idPassif, items.ElementAt(posObject).Key.rarete == ItemRarete.Consommable ? 1 : 0);
            if (items.ElementAt(posObject).Key.rarete == ItemRarete.Consommable)
            {
                items[items.ElementAt(posObject).Key]--;
                if (items[items.ElementAt(posObject).Key] <= 0)
                {
                    items.Remove(items.ElementAt(posObject).Key);
                }
            }
        }
        else
        {
            Debug.LogError("No item in this slot");
        }
    }

    public void CallItemEquip(int ID, bool isUnequip = false)
    {
        if (isUnequip)
        {
            photonView.RPC(nameof(ItemUnequip), RpcTarget.All, ID);
        }
        else
        {
            photonView.RPC(nameof(ItemEquip), RpcTarget.All, ID);
        }
    }

    [PunRPC]
    public void ItemEquip(int ID)
    {
        var item = GetItemFromID(ID);

        if (item == null) return;
        SetMaxHealth(GetMaxHealth() + item.health);
        SetMaxMana(GetMaxMana() + item.mana);
        SetAttackSpeed(GetAttackSpeed() + item.attackSpeed);
        SetResMag(GetResMag() + item.resMag);
        SetResPhys(GetResPhys() + item.resPhys);
        SetDegPhys(GetDegPhys() + item.dmgPhys);
        SetDegMag(GetDegMag() + item.dmgMag);
        if (item.rarete != ItemRarete.Consommable)
        {
            FindObjectOfType<ItemPassifs>().StartPassif(gameObject, item.idPassif, item.rarete == ItemRarete.Consommable ? 0 : 1);
        }
    }

    [PunRPC]
    public void ItemUnequip(int ID)
    {
        var item = GetItemFromID(ID);

        if (item == null) return;
        SetMaxHealth(GetMaxHealth() - item.health);
        SetMaxMana(GetMaxMana() - item.mana);
        SetAttackSpeed(GetAttackSpeed() - item.attackSpeed);
        SetResMag(GetResMag() - item.resMag);
        SetResPhys(GetResPhys() - item.resPhys);
        SetDegPhys(GetDegPhys() - item.dmgPhys);
        SetDegMag(GetDegMag() - item.dmgMag);
    }

    private ItemStats GetItemFromID(int id)
    {
        foreach (var item in itemPassifs.listItems)
        {
            if (id == item.ID)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

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

