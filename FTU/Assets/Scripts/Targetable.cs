using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public enum EnemyType
    {
        minion,
        golem,
        joueur,
        dieu
    }
    public EnemyType enemytype;

    private float Health;
    public bool canMove;
    private void Start()
    {
        Health = 200;
    }

    public void TakeDamage(float DegatsRecu, string type)
    {
        //application des res, a modifier pour les differents type de degats
        if (type == "Physique")
        {
            Health = Health - (DegatsRecu); // physique
        }
        else if (type == "Magique")
        {
            Health = Health - (DegatsRecu); // magique
        }
        else if (type == "Brut")
        {
            Health -= DegatsRecu;
        }
    }
}
