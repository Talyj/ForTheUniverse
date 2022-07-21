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
    public float gold;
    //[SerializeField] public GameObject deathEffect;   
    [HideInInspector] public Vector3 respawnPos;
    [HideInInspector] public Vector3 deathPos;
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
    [SerializeField] private float Exp;
    [SerializeField]private float MaxExp;
    [SerializeField] private float ExpRate;//multiplicateur de l'exp max
    private float DegatsPhysique;
    private float DegatsMagique;
    private int lvl;
    private bool canMove;
    private bool canAct;
    private bool isMoving;
    //private bool useSkills;
    [SerializeField] private bool canUlt;
    private bool InCombat;
    private bool IsDead;
    private bool InRegen;
    private float respawnCooldown;

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

    //public bool GetUseSkills()
    //{
    //    return useSkills;
    //}
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
    public void SetExp(float value)
    {
        Exp += value;
    }
    public void SetMaxExp(float value)
    {
        MaxExp = value;
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
    public void SetDeath(bool value)
    {
        IsDead = value;
    }
    #endregion

    #endregion


    public void Init()
    {
        canMove = true;
        canAct = true;
        //useSkills = true;
        canUlt = false;
        InCombat = false;
        InRegen = false;

        Health = MaxHealth;
        //deathEffect.SetActive(false);
        //for (int i = 0; i < disableOnDeath.Length; i++)
        //{
        //    disableOnDeath[i].enabled = true;
        //}
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
        CharacterStatsSetUp();
    }

    public void CharacterStatsSetUp()
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
        SetMaxExp(100);
        ExpRate = 1.85f;
        SetDegPhys(100);
        SetDegMag(100);


        respawnCooldown = 10.0f;

        SetExp(0);
        SetLvl(1);
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
            if (gameObject.CompareTag("Player"))
            {
                var rend = GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.enabled = false;
                }
                StartCoroutine(Spawn(rend));
            }
            else if (gameObject.CompareTag("dd"))
            {

            }
            else
            {
                PhotonNetwork.Destroy(gameObject.GetComponent<PhotonView>());
            }
        }
    }

    IEnumerator Spawn(Renderer rend)
    {
        if(transform.position != deathPos)
        {
            transform.position = deathPos;
            yield return new WaitForSeconds(respawnCooldown);

            SetDefault(rend);
            transform.position = respawnPos;
        }
    }

    public void SetDefault(Renderer rend)
    {
        Health = MaxHealth;
        Mana = MaxMana;
        //deathEffect.SetActive(false);
        //for (int i = 0; i < disableOnDeath.Length; i++)
        //{
        //    disableOnDeath[i].enabled = true;
        //}
        rend.enabled = true;
    }

    public void CheckTarget()
    {   
        if(Cible == null)
        {
            Cible = null;
        }

        if(Cible != null)
        {
            if (Cible.CompareTag("Player"))
            {
                if(Cible.GetComponent<IDamageable>().GetHealth() <= 0)
                {
                    Cible = null;
                }
            }
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
        var TypeConvert = 0;
        switch (type)
        {
            case DamageType.physique:
                TypeConvert = 0;
                break;
            case DamageType.magique:
                TypeConvert = 1;
                break;
            case DamageType.brut:
                TypeConvert = 2;
                break;
        }

        photonView.RPC("Damages", RpcTarget.All, new object[] { DegatsRecu, TypeConvert });
    }

    [PunRPC]
    public void Damages(float DegatsRecu, int type)
    {
        switch (type)
        {
            case 0:
                Health = Health - (DegatsRecu - ((ResistancePhysique * DegatsRecu) / 100)); // physique
                break;
            case 1:
                Health = Health - (DegatsRecu - ((ResistanceMagique * DegatsRecu) / 100)); // magique
                break;
            case 2:
                Health -= DegatsRecu;
                break;
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
            var test = GetViewRange();
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, GetViewRange());
            if (hitColliders != null)
            {
                foreach (var col in hitColliders)
                {                    
                    if(col.GetComponent<IDamageable>())
                    {
                        if (col.GetComponent<IDamageable>().team != team)
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
public enum Team
{
    Veritas,
    Dominion
}
