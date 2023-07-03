using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActio : MonoBehaviour {
    public Animator animator;
    public float lifetime;

    void Start() {
        animator.enabled = false;
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.X)) { // Mettre le if de quand quelqu'un rentre dedans
            animator.enabled = true;
            StartCoroutine(DestroyTrap(lifetime));
        }
    }

        
    private IEnumerator DestroyTrap(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
