using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissoinProjBehaviour : Projectile
{
    public MermaidBehaviour source;
    public Team team;
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
        if (photonView.IsMine)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(SpawnPuddle());
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if(other.GetComponent<IDamageable>())
            {
                if (other.GetComponent<IDamageable>().team != team && photonView.IsMine)
                {
                    StartCoroutine(SpawnPuddle());
                    DealDamage(other.gameObject, GetDamages(), GetDamageType());
                    source.AddPassive();
                    stopProjectile = true;
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    private IEnumerator SpawnPuddle()
    {
        if (photonView.IsMine)
        {
            var pud = PhotonNetwork.Instantiate(puddle.name, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
            pud.GetComponent<PuddlePoissoinBehaviour>().team = team;
        }
        
        yield return 0;
    }
}
