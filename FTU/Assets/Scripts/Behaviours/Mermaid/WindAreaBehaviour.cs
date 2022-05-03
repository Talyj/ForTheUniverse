using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAreaBehaviour : Projectile
{
    public MermaidBehaviour source;
    public Vector3 direction;
    private float timerDefault;
    public float speed;
    //Speed bigger
    public Vector3 scaleChange;

    public new void Start()
    {
        //scaleChange = new Vector3(0.1f, 0.0f, 0.0f);
        //speed = 50;
        timerDefault = 0.5f;
        StartCoroutine(Movement());
        StartCoroutine(SizeModification());
    }

    public new void Update()
    {
        //Destroy(gameObject, 1);
    }

    public IEnumerator Movement()
    {
        float timer = timerDefault;
        while(timer >= 0)
        {
            //transform.Translate(direction * speed * Time.deltaTime);
            transform.position += direction.normalized * Time.deltaTime * speed;
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
        Destroy(gameObject);
        yield return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("minion"))
        {
            if(other.gameObject.GetComponent<IDamageable>().team != source.team)
            {
                source.AddWindedTarget(other.gameObject);
            }
        }
    }
}
