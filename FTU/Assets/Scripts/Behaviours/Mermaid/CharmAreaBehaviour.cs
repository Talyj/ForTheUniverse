using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharmAreaBehaviour : NetworkBehaviour
{
    public MermaidBehaviour source;

    private void Update()
    {
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("minion"))
        {
            if(other.gameObject.GetComponent<IDamageable>().team != source.team){
                source.AddTCharmedTargets(other.gameObject);

            }
        }
    }
}
