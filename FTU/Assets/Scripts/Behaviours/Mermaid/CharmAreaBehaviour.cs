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
        if (other.CompareTag("cube"))
        {
            source.AddTCharmedTargets(other.gameObject);
        }
    }
}
