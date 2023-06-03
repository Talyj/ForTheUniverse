using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigStickBehaviour : Projectile
{
    private float timerDefault;
    public Vector3 direction;
    private Vector3 startPos;
    private Vector3 currentPos;

    // Start is called before the first frame update
    public new void Start()
    {
        timerDefault = 0.5f;
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    public new void Update()
    {
        currentPos = gameObject.transform.position;
        StartCoroutine(Movement());
        CheckRange();
    }

    public void CheckRange()
    {
        float dist = Vector3.Distance(startPos, currentPos);
        if(dist >= 70 && photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            if(other.GetComponent<IDamageable>().team != team)
            other.GetComponent<IDamageable>().TakeDamage(GetDamages(), GetDamageType());
        }
    }
}
