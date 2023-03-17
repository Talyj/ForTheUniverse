using Photon.Pun;
using System.Collections;
using UnityEngine;


public class DashBehaviour : Projectile
{
    public BretoBehaviour source;

    // Use this for initialization
    private new void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DestroyBullet(3f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable)
        {
            if(damageable.IsTargetable(source.team))
            {
                other.GetComponent<IDamageable>().TakeDamage(source.skills[0].Damage, IDamageable.DamageType.physique);
                Debug.Log("BBABABABABA");
            }

        }                    
    }
}
