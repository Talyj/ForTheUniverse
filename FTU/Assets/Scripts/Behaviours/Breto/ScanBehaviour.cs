using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBehaviour : Projectile
{
    public BretoBehaviour source;
    private Dictionary<IDamageable, LayerMask> previousMasks;

    public new void Start()
    {
        StartCoroutine(DestroyBullet(5f));
    }

    // Update is called once per frame
    public new void Update()
    {
        //cpt -= Time.deltaTime;
        //if (cpt <= 0)
        //{
        //    cpt = 0.1f;
        //    foreach (var col in colliders)
        //    {
        //        source.AddCaughtTargets(col);
        //    }
        //}
        transform.position = source.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() && photonView.IsMine)
        {
            if(other.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.player ||
                other.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.minion ||
                other.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.voister)
            {
                if (other.gameObject.GetComponent<IDamageable>().team != source.team)
                {
                    //TODO might have a problem if the target situation changes
                    previousMasks.Add(other.GetComponent<IDamageable>(), other.gameObject.layer);
                    //TODO might have to change the default layer ?
                    other.gameObject.layer = LayerMask.NameToLayer("Default");                
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDamageable>() && photonView.IsMine)
        {
            if (previousMasks.ContainsKey(other.GetComponent<IDamageable>()))
            {
                other.gameObject.layer = previousMasks[other.GetComponent<IDamageable>()].value;
                previousMasks.Remove(other.GetComponent<IDamageable>());
            }
        }
    }
}
