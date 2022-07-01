using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball1 : Projectile
{
    public Dps1 dps;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<IDamageable>())
        {
            if (col.gameObject.GetComponent<IDamageable>().team != dps.team)
            {
                col.gameObject.GetComponent<IDamageable>().TakeCC(IDamageable.ControlType.slow, 2.55f);
                Destroy(gameObject);
            }
        }

    }
}
