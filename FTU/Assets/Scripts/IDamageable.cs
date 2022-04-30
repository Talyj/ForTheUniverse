using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class IDamageable : NetworkBehaviour
{

    
    public enum AttackType { Melee, Ranged }
    public AttackType attackType;
    [SerializeField]
    ControlType cc;
    [SerializeField]
    EnemyType enemyType;
    public GameObject Cible;
    [Header("Stats")]
    public float Health = 500, MaxHealth = 500;
    public float MoveSpeed = 4.5f;
    public float AttackSpeed = 0.5f;
    public float AttackRange = 1.5f;
    public float Mana = 100, MaxMana = 100;
    public float ResistancePhysique = 0; // calcul des resistance health = health - (DegatsPhysiqueRe�u -((ResistancePhysique * DegatsPhysiqueRe�u)/100)
    public float ResistanceMagique = 0; //calcul des resistance health = health - (DegatsMagiqueRe�u - ((ResistanceMagique * DegatsMagiqueRe�u) / 100)
    public float Exp = 0;
    public float MaxExp = 100;
    public float ExpRate = 1.75f;//multiplicateur de l'exp max
    public float DegatsPhysique = 100;
    public float DegatsMagique = 100;
    public int lvl = 1;
    public bool canMove;
    public bool canAct;
    public bool canMove = true;
    public bool useSkills = true;
    public bool canUlt = false;
    public bool InCombat = false;
    public bool InRegen = false;

    [Header("Ranged variables")]
    public GameObject projPrefab;
    public Transform SpawnPrefab;

    public EnemyType enemytype;
    public enum EnemyType
    {
        minion,
        golem,
        joueur,
        dieu,
        voister
    }
    public Transform SpawnPrefab2;

    //public enum EnemyType
    //{
    //    minion,
    //    golem,
    //    joueur,
    //    dieu,
    //    voister
    //}



    #region Getter and Setter

    #region Getter
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
    public float GetArmor()
    {
        return ResistancePhysique;
    }
    public float GetRM()
    {
        return ResistanceMagique;
    }
    public float GetExp()
    {
        return Exp;
    }
    public float GetAD()
    {
        return DegatsPhysique;
    }
    public float GetAP()
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
    public void SetHealth(float value)
    {
        Health += value;
    }
    public void SetMaxHealth(float value)
    {
        MaxHealth += value;
    }
    public void SetMana(float value)
    {
        Mana += value;
    }
    public void SetMoveSpeed(float value)
    {
        MoveSpeed += value;
    }
    public void SetAttackSpeed(float value)
    {
        AttackSpeed += value;
    }
    public void SetAttackRange(float value)
    {
        AttackRange += value;
    }
    public void SetArmor(float value)
    {
        ResistancePhysique += value;
    }
    public void SetRM(float value)
    {
        ResistanceMagique += value;
    }
    public void SetExp(float value)
    {
        Exp += value;
    }
    public void SetAD(int value)
    {
        DegatsPhysique += value;
    }
    public void SetAP(int value)
    {
        DegatsMagique += value;
    }
    public void SetLvl(int value)
    {
        lvl += value;
    }
    #endregion

    #endregion


    // Start is called before the first frame update
    void Start()
    {
       
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




    public void TakeDamage(float DegatsRecu, string type)
    {
        //application des res, a modifier pour les differents type de degats
        if (type == "Physique")
        {
            Health = Health - (DegatsRecu - ((ResistancePhysique * DegatsRecu) / 100)); // physique
        }
        else if (type == "Magique")
        {
            Health = Health - (DegatsRecu - ((ResistanceMagique * DegatsRecu) / 100)); // magique
        }
        else if (type == "Brut")
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
    public bool IsDead()
    {
        if (Health <= 0)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
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
}

public enum EnemyType
{
    minion,
    golem,
    joueur,
    dieu,
    voister
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

