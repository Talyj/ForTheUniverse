using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActio : MonoBehaviourPun {
    public Animator animator;
    public float lifetime;
    private ActioBehaviour source;

    void Start() {
        animator.enabled = false;
        source = GetComponent<Trap>().source;
    }
    
    void Update() {
        
    }

        
    private IEnumerator DestroyTrap(float delay) {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>())
        {
            if (other.GetComponent<IDamageable>().team.Code != source.team.Code)
            {
                animator.enabled = true;
                StartCoroutine(DestroyTrap(lifetime));
            }
        }
    }
}
