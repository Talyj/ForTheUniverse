using Photon.Pun;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class IDamageable : MonoBehaviourPun, IPunObservable
{
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

    private float DegatsPhysique;
    private float DegatsMagique;
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

    public Team team;
    public float damageSupp;
    public bool isAI;




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
        canMove = true;
        canAct = true;
        //useSkills = true;
        canUlt = false;
        InCombat = false;
        InRegen = false;

        Health = MaxHealth;
        Mana = MaxMana;

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
            photonView.RPC("GiveExperience", RpcTarget.All, new object[] { });
            if (gameObject.CompareTag("dd"))
            {
                //Victory
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject.GetComponent<PhotonView>());
            }
        }        
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
            SetMaxHealth(GetMaxHealth() * 1.06f);
            SetMaxMana(GetMaxMana() * 1.05f);
            SetAttackSpeed(GetAttackSpeed() * 1.12f);
            SetDegPhys(GetDegPhys() * 1.75f);
            SetDegMag(GetDegMag() * 1.75f);
            SetResMag(GetResMag() * 1.25f);
            SetResPhys(GetResPhys() * 1.25f);
            SetMoveSpeed(GetMoveSpeed() * 1.15f);
        }
    }

    [PunRPC]
    public void GiveExperience()
    {
        var expToGive = 0;
        //TODO Change the amount of xp
        switch (enemyType)
        {
            case EnemyType.minion:
                expToGive = 100;
                break;
            case EnemyType.player:
                expToGive = 1000 * gameObject.GetComponent<PlayerStats>().GetLvl();
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
                var collider = col.GetComponent<PlayerStats>();
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
                Health += val;
            }

            if (Mana < MaxMana)
            {
                float val = Mathf.FloorToInt(MaxMana * 0.1f);
                Mana += val;
            }
        }
    }

    public float TakeDamage(float DegatsRecu, DamageType type)
    {
        //Degat brut
        var degRes = DegatsRecu;
        switch (type)
        {
            case DamageType.physique:
                degRes = (DegatsRecu - ((ResistancePhysique * DegatsRecu) / 100)); // physique
                break;
            case DamageType.magique:
                degRes = (DegatsRecu - ((ResistanceMagique * DegatsRecu) / 100)); // magique
                break;
        }
        

        photonView.RPC("Damages", RpcTarget.All, new object[] { DegatsRecu });
        return degRes;
    }

    [PunRPC]
    public void Damages(float DegatsRecu)
    {
        Health = Health - DegatsRecu;
    }

    public void WalkToward()
    {
        try
        {
            var dist = Vector3.Distance(transform.position, Cible.transform.position);
            if (dist > gameObject.GetComponent<IDamageable>().GetAttackRange())
            {
                SetIsMoving(true);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z), GetMoveSpeed() * Time.deltaTime);
            }
            SetIsMoving(false);
        }
        catch (NullReferenceException e)
        {
            Cible = null;
        }
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

    public bool IsTargetable(EnemyType enemyToCompare)
    {
        if(enemyToCompare == EnemyType.minion ||
        enemyToCompare == EnemyType.voister ||
        enemyToCompare == EnemyType.player ||
        enemyToCompare == EnemyType.dieu ||
        enemyToCompare == EnemyType.golem)
        {
            return true;
        }
        return false;
    }

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
                    if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
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

            yield return new WaitForSeconds(GetAttackSpeed() / ((100 / +GetAttackSpeed()) * 0.01f));
        }

    }

    public IEnumerator RangeAutoAttack()
    {
        while (Cible != null)
        {
            //anim.SetBool("AA", true);
            try
            {
                if (Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() ||
                    Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 5 && isAI)
                {
                    if (IsTargetable(Cible.GetComponent<IDamageable>().GetEnemyType()))
                    {
                        SpawnRangeAttack(Cible, damageSupp);
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

    public void SpawnRangeAttack(GameObject Target, float dmgSupp = 0)
    {
        var bullets = PhotonNetwork.Instantiate(projPrefab.name, transform.position, Quaternion.identity);

        bullets.GetComponent<Projectile>().SetDamages(GetDegMag() + dmgSupp, DamageType.magique);
        bullets.GetComponent<Projectile>().target = Target;
        bullets.GetComponent<Projectile>().targetSet = true;
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
}
public enum Team
{
    Veritas = 0,
    Dominion = 1
}
