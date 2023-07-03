using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
//TODO change class name

public abstract class IDamageable : MonoBehaviourPun, IPunObservable
{
    public int characterID = -1;
    public enum AttackType { Melee, Ranged }
    public AttackType attackType;
    [SerializeField]
    ControlType cc;
    public GameObject Cible;
    //[SerializeField] public GameObject deathEffect;   
    public string userId;
    [Header("Stats")]
    [SerializeField] private float Health, MaxHealth;
    private float MoveSpeed;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AttackRange;
    private float ViewRange;
    private float Mana, MaxMana;
    private float ResistancePhysique; // calcul des resistance health = health - (DegatsPhysiqueRe�u -((ResistancePhysique * DegatsPhysiqueRe�u)/100)
    private float ResistanceMagique; //calcul des resistance health = health - (DegatsMagiqueRe�u - ((ResistanceMagique * DegatsMagiqueRe�u) / 100)

    [SerializeField] private float DegatsPhysique;
    [SerializeField] private float DegatsMagique;
    private bool canMove;
    private bool canAct;
    private bool isMoving;
    //private bool useSkills;
    [SerializeField] private bool canUlt;
    private bool InCombat;
    private bool IsDead;
    private bool InRegen;
    private float cptRegen = 0;
    public bool isAttacking;

    //exp
    [SerializeField] protected float Exp;
    [SerializeField] protected float MaxExp;
    [SerializeField] protected float ExpRate;//multiplicateur de l'exp max
    [SerializeField] protected int lvl;

    [Header("Ranged variables")]
    public GameObject projPrefab;
    public Transform SpawnPrefab;

    //public Team team;
    public PhotonTeam team;
    public float damageSupp;
    public bool isAI;

    //State
    public int inBush;

    //Nav Mesh
    [HideInInspector] public UnityEngine.AI.NavMeshAgent _navMeshAgent;

    public EnemyType enemyType;
    public enum EnemyType
    {
        minion,
        golem,
        player,
        dieu,
        voister
    }
    

    public enum DamageType
    {
        physique,
        magique,
        brut
    }

    public enum ControlType
    {
        none,//aucun cc
        stun,//etourdit
        bump,//en l'air
        root,//immobiliser mais pas stun
        slow,//move speed ralenti
        charme
    }
    //ControlType cc;
    [Header("Competences")]
    public Passifs passif;
    public Skills[] skills;

    //Damage
    public GameObject ult;


    public Transform SpawnPrefab2;

    #region Getter and Setter

    #region Getter

    //public bool GetUseSkills()
    //{
    //    return useSkills;
    //}
    public float GetExp()
    {
        return Exp;
    }

    public float GetMaxExp()
    {
        return MaxExp;
    }
    public int GetLvl()
    {
        return lvl;
    }
    public bool IsMoving()
    {
        return isMoving;
    }
    public float GetViewRange()
    {
        return ViewRange;
    }
    public bool GetCanMove()
    {
        return canMove;
    }

    public bool GetCanAct()
    {
        return canAct;
    }

    public bool GetCanUlt()
    {
        return canUlt;
    }

