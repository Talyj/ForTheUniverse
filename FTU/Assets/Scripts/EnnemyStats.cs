using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    float Health = 500, MaxHealth = 500;
    [SerializeField]
    float MoveSpeed = 4.5f;
    [SerializeField]
    float AttackSpeed = 0.5f;
    float AttackRange = 1.5f;
    [SerializeField]
    float Mana = 100, MaxMana = 100;
    [SerializeField]
    float ResistancePhysique = 40; // calcul des resistance health = health - (DegatsPhysiqueReçu -((ResistancePhysique * DegatsPhysiqueReçu)/100)
    [SerializeField]
    float ResistanceMagique = 40; //calcul des resistance health = health - (DegatsMagiqueReçu - ((ResistanceMagique * DegatsMagiqueReçu) / 100)
    [SerializeField]
    float Exp = 0;
    [SerializeField]
    float MaxExp = 100;
    [SerializeField]
    float ExpRate = 1.75f;//multiplicateur de l'exp max
    [SerializeField]
    float DegatsPhysique = 100;
    [SerializeField]
    float DegatsMagique = 100;
    [SerializeField]
    int lvl = 1;
    //[Header("Competences")]
    //[SerializeField]
    //Passifs passif;
    //[SerializeField]
    //Skills[] Skills;
    //[SerializeField]
    //bool canUlt = false;
    //[SerializeField]
    //bool InCombat = false;
    //[SerializeField]
    //bool InRegen = false;




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

    //public Skills[] GetSkills()
    //{
    //    return Skills;
    //}
    //public Skills GetSkill1()
    //{
    //    return Skills[0];
    //}
    //public Skills GetSkill2()
    //{
    //    return Skills[1];
    //}
    //public Skills GetUlt()
    //{
    //    return Skills[2];
    //}
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
}
