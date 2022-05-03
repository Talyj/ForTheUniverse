using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddlePoissoinBehaviour : Projectile
{
    private int insideArea;
    private List<GameObject> targets = new List<GameObject>();
    private bool isActive;
    public MermaidBehaviour source;

    // Start is called before the first frame update
    public new void Start()
    {
        insideArea = 0;
        isActive = false;
    }
    // Update is called once per frame
    public new void Update()
    {
        //Behaviour();
        if (insideArea > 0 && !isActive)
        {
            StartCoroutine(DealDamage(targets, degats, "Magique"));
        }
        Destroy(gameObject, 10);
    }

    public IEnumerator DealDamage(List<GameObject> targets, float dmg, string typeDmg)
    {
        while(insideArea > 0)
        {
            if(targets.Count > 0)
            {
                isActive = true;
                foreach(var targ in targets)
                {
                    if (targ.GetComponent<IDamageable>().team != source.team)
                    {
                        targ.GetComponent<IDamageable>().TakeDamage(dmg, typeDmg);
                    }
                    else
                    {
                        targ.GetComponent<IDamageable>().SetHealth(targ.GetComponent<IDamageable>().Health + dmg);
                    }
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        isActive = false;
        yield return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("minion") || other.CompareTag("golem"))
        {
            insideArea += 1;
            targets.Add(other.gameObject);
            Debug.Log("ekip");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideArea -= 1;
            targets.Remove(other.gameObject);
            Debug.Log("stop");
        }
    }
}
