using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public int idLastDamageTaken = -1;
    [Space]
    [Header("Stats")]
    [Space]
    [SerializeField] private float Health, MaxHealth;
    private float MoveSpeed;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AttackRange;
    private float attackTimer=0f;
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
    public bool inFight;
    [SerializeField] private bool IsDead;
    private bool InRegen;
    private float cptRegen = 0;
    public bool isAttacking;

    //Animation mort
    [SerializeField] protected Material dissolveMaterial; 
    [SerializeField] protected RuntimeAnimatorController dissolveController; 
    protected float dissolveDuration = 1.0f; 

    public float healthDecreaseTimer = -1f;
    //exp
    [SerializeField] protected float Exp;
    [SerializeField] protected float MaxExp;
    [SerializeField] protected float ExpRate;//multiplicateur de l'exp max
    [SerializeField] protected int lvl;
    [SerializeField] protected bool lvlup=false;
    float healthAmount ;
    float regenTimer = 0f;
    [Space]
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
    public void HealthRegen(float value)
    {
        Health += value;
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
        healthAmount =25f;
        SetMaxHealth(5000);
        SetMaxMana(50000);
        SetMaxMana(GetMaxMana());
        canMove = true;
        canAct = true;
        //useSkills = true;
        canUlt = false;
        inFight = false;
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
        if (col != null)
        {
            col.enabled = true;
        }

        if (!photonView.IsMine)
        {
            GetComponentInChildren<UI>().gameObject.SetActive(false);
        }
    }

    private void SetGameLayerRecursive(GameObject gameObject, string layer)
    {
        if (!gameObject.CompareTag("minimapView") && gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            /*if (enemyType == EnemyType.player)
            {
                Debug.Log($"{gameObject.name} - {LayerMask.LayerToName(gameObject.layer)}");
            }*/
            
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

    public virtual void HealthBehaviour()
    {
        //Debug.Log("toto");
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
            foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
                meshRenderer.material = dissolveMaterial;

                var animator = meshRenderer.gameObject.GetComponent<Animator>();
                if (animator == null) {
                    animator = meshRenderer.gameObject.AddComponent<Animator>();
                }

                animator.runtimeAnimatorController = dissolveController;
            }
            //photonView.RPC("GiveExperience", RpcTarget.All, new object[] { });
            if (gameObject.CompareTag("dd"))
            {

                var mainGame = FindObjectOfType<MainGame>();
                if (mainGame)
                {
                    mainGame.SendVictoryMessage(team.Code);

                }
            } 
            else if (gameObject.GetComponent<BasicAIStats>())
            {
                StartCoroutine(DissolveEffect());
            }
            //RPC_SendKillfeed(this.GetComponent<PhotonView>().ViewID, Cible.GetComponent<PhotonView>().ViewID);
            //else if (PhotonNetwork.IsMasterClient)
            //{
            //    Debug.Log("dead");
            //    PhotonNetwork.Destroy(gameObject.GetComponent<PhotonView>());
            //}
            //PhotonView.Get(this).RPC("RPC_ReceiveKillfeed", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId, Cible.name);
        }
    }

    protected IEnumerator DissolveEffect() {
        yield return new WaitForSeconds(dissolveDuration);

       //Destroy(gameObject);
        PhotonNetwork.Destroy(gameObject);
    }

    protected IEnumerator DissolveEffect(GameObject objectToDestroy)
    {
        PhotonNetwork.Destroy(objectToDestroy);
        yield return 0;
    }

    [PunRPC]
    public void SendKillfeed(string killerName, string victimName, PhotonMessageInfo info)
    {
        // Envoyer les informations d'élimination aux autres clients
        // Vous pouvez également mettre à jour votre UI pour afficher les informations dans le killfeed
        Debug.Log(killerName + " a kill " + victimName);
        //Debug.LogFormat("Info: {0} sender {1} {2}", info.Sender, info.photonView, info.SentServerTime);

        //photonView.RPC("GiveExperience", RpcTarget.All, new object[] { });
    }



    public void ExperienceBehaviour()
    {
        lvlup = (Exp >= MaxExp) ? true : false;
        if (lvlup)
        {
            StartCoroutine(Levelling());
        }
    }


    IEnumerator Levelling()
    {
        lvlup = false;
        float reste = Exp - MaxExp;
        lvl += 1;
        Exp = reste;
        MaxExp = MaxExp * ExpRate;
        print("lvl up");
        if (lvl == 6)
        {
            SetCanUlt(true);
        }


        // augmentation des stats %
        float percentHp = GetMaxHealth() * 0.08f;
        float percentPm = GetMaxMana() * 0.08f;
        float percentAs = GetAttackSpeed() * 0.97f;
        float percentAd = GetDegPhys() * 0.045f;
        float percentAp = GetDegMag() * 0.045f;
        float percentAr = GetResMag() * 0.06f;
        float percentRm = GetResPhys() * 0.06f;
        float percentMs = GetMoveSpeed() * 0.01f;


        //test en dur a rendre plus automatique par scriptableobject surement
        SetMaxHealth(GetMaxHealth() +percentHp);
        SetMaxMana(GetMaxMana()+percentPm);
        SetAttackSpeed(GetAttackSpeed()+percentAs);
        SetDegPhys(GetDegPhys()+percentAd);
        SetDegMag(GetDegMag()+percentAp);
        SetResMag(GetResMag()+percentAr);
        SetResPhys(GetResPhys()+percentRm);
        SetMoveSpeed(GetMoveSpeed()+percentMs);

        if(GetHealth() == GetMaxHealth())
        {
            SetHealth(GetMaxHealth());
        }
        yield return null;
    }

    [PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void GiveExperience()
    {
        float expToGive = 0;
        //TODO Change the amount of xp
        Debug.Log("test enemytype " + Cible.GetComponent<IDamageable>().enemyType);
        switch (Cible.GetComponent<IDamageable>().enemyType)
        {
            case EnemyType.minion:
                expToGive = 12f * gameObject.GetComponent<PlayerStats>().GetLvl(); 
                break;
            case EnemyType.player:
                //expToGive = 1000 * gameObject.GetComponent<PlayerStats>().GetLvl();
                expToGive = 75 * gameObject.GetComponent<PlayerStats>().GetLvl(); 
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
            Debug.Log("give exp 1");
            foreach (var col in hitColliders)
            {
            Debug.Log("give exp 2");
                var collider = col.GetComponent<IDamageable>();
        Debug.Log("my enemytype " + collider.GetComponent<IDamageable>().enemyType);
                if (collider)
                {
            Debug.Log("give exp 3");
                    if (collider.team.Code != team.Code && collider.enemyType == EnemyType.player )
                    {
            Debug.Log("give exp 4");
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
        //cptRegen -= Time.deltaTime;
        //if(cptRegen <= 0)
        //cptRegen -= Time.deltaTime;
        //if (cptRegen <= 0)
        //{
        //    cptRegen = 5.0f;
        //    if (Health < MaxHealth)
        //    {
        //        float val = Mathf.FloorToInt(MaxHealth * 0.01f);
        //        Health += val/2;
        //    }

        //    if (Mana < MaxMana)
        //    {
        //        float val = Mathf.FloorToInt(MaxMana * 0.01f);
        //        Mana += val/2;
        //    }
        //}
        
        
        if (inFight == false)
        {
            if(regenTimer < 5f)
            {
                regenTimer += Time.deltaTime;
            }
            else
            {
                //Debug.Log( healthAmount);
                HealthRegen(healthAmount);
                regenTimer = 0f;
            }
        }
    }

    public float TakeDamage(float DegatsRecu, DamageType type, int de, bool toMana = false)
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

        photonView.RPC("DealDamages", RpcTarget.All, degRes, de);
        healthDecreaseTimer = 0f;
        return degRes;
    }

    [PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void DealDamages(float DegatsRecu, int de)
    {
        Health -= DegatsRecu;

        idLastDamageTaken = de;

        //list des assits
        List<PlayerStats> assitPlayers = new List<PlayerStats>(5);
        if(PhotonView.Find(de).GetComponent<PlayerStats>() != null)
        {
            if (!assitPlayers.Contains(PhotonView.Find(de).GetComponent<PlayerStats>()))
            {
                assitPlayers.Add(PhotonView.Find(de).GetComponent<PlayerStats>());
            }
        }
        


        string by = PhotonView.Find(de).gameObject.name;
        //Debug.Log(this.gameObject.name + " a recu " + DegatsRecu + " de " + by);
        if (Health <= 0)
        {
            if (gameObject.GetComponent<PlayerStats>())
            {
                if (PhotonView.Find(de).GetComponent<PlayerStats>())
                {
                    Debug.Log(" 1");
                    PhotonView.Find(de).GetComponent<PlayerStats>().kill += 1;
                    PhotonView.Find(de).GetComponent<IDamageable>().SetExp(75);
                    PhotonView.Find(de).GetComponent<PlayerStats>().SetGold(300);
                    PhotonView.Find(de).GetComponent<PlayerStats>().Cible = null;
                    foreach (PlayerStats item in assitPlayers)
                    {
                        if (!item.Equals(PhotonView.Find(de).GetComponent<PlayerStats>()))
                        {
                            item.SetGold(150);
                            item.SetExp(37.5f);
                            item.assist += 1;
                        }

                    }
                }
            }
            else if (gameObject.GetComponent<BasicAIStats>().GetEnemyType()== EnemyType.minion)
            {
                //Player killer = PhotonNetwork.CurrentRoom.GetPlayer(de);
                //PhotonView.Find(de).GetComponent<IDamageable>().photonView.RPC("GiveExperience",RpcTarget.All);
                if (PhotonView.Find(de).GetComponent<PlayerStats>())
                {
                Debug.Log(" 2");
                    PhotonView.Find(de).GetComponent<IDamageable>().SetExp(25);
                    PhotonView.Find(de).GetComponent<PlayerStats>().SetGold(50);
                    PhotonView.Find(de).GetComponent<PlayerStats>().creep += 1;
                }
            }
            else if (gameObject.GetComponent<BasicAIStats>().GetEnemyType() == EnemyType.golem)
            {
                if (PhotonView.Find(de).GetComponent<PlayerStats>())
                {
                Debug.Log(" 3");
                    PhotonView.Find(de).GetComponent<IDamageable>().SetExp(125);
                    //PhotonView.Find(de).GetComponent<IDamageable>().photonView.RPC("GiveExperience", RpcTarget.All);
                    PhotonView.Find(de).GetComponent<PlayerStats>().SetGold(500);

                }
            }
            else if ( gameObject.GetComponent<BasicAIStats>().GetEnemyType() == EnemyType.voister)
            {
                if (PhotonView.Find(de).GetComponent<PlayerStats>())
                {
                Debug.Log(" 4");
                    PhotonView.Find(de).GetComponent<IDamageable>().SetExp(75);
                    PhotonView.Find(de).GetComponent<PlayerStats>().SetGold(200);
                    PhotonView.Find(de).GetComponent<PlayerStats>().creep += 1;
                }
            }
                
        }
    }

    [PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void DealDamagesToMana(float DegatsRecu)
    {
        Mana -= DegatsRecu;
    }

    public void TakeCC(ControlType _cc, float time)
    {
        cc = _cc;
        if (_cc == ControlType.slow)
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
        if (enemyType == EnemyType.minion ||
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
            if (Input.GetMouseButtonDown(0) && Time.deltaTime < GetAttackSpeed() / ((100 / +GetAttackSpeed()) * 0.01f) && !isAttacking)
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) > GetAttackRange())
                {
                    print("Hors d portée");
                }
                else
                {
                    isAttacking = true;
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
                        Cible.GetComponent<IDamageable>().TakeDamage(GetDegPhys() + damageSupp, DamageType.physique, this.photonView.ViewID);
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
            isAttacking = false;
        }

    }

    public IEnumerator RangeAutoAttack()
    {
        while (Cible != null)
        {
            if (gameObject == null)
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

            isAttacking = false;
        }

    }

    //[PunRPC] // NE PEUT TRANSMETTRE QUE DES TYPE CLASSIQUE (int, float, bool)
    public void SpawnRangeAttack(GameObject Target, float dmgSupp = 0)
    {
        var bullets = PhotonNetwork.Instantiate(projPrefab.name, transform.position, Quaternion.identity);
        //bullets.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
        //Debug.Log(bullets.GetComponent<Projectile>().playerId);
        //Debug.Log(userId);

        bullets.GetComponent<Projectile>().SetDamages(GetDegPhys() + dmgSupp, DamageType.physique);
        bullets.GetComponent<Projectile>().target = Target;
        bullets.GetComponent<Projectile>().targetSet = true;
        bullets.GetComponent<Projectile>().playerId = photonView.name;
        bullets.GetComponent<Projectile>().SetCreator(photonView);
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
            stream.SendNext(GetMaxHealth());
            stream.SendNext(GetMana());
            stream.SendNext(GetMaxMana());
            stream.SendNext(GetAttackSpeed());
            stream.SendNext(GetAttackRange());
            stream.SendNext(GetResPhys());
            stream.SendNext(GetResMag());
            stream.SendNext(GetDegPhys());
            stream.SendNext(GetDegMag());
            stream.SendNext(lvl);
        }
        else
        {
            SetHealth((float)stream.ReceiveNext());
            SetMaxHealth((float)stream.ReceiveNext());
            SetMana((float)stream.ReceiveNext());
            SetMaxMana((float)stream.ReceiveNext());
            SetAttackSpeed((float)stream.ReceiveNext());
            SetAttackRange((float)stream.ReceiveNext());
            SetResPhys((float)stream.ReceiveNext());
            SetResMag((float)stream.ReceiveNext());
            SetDegPhys((float)stream.ReceiveNext());
            SetDegMag((float)stream.ReceiveNext());
            SetLvl((int)stream.ReceiveNext());
        }
    }

    public void EnterBush(BushBehavior bush)
    {
        
        Debug.Log("InBush");
        inBush++;
        
        if (inBush == 1)
        {
            Debug.Log($"Team code : {team.Code} - Team Name : {team.Name}");
            var layer = team.Code == 0 ? "InvisibleDominion" : "InvisibleVeritas";
            gameObject.layer = LayerMask.NameToLayer(layer);
            transform.SetLayerRecursively(LayerMask.NameToLayer(layer));
            BushManager.Instance().AddEntityToBush(bush.bushID, gameObject);
            foreach (Transform child in gameObject.transform)
            {

                if (!child.gameObject.CompareTag("minimapView") && child.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    child.gameObject.layer = LayerMask.NameToLayer(layer);
                    child.SetLayerRecursively(LayerMask.NameToLayer(layer));

                }

                //BushManager.Instance().AddEntityToBush(other.gameObject.GetComponent<BushBehavior>().bushID, gameObject);
            }
        }

    }

    public void ExitBush(BushBehavior bush)
    {
        inBush--;
        
        if (inBush <= 0)
        {
            switch (this.team.Code)
            {
                case 0:
                    foreach (Transform child in gameObject.transform)
                    {
                        if (!child.gameObject.CompareTag("minimapView") && child.gameObject.layer != LayerMask.NameToLayer("UI"))
                        {
                            child.gameObject.layer = LayerMask.NameToLayer("Dominion");
                            child.SetLayerRecursively(LayerMask.NameToLayer("Dominion"));

                        }
                    }
                    break;
                case 1:
                    foreach (Transform child in gameObject.transform)
                    {
                        if (!child.gameObject.CompareTag("minimapView") && child.gameObject.layer != LayerMask.NameToLayer("UI"))
                        {
                            child.gameObject.layer = LayerMask.NameToLayer("Veritas");
                            child.SetLayerRecursively(LayerMask.NameToLayer("Veritas"));

                        }
                    }
                    break;
            }
            //gameObject.layer = LayerMask.NameToLayer("Player");

            //transform.SetLayerRecursively(LayerMask.NameToLayer("Player"));
            BushManager.Instance().RemoveEntityToBush(bush.bushID, gameObject);
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
        if (parent.gameObject.CompareTag("minimapView") || parent.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            return;
        }
        parent.gameObject.layer = layer;

        for (int i = 0, count = parent.childCount; i < count; i++)
        {
            parent.GetChild(i).SetLayerRecursively(layer);
        }
    }
}