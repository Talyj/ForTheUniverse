using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStickBehaviour : Projectile
{
    public SunBehaviour source;

    public new void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    public new void Update()
    {
        transform.Rotate(0, 1, 0);
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        source.TP(gameObject.transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var trig = other.GetComponent<IDamageable>();
        if (trig)
        {
            if(trig.team != source.team)
            {
                source.TP(gameObject.transform.position);
                Destroy(gameObject);
            }
        }
    }
}
