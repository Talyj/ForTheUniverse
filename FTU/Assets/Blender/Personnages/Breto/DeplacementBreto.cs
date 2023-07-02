using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DeplacementBreto : MonoBehaviour {
    public Animator animator;

    public VisualEffect auto_explosion;
    public VisualEffect destroy_explosion;

    public VisualEffect slash;
    public ParticleSystem sort2;

    public GameObject trident;
    public GameObject sonar;

    public GameObject ulti;

    public float lifetime_ulti;

    public float temps_sonar;



    void Start() {
        trident.SetActive(false);

        sonar.SetActive(false);
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) { // Autoattack
            animator.SetTrigger("Auto");
            StartCoroutine(PutTrident(0.2f));
            StartCoroutine(TriggerVFXAfterDelay(slash, 0.3f));
        }

        if(Input.GetKeyDown(KeyCode.U)) { // Sonar
            animator.SetTrigger("Sonar");
            if(trident.activeSelf) {
                StartCoroutine(DestroyTrident(0.0f));
            }
            StartCoroutine(RunSonar(0.3f));
        }

        if(Input.GetKeyDown(KeyCode.R)) { // Ulti
            animator.SetTrigger("Ulti");
            if(trident.activeSelf) {
                StartCoroutine(DestroyTrident(0.0f));
            }
            StartCoroutine(PutUlti(0.2f));
            StartCoroutine(DestroyUlti(lifetime_ulti));
        }

        if (Input.GetKey(KeyCode.Z)) { //Mouvement quand il avance
            animator.SetBool("Walk", true);
        } else {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.E)) { // Dash
            animator.SetTrigger("Dash");
            sort2.Play();
        }
    }

    private IEnumerator DestroyTrident(float delay) {
        yield return new WaitForSeconds(delay);
        destroy_explosion.SendEvent("OnPlay");
        trident.SetActive(false);
    }

    private IEnumerator DestroyUlti(float delay) {
        yield return new WaitForSeconds(delay);
        ulti.SetActive(false);
    }

    private IEnumerator PutUlti(float delay) {
        yield return new WaitForSeconds(delay);
        ulti.SetActive(true);
    }
    
    private IEnumerator PutTrident(float delay) {
        yield return new WaitForSeconds(delay);
        if(!trident.activeSelf) {
            auto_explosion.SendEvent("OnPlay");
            trident.SetActive(true);
        }
    }

    private IEnumerator RunSonar(float delay) {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Sonar());
    }

    private IEnumerator Sonar() {
        for (int i = 0; i < 5; i++) {
            GameObject wave = Instantiate(sonar, transform.position, transform.rotation, transform);
            wave.SetActive(true);
            yield return new WaitForSeconds(temps_sonar/5);
        }
    }

    private IEnumerator TriggerVFXAfterDelay(VisualEffect vfx, float delay) {
        yield return new WaitForSeconds(delay);
        vfx.SendEvent("OnPlay");
    }
}
