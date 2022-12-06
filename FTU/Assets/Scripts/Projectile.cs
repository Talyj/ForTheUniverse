using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{

    private IDamageable.DamageType typeDegats;
    [SerializeField] private float degats;
    public float vitesse = 30;
    public GameObject target;
    public bool targetSet;
    protected bool stopProjectile = false;
    public bool touched;

    //public void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    public void Start()
    {
        touched = false;
        DontDestroyOnLoad(gameObject);
        DestroyBullet(10f);
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            if(target == null)
            {
               PhotonNetwork.Destroy(gameObject);
            }
            Behaviour();
        }
    }

    public void Behaviour()
    {
        if (target)
        {
            if (target == null)
            {
                target = null;
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, vitesse * Time.deltaTime);

            if (stopProjectile == false)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 0.75f)
                    //if (touched)
                {
                    if (target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.minion ||
                        target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.voister ||
                        target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.player ||
                        target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.dieu ||
                        target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.golem)
                    {
                            touched = true;
                            DealDamage(target, degats, typeDegats);
                            stopProjectile = true;
                            PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void SetDamages(float dmg, IDamageable.DamageType typeDmg)
    {
        degats = dmg;
        typeDegats = typeDmg;
    }

    public float GetDamages()
    {
        return degats;
    }

    public IDamageable.DamageType GetDamageType()
    {
        return typeDegats;
    }

    public void DealDamage(GameObject target, float dmg, IDamageable.DamageType typeDmg)
    {
        target.GetComponent<IDamageable>().TakeDamage(dmg, typeDmg);
    }

    protected IEnumerator DestroyBullet(float time)
    {
        yield return new WaitForSeconds(time);
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
