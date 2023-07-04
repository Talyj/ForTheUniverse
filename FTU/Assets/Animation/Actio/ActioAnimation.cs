using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.VFX;

public class ActioAnimation : MonoBehaviour {
    public Animator animator;

    public VisualEffect explosion_dash1;
    public VisualEffect explosion_dash2;
    public VisualEffect explosion_ulti;

    public GameObject trap;


    void Start() {
        trap.SetActive(false);
    }
    
    void Update() {
        /*if (Input.GetKeyDown(KeyCode.A)) { // Autoattack
            animator.SetTrigger("Shoot");
        }

        if(Input.GetKeyDown(KeyCode.R)) { // Ulti
            animator.SetTrigger("Ulti");
            StartCoroutine(TriggerVFXAfterDelay(explosion_ulti, 0.4f));
            StartCoroutine(PutTrap(0.5f));
        }

        /*if (Input.GetKey(KeyCode.Z)) { //Mouvement quand il avance
            animator.SetBool("Walk", true);
        } else {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.Z)) { // Dash
            animator.SetTrigger("Dash");
            StartCoroutine(TriggerVFXAfterDelay(explosion_dash1, 0.2f));
            StartCoroutine(TriggerVFXAfterDelay(explosion_dash2, 0.2f));
        }*/
    }

    private IEnumerator TriggerVFXAfterDelay(VisualEffect vfx, float delay) {
        yield return new WaitForSeconds(delay);
        vfx.SendEvent("OnPlay");
    }

    private IEnumerator PutTrap(float delay) {
        yield return new WaitForSeconds(delay);
        GameObject trapchild = PhotonNetwork.Instantiate(trap.name, transform.position, transform.rotation);
        trap.GetComponent<Trap>().dps = gameObject.GetComponentInParent<ActioBehaviour>();
        //trapchild.transform.parent = transform.parent.transform;
        trapchild.SetActive(true);
    }

    public void Skill1Animation()
    {
        animator.SetTrigger("Shoot");
    }

    public void Skill2Animation()
    {
        animator.SetTrigger("Dash");
        StartCoroutine(TriggerVFXAfterDelay(explosion_dash1, 0.2f));
        StartCoroutine(TriggerVFXAfterDelay(explosion_dash2, 0.2f));
    }

    public void UltimateAnimation()
    {
        animator.SetTrigger("Ulti");
        StartCoroutine(TriggerVFXAfterDelay(explosion_ulti, 0.4f));
        StartCoroutine(PutTrap(0.5f));
    }

}
