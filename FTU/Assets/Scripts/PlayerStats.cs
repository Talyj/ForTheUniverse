using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamegeable
{

    [Header("Stats")]
    [SerializeField]
    float Health = 500;
    [SerializeField]
    float MoveSpeed = 4.5f;
    [SerializeField]
    float AttackSpeed = 0.5f;
    [SerializeField]
    float Mana = 100;
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

    public void TakeDamage(float DegatsRecu)
    {
        //application des res, a modifier pour les differents type de degats

        Health = Health - (DegatsRecu - ((ResistancePhysique * DegatsRecu) / 100)); // physique

    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Xp avec calcule des reste d'exp si lvl up

        if(Exp >= MaxExp)
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
            TakeDamage(100);
            print(Health);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Exp += 90;
            print(Health);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Mana -= 40;
            print(Mana);
        }
    }
}
