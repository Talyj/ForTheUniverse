using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarBehaviour : Projectile
{

    public MauBehaviour source;
    public Vector3 direction;
    private float timerDefault;
    //Speed bigger
    public Vector3 scaleChange;

    public new void Start()
    {
        timerDefault = 1f;
        StartCoroutine(Movement());
        StartCoroutine(SizeModification());
        StartCoroutine(DestroyBullet(timerDefault));
    }

    // Update is called once per frame
    public new void Update()
    {
        
    }

    public IEnumerator Movement()
    {
        float timer = timerDefault;
        while (timer >= 0)
        {
            //transform.Translate(direction * speed * Time.deltaTime);
            transform.position += direction.normalized * Time.deltaTime * vitesse;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return 0;
    }

    public IEnumerator SizeModification()
    {
        float timer = timerDefault;
        while (timer >= 0)
        {
            transform.localScale += scaleChange;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("minion") || other.CompareTag("golem"))
        {
            if (other.gameObject.GetComponent<IDamageable>().team != source.team)
            {
                source.RoaredDamageTarget(other.gameObject);
            }
        }
    }
}
