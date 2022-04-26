using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissoinProjBehaviour : Projectile
{
    public MermaidBehaviour source;
    [SerializeField] private GameObject puddle;

    public new void Start()
    {
        StartCoroutine(SpawnPuddle());
        touched = false;
    }

    public new void Update()
    {
        Behaviour();
        if(touched == true)
        {
            source.AddPassive();
        }        
    }

    private IEnumerator SpawnPuddle()
    {
        yield return new WaitForSeconds(1);
        Instantiate(puddle, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        Destroy(gameObject, 1);
    }
}
