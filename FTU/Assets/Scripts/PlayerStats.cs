using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamegeable, ISkill
{

    [Header("Stats")]
    [SerializeField]
    float Health = 500,MaxHealth=500;
    [SerializeField]
    float MoveSpeed = 4.5f;
    [SerializeField]
    float AttackSpeed = 0.5f;
    [SerializeField]
    float Mana = 100,MaxMana=100;
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
    int DegatsPhysique = 100;
    [SerializeField]
    int DegatsMagique = 100;
    [SerializeField]
    int lvl = 1;
    [SerializeField]
    Skills[] Skills;

    public void TakeDamage(float DegatsRecu,string type)
    {
        //application des res, a modifier pour les differents type de degats
        if(type == "Physique")
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





    // Start is called before the first frame update
    void Start()
    {
        Passif();
    }

    // Update is called once per frame
    void Update()
    {

        if (Health >= MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Mana >= MaxMana)
        {
            Mana = MaxMana;
        }

        //Xp avec calcule des reste d'exp si lvl up

        if (Exp >= MaxExp)
        {
            float reste = Exp - MaxExp;
            lvl += 1;
            Exp = 0 + reste;
            MaxExp = MaxExp * ExpRate;
            // augmentation des stats a faire
        }





        // test des touches
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100,"Physique");
            
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(100, "Magique");

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(50, "Brut");

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Exp += 90;
            print(Health);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Skill1();
        }
    }

    // a rendre plus generique
    IEnumerator CoolDown(float cd)
    {
        yield return new WaitForSeconds(cd);
        Debug.Log("fin des cd");
        Skills[0].isCooldown = false;
    }

    //function de regen mana et vie

    public void Regen(float rateHp, float rateMana)
    {
        Health = Health * rateHp;
        Mana = Mana * rateMana;
    }

    public void Passif()
    {
        //test bigED passif
        ResistanceMagique = ResistanceMagique * 1.05f;//augmentation 5%
        ResistancePhysique = ResistancePhysique * 1.05f;
        MoveSpeed = MoveSpeed * 0.95f;//reduction 5%
    }

    public void Skill1()
    {
        if(Skills[0].isCooldown == false && Mana >= Skills[0].Cost)
        {
            Mana -= Skills[0].Cost;
            Debug.Log(Skills[0].Name + " lancée");
            Skills[0].isCooldown = true;
            if (Skills[0].isCooldown == true)
            {
                StartCoroutine(CoolDown(Skills[0].Cooldown));
            }
        }
        else if (Skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if ( Mana < Skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public void Skill2()
    {
        throw new System.NotImplementedException();
    }

    public void Ultime()
    {
        throw new System.NotImplementedException();
    }

    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