    public float GetHealth()
    {
        return Health;
    }
    public float GetMaxHealth()
    {
        return MaxHealth;
    }
    public float GetMana()
    {
        return Mana;
    }
    public float GetMaxMana()
    {
        return MaxMana;
    }
    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }
    public float GetAttackSpeed()
    {
        return AttackSpeed;
    }
    public float GetAttackRange()
    {
        return AttackRange;
    }
    public float GetResPhys()
    {
        return ResistancePhysique;
    }
    public float GetResMag()
    {
        return ResistanceMagique;
    }

    public float GetDegPhys()
    {
        return DegatsPhysique;
    }
    public float GetDegMag()
    {
        return DegatsMagique;
    }

    public bool GetDeath()
    {
        return IsDead;
    }

    public ControlType GetControl()
    {
        return cc;
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    #endregion
    #region Setter

    public void SetTeam(int value)
    {
        team.Code = (byte)value;
    }
    public void SetExp(float value)
    {
        Exp += value;
    }
    public void SetMaxExp(float value)
    {
        MaxExp = value;
    }
    public void SetLvl(int value)
    {
        lvl = value;
    }
    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }
    public void SetViewRange(float value)
    {
        ViewRange = value;
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    public void SetCanAct(bool value)
    {
        canAct = value;
    }
    public void SetCanUlt(bool value)
    {
        canUlt = value;
    }
    public void SetHealth(float value)
    {
        Health = value;
    }
    public void SetMaxHealth(float value)
    {
        MaxHealth = value;
    }
    public void SetMana(float value)
    {
        Mana = value;
    }

    public void SetMaxMana(float value)
    {
        MaxMana = value;
    }

    public void SetMoveSpeed(float value)
    {
        MoveSpeed = value;
    }
    public void SetAttackSpeed(float value)
    {
        AttackSpeed = value;
    }
    public void SetAttackRange(float value)
    {
        AttackRange = value;
    }
    public void SetResPhys(float value)
    {
        ResistancePhysique = value;
    }
    public void SetResMag(float value)
    {
        ResistanceMagique = value;
    }
    public void SetDegPhys(float value)
    {
        DegatsPhysique = value;
    }
    public void SetDegMag(float value)
    {
        DegatsMagique = value;
    }
    public void SetDeath(bool value)
    {
        IsDead = value;
    }

    public void SetEnemyType(EnemyType value)
    {
        enemyType = value;
    }
    #endregion

    #endregion

    public void BaseInit()
    {
        SetMaxHealth(5000);
        canMove = true;
        canAct = true;
        //useSkills = true;
        canUlt = false;
        InCombat = false;
        InRegen = false;

        Health = MaxHealth;
        Mana = MaxMana;

        inBush = 0;
        var layer = "Default";
        switch (this.team.Code)
        {
            case 0:
                layer = "Dominion";
                SetGameLayerRecursive(gameObject, layer);
                break;
            case 1:
                layer = "Veritas";
                SetGameLayerRecursive(gameObject, layer);
                break;
        }
        
        //team = PhotonTeamsManager.Instance.GetAvailableTeams()[1];

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
        }
    }

    private void SetGameLayerRecursive(GameObject gameObject, string layer)
    {
        if (!gameObject.CompareTag("minimapView"))
        {
            gameObject.layer = LayerMask.NameToLayer(layer);
            foreach (Transform child in gameObject.transform)
            {

                SetGameLayerRecursive(child.gameObject, layer);
            }
        }
    }
    public void SetupForAI()
    {
        SetMaxHealth(500);
        SetHealth(GetMaxHealth());
        SetMoveSpeed(4.5f);
        SetAttackSpeed(0.5f);
        SetAttackRange(1.5f);
        SetMaxMana(100);
        SetMana(GetMaxMana());
        SetResPhys(0);
        SetResMag(0);
        SetDegPhys(100);
        SetDegMag(100);
        SetupMinimap();
    }

    protected void SetupMinimap()
    {
        var MainGame = GameObject.FindObjectOfType<MainGame>();
        if(MainGame != null)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("minimapView"))
                {
                    child.GetComponent<Renderer>().material = MainGame.materialsMinimapView[team.Code];
                }
            }
        }
    }

    
    private void CheckCC()
    {
        switch (cc)
        {
            case ControlType.none:
                canMove = true;
                //useSkills = true;
                break;
            case ControlType.stun:
                canMove = false;
                //useSkills = false;
                break;
            case ControlType.bump:
                canMove = false;
                //useSkills = false;
                break;
            case ControlType.charme:
                canMove = false;
                //useSkills = false;
                break;
            case ControlType.root:
                canMove = false;
                break;
            case ControlType.slow:
                canMove = true;
                //useSkills = true;
                break;
        }
    }

    protected void HealthBehaviour()
    {

        if (Health >= MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Mana >= MaxMana)
        {
            Mana = MaxMana;
        }

        if (Health <= 0)
        {
            //photonView.RPC("GiveExperience", RpcTarget.All, new object[] { });
            if (gameObject.CompareTag("dd"))
            {
                //Victory
            }
            else if (gameObject.GetComponent<BasicAIStats>())
            {
                userId = gameObject.name;
                //PhotonView.Get(this).RPC("SendKillfeed", RpcTarget.AllBuffered,  Cible.name, userId);
                //PhotonView.Get(this).RPC("RPC_ReceiveKillfeed", RpcTarget.All,userId, Cible.name);
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                PhotonView.Get(this).RPC("RPC_SendKillfeed", RpcTarget.All, this.GetComponent<PhotonView>().ViewID, Cible.GetComponent<PhotonView>().ViewID);
            }
            //else if (PhotonNetwork.IsMasterClient)
            //{
            //    Debug.Log("dead");
            //    PhotonNetwork.Destroy(gameObject.GetComponent<PhotonView>());
            //}
            PhotonView.Get(this).RPC("RPC_ReceiveKillfeed", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId, Cible.name);
        }
    }

    [PunRPC]
    public void SendKillfeed(string killerName, string victimName, PhotonMessageInfo info)
    {
        // Envoyer les informations d'élimination aux autres clients
        // Vous pouvez également mettre à jour votre UI pour afficher les informations dans le killfeed
        Debug.Log(killerName + " a kill " + victimName);
        //Debug.LogFormat("Info: {0} sender {1} {2}", info.Sender, info.photonView, info.SentServerTime);

        photonView.RPC("GiveExperience", RpcTarget.All, new object[] { });
    }

    [PunRPC]
    public void RPC_SendKillfeed(int killerId, int victimId)
    {
        Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerId);
        Player victim = PhotonNetwork.CurrentRoom.GetPlayer(victimId);
        Debug.Log(killer + " a kill " + victim);
        // Mettre à jour votre UI pour afficher les informations dans le killfeed
    }

    public void ExperienceBehaviour()
    {
        if (Exp >= MaxExp)
        {
            float reste = Exp - MaxExp;
            lvl += 1;
            Exp = reste;
            MaxExp = MaxExp * ExpRate;
            print("lvl up");
            if (lvl == 6)
            {
                SetCanUlt(true);
            }
            // augmentation des stats a faire
            //test en dur a rendre plus automatique par scriptableobject surement
            SetHealth(GetHealth() * 1.06f);
            SetMaxHealth(GetMaxHealth() * 1.06f);
            SetMana(GetMana() * 1.05f);
            SetMaxMana(GetMaxMana() * 1.05f);
            SetAttackSpeed(GetAttackSpeed() * 0.97f);
            SetDegPhys(GetDegPhys() * 1.75f);
            SetDegMag(GetDegMag() * 1.75f);
            SetResMag(GetResMag() * 1.25f);
            SetResPhys(GetResPhys() * 1.25f);
            SetMoveSpeed(GetMoveSpeed() * 1.15f);
        }
    }

    [PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void GiveExperience()
    {
        float expToGive = 0;
        //TODO Change the amount of xp
        Debug.Log("test enemytype "+ Cible.GetComponent<IDamageable>().enemyType);
        switch (Cible.GetComponent<IDamageable>().enemyType)
        {
            case EnemyType.minion:
                expToGive = 1f;
                break;
            case EnemyType.player:
                //expToGive = 1000 * gameObject.GetComponent<PlayerStats>().GetLvl();
                expToGive = 10;
                break;
            case EnemyType.dieu:
                expToGive = 10000;
                break;
            case EnemyType.golem:
                expToGive = 500;
                break;
            case EnemyType.voister:
                expToGive = 200 * gameObject.GetComponent<VoisterBehaviour>().GetLvl();
                break;
        }

        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 30f);
        if (hitColliders != null)
        {
            foreach (var col in hitColliders)
            {
                var collider = col.GetComponent<IDamageable>();
                if (collider)
                {
                    if (collider.team != team && collider.enemyType == EnemyType.player ||
                        collider.team != team && collider.enemyType == EnemyType.voister)
                    {
                        collider.SetExp(expToGive);
                    }
                }
            }
        }
    }

    public IEnumerator CoolDown(Skills skill)
    {
        yield return new WaitForSeconds(skill.Cooldown);
        //Debug.Log("fin des cd");
        skill.isCooldown = false;
    }

    //function de regen mana et vie

    public void Regen()
    {
        cptRegen -= Time.deltaTime;
        if(cptRegen <= 0)
        {
            cptRegen = 5.0f;
            if (Health < MaxHealth)
            {
                float val = Mathf.FloorToInt(MaxHealth * 0.1f);
                Health += val/2;
            }

            if (Mana < MaxMana)
            {
                float val = Mathf.FloorToInt(MaxMana * 0.1f);
                Mana += val/2;
            }
        }
    }

    public float TakeDamage(float DegatsRecu, DamageType type, bool toMana = false)
    {
        if (toMana)
        {
            photonView.RPC("DealDamagesToMana", RpcTarget.All, DegatsRecu);
            return DegatsRecu;
        }
        //Degat brut
        float degRes = DegatsRecu;
        switch (type)
        {
            case DamageType.physique:
                degRes = (DegatsRecu - ((ResistancePhysique * DegatsRecu) / 100)); // physique
                break;
            case DamageType.magique:
                degRes = (DegatsRecu - ((ResistanceMagique * DegatsRecu) / 100)); // magique
                break;
        }
        
        photonView.RPC("DealDamages", RpcTarget.All,degRes);
        return degRes;
    }

    [PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void DealDamages(float DegatsRecu)
    {
        Health -= DegatsRecu;
    }

    [PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void DealDamagesToMana(float DegatsRecu)
    {
        Mana -= DegatsRecu;
    }

    public void TakeCC(ControlType _cc,float time)
    {
        cc = _cc;
        if(_cc == ControlType.slow)
        {
            MoveSpeed = MoveSpeed / 2;
        }
        StartCoroutine(TimeCC(time));
    }
    
    IEnumerator TimeCC(float time)
    {
        Debug.Log("cc");
        CheckCC();
        yield return new WaitForSeconds(time);
        
        cc = ControlType.none;
        CheckCC();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    public bool IsTargetable(PhotonTeam team)
    {
        if (this.team == team) return false;
        if(enemyType == EnemyType.minion ||
        enemyType == EnemyType.voister ||
        enemyType == EnemyType.player ||
        enemyType == EnemyType.dieu ||
        enemyType == EnemyType.golem)
        {
            return true;
        }
        return false;
    }

    //TODO bouger la fonction dans le code concerné
    public bool IsControl(EnemyType enemyToCompare, ControlType cc)
    {
        if (enemyToCompare == EnemyType.minion ||
        enemyToCompare == EnemyType.voister ||
        enemyToCompare == EnemyType.player ||
        enemyToCompare == EnemyType.dieu ||
        enemyToCompare == EnemyType.golem && cc != ControlType.none)
        {
            return true;
        }
        return false;
    }

    public GameObject viewIdToGameObject(int actorNumber)
    {
        var views = FindObjectsOfType<PhotonView>();
        GameObject res = null;

        foreach (var view in views)
        {
            if (view.ViewID == actorNumber)
            {
                res = view.gameObject;
            }
        }
        return res;
    }

    public void CheckRangeAttack()
    {
        try
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
                {
                    print("Hors d portée");
                }
                else
                {
                    if (attackType == AttackType.Melee)
                    {
                        StartCoroutine(AutoAttack());
                    }
                    if (attackType == AttackType.Ranged)
                    {
                        StartCoroutine(RangeAutoAttack());
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("No target available");
            Debug.Log(e);
        }
    }

    public IEnumerator AutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            try
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() ||
                    Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 5 && isAI)
                {
                    if (Cible.GetComponent<IDamageable>().IsTargetable(team))
                    {
                        Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique);
                    }
                }
                else
                {
                    //anim.SetBool("AA", false);
                }

                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
                {
                    //anim.SetBool("AA", false);
                }
            }
            catch (NullReferenceException e)
            {
                Cible = null;
            }

            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / GetAttackSpeed()) * 0.01f));
        }

    }

    public IEnumerator RangeAutoAttack()
    {
        while (Cible != null)
        {
            if(gameObject == null)
            {
                break;
            }
            //anim.SetBool("AA", true);
            try
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() ||
                    Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 5 && isAI)
                {
                    if (Cible.GetComponent<IDamageable>().IsTargetable(team))
                    {
                        SpawnRangeAttack(Cible, damageSupp);
                        //photonView.RPC("SpawnRangeAttack",RpcTarget.All, new object[] { Cible, damageSupp } );
                    }
                }
                else
                {
                    //anim.SetBool("AA", false);
                }
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
                {
                    //anim.SetBool("AA", false);
                }
            }
            catch (NullReferenceException e)
            {
                Cible = null;
            }
            var test = GetAttackSpeed() / ((100 / GetAttackSpeed()) * 0.01f);
            yield return new WaitForSeconds(test);
        }

    }

    //[PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void SpawnRangeAttack(GameObject Target, float dmgSupp = 0)
    {
        var bullets = PhotonNetwork.Instantiate(projPrefab.name, transform.position, Quaternion.identity);
        bullets.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
        //Debug.Log(bullets.GetComponent<Projectile>().playerId);
        //Debug.Log(userId);
        bullets.GetComponent<Projectile>().SetDamages(GetDegPhys() + dmgSupp, DamageType.physique);
        bullets.GetComponent<Projectile>().target = Target;
        bullets.GetComponent<Projectile>().targetSet = true;
        bullets.GetComponent<Projectile>().playerId = photonView.name;
        //photonView.RPC("OnProjectileCreated", RpcTarget.OthersBuffered, bullets.GetPhotonView().ViewID, PhotonNetwork.LocalPlayer.ActorNumber);
    }


    [PunRPC]
    void OnProjectileCreated(int projectileViewID, int ownerActorNumber)
    {
        // Récupération du projectile créé sur les autres clients
        PhotonView projectileView = PhotonView.Find(projectileViewID);
        if (projectileView != null)
        {
            // Récupération du propriétaire du projectile
            Player owner = PhotonNetwork.CurrentRoom.GetPlayer(ownerActorNumber);

            // Traitement du projectile créé sur les autres clients en utilisant le propriétaire
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GetHealth());
        }
        else
        {
            SetHealth((float)stream.ReceiveNext());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bush"))
        {
            Debug.Log("InBush");
            inBush++;
            
            if (inBush == 1)
            {
                Debug.Log($"Team code : {team.Code} - Team Name : {team.Name}");
                var layer = team.Code == 0 ? "InvisibleDominion" : "InvisibleVeritas";
                foreach (Transform child in gameObject.transform)
                {

                    if (!child.gameObject.CompareTag("minimapView"))
                    {
                        child.gameObject.layer = LayerMask.NameToLayer(layer);
                        child.SetLayerRecursively(LayerMask.NameToLayer(layer));

                    }
                }
                
                //BushManager.Instance().AddEntityToBush(other.gameObject.GetComponent<BushBehavior>().bushID, gameObject);
            }
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bush"))
        {
            Debug.Log("ExitBush");
            inBush--;
            
            if (inBush <= 0)
            {
                switch (this.team.Code)
                {
                    case 0:
                        foreach (Transform child in gameObject.transform)
                        {
                            if (!child.gameObject.CompareTag("minimapView"))
                            {
                                child.gameObject.layer = LayerMask.NameToLayer("Dominion");
                                child.SetLayerRecursively(LayerMask.NameToLayer("Dominion"));

                            }
                        }
                        break;
                    case 1:
                        foreach (Transform child in gameObject.transform)
                        {
                            if (!child.gameObject.CompareTag("minimapView"))
                            {
                                child.gameObject.layer = LayerMask.NameToLayer("Veritas");
                                child.SetLayerRecursively(LayerMask.NameToLayer("Veritas"));

                            }
                        }
                        break;
                }
                //gameObject.layer = LayerMask.NameToLayer("Player");

                //transform.SetLayerRecursively(LayerMask.NameToLayer("Player"));
                //BushManager.Instance().RemoveEntityToBush(other.gameObject.GetComponent<BushBehavior>().bushID, gameObject);
            }
        }

        
    }
    
}
//public enum Team
//{
//    Veritas = 0,
//    Dominion = 1,
//    Voister = 2,
//    nothing = 3
//}


public static class Utils
{
    public static void SetLayerRecursively(this Transform parent, int layer)
    {
        parent.gameObject.layer = layer;
 
        for (int i = 0, count = parent.childCount; i < count; i++)
        {
            parent.GetChild(i).SetLayerRecursively(layer);
        }
    }
}