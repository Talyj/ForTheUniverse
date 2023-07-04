using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaelstromBehaviour : Projectile
{
    public BretoBehaviour source;
    private float cpt;
    private float cptDmg;
    List<GameObject> colliders;
    public new void Start()
    {
        colliders = new List<GameObject>();
        cpt = 0;
        StartCoroutine(DestroyBullet(5f));
        source.UltPos = transform.position;
    }

    // Update is called once per frame
    public new void Update()
    {
        cpt -= Time.deltaTime;
        if(cpt <= 0)
        {
            cpt = 0.1f;
            foreach(var col in colliders)
            {
                source.AddCaughtTargets(col);
            }
        }
        DealDamagesPerSeconds();
    }

    public void DealDamagesPerSeconds()
    {
        cptDmg -= Time.deltaTime;
        if(cptDmg <= 0)
        {
            cptDmg = 1f;
            foreach(var col in colliders)
            {
                col.GetComponent<IDamageable>().TakeDamage(source.skills[2].Damage, IDamageable.DamageType.magique, source.photonView.ViewID);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>() && photonView.IsMine)
        {
            if(other.gameObject.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.player ||
                other.gameObject.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.minion)
            {
                if (other.gameObject.GetComponent<IDamageable>().team != source.team)
                {
                    colliders.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>() && photonView.IsMine)
        {
            if (other.gameObject.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.player ||
                other.gameObject.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.minion)
            {
                if (other.gameObject.GetComponent<IDamageable>().team != source.team)
                {
                    colliders.Remove(other.gameObject);
                }
            }
        }
    }
}
