using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*                                                    */
/*     This class can be replaced by IDamageable      */
/*                                                    */

public enum ControlType
{
    stun,
    bump,//en l'air
    root,//immobiliser mais pas stun
    charme
}
public class Targetable : IDamageable
{
    public enum EnemyType
    {
        minion,
        golem,
        joueur,
        dieu,
        voister
    }
    public EnemyType enemytype;

    public bool canMove;
    //public void Start()
    //{
    //    Health = 200;
    //    canAct = true;
    //    canMove = true;
    //}

    //public void Update()
    //{
    //    Debug.Log("Health : " + Health);
    //}

    //public void TakeDamage(float DegatsRecu, string type)
    //{
    //    //application des res, a modifier pour les differents type de degats
    //    if (type == "Physique")
    //    {
    //        Health = Health - (DegatsRecu); // physique
    //    }
    //    else if (type == "Magique")
    //    {
    //        Health = Health - (DegatsRecu); // magique
    //    }
    //    else if (type == "Brut")
    //    {
    //        Health -= DegatsRecu;
    //    }
    //}
}
