using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class IDamageable : NetworkBehaviour
{

    
    public enum AttackType { Melee, Ranged }
    public AttackType attackType;
    public GameObject Cible;
    [Header("Stats")]
    public float Health = 500, MaxHealth = 500;
    public float MoveSpeed = 4.5f;
    public float AttackSpeed = 0.5f;
    public float AttackRange = 1.5f;
    public float Mana = 100, MaxMana = 100;
    public float ResistancePhysique = 0; // calcul des resistance health = health - (DegatsPhysiqueReçu -((ResistancePhysique * DegatsPhysiqueReçu)/100)
    public float ResistanceMagique = 0; //calcul des resistance health = health - (DegatsMagiqueReçu - ((ResistanceMagique * DegatsMagiqueReçu) / 100)
    public float Exp = 0;
    public float MaxExp = 100;
    public float ExpRate = 1.75f;//multiplicateur de l'exp max
    public float DegatsPhysique = 100;
    public float DegatsMagique = 100;
    public int lvl = 1;
    //public bool canMove;
    public bool canAct;

    public bool canUlt = false;
    public bool InCombat = false;
    public bool InRegen = false;

    [Header("Ranged variables")]
    public GameObject projPrefab;
    public Transform SpawnPrefab;

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
            MoveSpeed += 0.75f;
        }
    }
   

    IEnumerator CoolDown(Skills skill)
    {
        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
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

    public bool IsTargetable(Targetable.EnemyType enemyToCompare)
    {
        if(enemyToCompare == Targetable.EnemyType.minion ||
        enemyToCompare == Targetable.EnemyType.voister ||
        enemyToCompare == Targetable.EnemyType.joueur ||
        enemyToCompare == Targetable.EnemyType.dieu ||
        enemyToCompare == Targetable.EnemyType.golem)
        {
            return true;
        }
        return false;
    }
}


