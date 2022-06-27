using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissoinProjBehaviour : Projectile
{
    public MermaidBehaviour source;
    public IDamageable.Team team;
    [SerializeField] private GameObject puddle;

    public new void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    public new void Update()
    {
        
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnPuddle());
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {      
        if(other.GetComponent<IDamageable>())
        {
            if (other.GetComponent<IDamageable>().team != team)
            {
                source.AddPassive();
                DealDamage(other.gameObject, GetDamages(), GetDamageType());
                StartCoroutine(SpawnPuddle());
                stopProjectile = true;
                Destroy(gameObject);
            }
        }

    }

    private IEnumerator SpawnPuddle()
    {
        var pud = PhotonNetwork.Instantiate(puddle.name, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        pud.GetComponent<PuddlePoissoinBehaviour>().team = team;
        
        yield return 0;
    }
}
