using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RoarBehaviour : Projectile
{

    public MauBehaviour source;
    public Vector3 direction;
    private float timerDefault;
    //Speed bigger
    public Vector3 scaleChange;
    [SerializeField]private VisualEffect roar_animation;
    private VisualEffect roar_anim;

    public new void Start()
    {
        timerDefault = 1f;
        //roar_anim = Instantiate(roar_animation, transform.position, transform.rotation, transform);
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
            //roar_anim.transform.localScale += scaleChange;
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
