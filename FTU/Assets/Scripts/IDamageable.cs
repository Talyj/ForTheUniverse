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
    [Header("death")]
    [SerializeField]
    Behaviour[] disableOnDeath;
    //[SerializeField]
    //public GameObject deathEffect;
    //[SerializeField]
    Transform templeSpawn;
    //TODO when dev is over ad get set and change to private
    public string userId;
    [Header("Stats")]
    private float Health, MaxHealth;
    private float MoveSpeed;
    private float AttackSpeed;
    private float AttackRange;
    private float Mana, MaxMana;
    private float ResistancePhysique; // calcul des resistance health = health - (DegatsPhysiqueRe�u -((ResistancePhysique * DegatsPhysiqueRe�u)/100)
    private float ResistanceMagique; //calcul des resistance health = health - (DegatsMagiqueRe�u - ((ResistanceMagique * DegatsMagiqueRe�u) / 100)
    private float Exp;
    private float MaxExp;
    private float ExpRate;//multiplicateur de l'exp max
    private float DegatsPhysique;
    private float DegatsMagique;
    private int lvl;
    private bool canMove;
    private bool canAct;
    private bool useSkills;
    private bool canUlt;
    private bool InCombat;
    private bool InRegen;

    [Header("Ranged variables")]
    public GameObject projPrefab;
    public Transform SpawnPrefab;

    public EnemyType enemyType;
    public enum EnemyType
    {
        minion,
        golem,
        joueur,
        dieu,
        voister
    }

    public Team team;
    public enum Team
    {
        Veritas,
        Dominion
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


    public Transform SpawnPrefab2;

    #region Getter and Setter

    #region Getter

    public bool GetUseSkills()
    {
        return useSkills;
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
    public float GetExp()
    {
        return Exp;
    }

    public float GetMaxExp()
    {
        return MaxExp;
    }

    public float GetDegPhys()
    {
        return DegatsPhysique;
    }
    public float GetDegMag()
    {
        return DegatsMagique;
    }
    public int GetLvl()
    {
        return lvl;
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
    public void SetExp(float value)
    {
        Exp = value;
    }
    public void SetDegPhys(float value)
    {
        DegatsPhysique = value;
    }
    public void SetDegMag(float value)
    {
        DegatsMagique = value;
    }
    public void SetLvl(int value)
    {
        lvl = value;
    }
    #endregion

    #endregion


    public void Init()
    {
        canMove = true;
        canAct = true;
        useSkills = true;
        canUlt = false;
        InCombat = false;
        InRegen = false;

        Health = MaxHealth;
        //deathEffect.SetActive(false);
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = true;
        }
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

        MaxHealth = 500;
        Health = MaxHealth;
        MoveSpeed = 4.5f;
        AttackSpeed = 0.5f;
        AttackRange = 1.5f;
        Mana = 100;
        MaxMana = 100;
        ResistancePhysique = 0;
        ResistanceMagique = 0;
        Exp = 0;
        MaxExp = 100;
        ExpRate = 1.75f;
        DegatsPhysique = 100;
        DegatsMagique = 100;
        lvl = 1;
    }

    //public void Setup()
    //{
        
    //    wasEnableOnStart = new bool[disableOnDeath.Length];
    //    for(int i = 0; i < disableOnDeath.Length; i++)
    //    {
    //        wasEnableOnStart[i] = disableOnDeath[i].enabled;
    //    }
    //    SetDefault();
    //}
    public void SetDefault()
    {
        Health = MaxHealth;
        //deathEffect.SetActive(false);
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = true;
        }
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

    
    private void CheckCC()
    {
        switch (cc)
        {
            case ControlType.none:
                canMove = true;
                useSkills = true;
                break;
            case ControlType.stun:
                canMove = false;
                useSkills = false;
                break;
            case ControlType.bump:
                canMove = false;
                useSkills = false;
                break;
            case ControlType.charme:
                canMove = false;
                useSkills = false;
                break;
            case ControlType.root:
                canMove = false;
                break;
            case ControlType.slow:
                canMove = true;
                useSkills = true;
                break;

        }
    }
    //Has to be present in the final update
    public void HealthBehaviour()
    {
        if (Health >= MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Mana >= MaxMana)
        {
            Mana = MaxMana;
        }

        if(Health <= 0)
        {
            Destroy(gameObject);
        }
        //TODO DEATH
    }

    public void CheckTarget()
    {   
        if(Cible == null)
        {
            Cible = null;
        }
    }

    //Has to be present in the final update
    public void ExperienceBehaviour()
    {
        if (Exp >= MaxExp)
        {
            float reste = Exp - MaxExp;
            lvl += 1;
            Exp = 0 + reste;
            MaxExp = MaxExp * ExpRate;
            print("lvl up");
            if (lvl == 6)
            {
                canUlt = true;
            }
            // augmentation des stats a faire
            //test en dur a rendre plus automatique par scriptableobject surement
            MaxHealth += 106;
            MaxMana += 65;
            AttackSpeed += .12f;
            DegatsPhysique += 3.75f;
            DegatsMagique += 2.75f;
            ResistanceMagique += 2.25f;
            ResistancePhysique += 2.25f;
            MoveSpeed += 0.55f;
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
        StartCoroutine(RegenHealAndMana());
    }

    IEnumerator RegenHealAndMana()
    {

        if (Health < MaxHealth)
        {
            float val = Mathf.FloorToInt(MaxHealth * 0.05f);
            Health += val;
            Debug.Log("+ " + val);
        }



        yield return new WaitForSeconds(1.5f);

    }




    public void TakeDamage(float DegatsRecu, DamageType type)
    {
        //application des res, a modifier pour les differents type de degats
        if (type == DamageType.physique)
        {
            Health = Health - (DegatsRecu - ((ResistancePhysique * DegatsRecu) / 100)); // physique
        }
        else if (type == DamageType.magique)
        {
            Health = Health - (DegatsRecu - ((ResistanceMagique * DegatsRecu) / 100)); // magique
        }
        else if (type == DamageType.brut)
        {
            Health -= DegatsRecu;
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
    public void IsDead()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        //deathEffect.SetActive(true);
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
             disableOnDeath[i].enabled = false;
        }
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.useGravity = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        Debug.Log(transform.name + " est mort");
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        
        yield return new WaitForSeconds(5f);
        
        SetDefault();
        Transform spawnPoint = templeSpawn;
        transform.position = spawnPoint.position;
        //transform.rotation = spawnPoint.rotation;
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
        enemyToCompare == EnemyType.joueur ||
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
        enemyToCompare == EnemyType.joueur ||
        enemyToCompare == EnemyType.dieu ||
        enemyToCompare == EnemyType.golem && cc != ControlType.none)
        {
            return true;
        }
        return false;
    }

    public void GetNearestTarget()
    {
        if (Cible == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, GetAttackRange());
            if (hitColliders != null)
            {
                foreach (var col in hitColliders)
                {
                    if (col.gameObject.CompareTag("Player") ||
                        col.gameObject.CompareTag("minion") ||
                        col.gameObject.CompareTag("golem") ||
                        col.gameObject.CompareTag("dd"))

                    {
                        if (col.gameObject.GetComponent<IDamageable>().team != team)
                        {
                            Cible = col.gameObject;
                            break;
                        }
                    }

                }
            }
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
}

