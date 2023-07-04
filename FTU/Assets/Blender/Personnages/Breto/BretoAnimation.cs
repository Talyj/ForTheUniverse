using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BretoAnimation : MonoBehaviour {
    public Animator animator;

    public VisualEffect auto_explosion;
    public VisualEffect destroy_explosion;

    public VisualEffect slash;
    public ParticleSystem sort1;

    public GameObject trident;
    public GameObject sonar;

    public GameObject ulti;

    public float lifetime_ulti;

    public float temps_sonar;
    
    private GameObject ultiPref = null;



    void Start() {
        trident.SetActive(false);

        sonar.SetActive(false);
    }
    
    void Update() {
        /*if (Input.GetKeyDown(KeyCode.A)) { // Autoattack
            animator.SetTrigger("Auto");
            StartCoroutine(PutTrident(0.2f));
            StartCoroutine(TriggerVFXAfterDelay(slash, 0.3f));
        }*/
        
        /*if (Input.GetKeyDown(KeyCode.A)) { // Dash
            animator.SetTrigger("Dash");
            sort1.Play();
        }

        if(Input.GetKeyDown(KeyCode.Z)) { // Sonar
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
        }*/

        /*if (Input.GetKey(KeyCode.Z)) { //Mouvement quand il avance
            animator.SetBool("Walk", true);
        } else {
            animator.SetBool("Walk", false);
        }*/

        
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
        ultiPref = PhotonNetwork.Instantiate(ulti.name, /*transform.parent.*/transform.position, /*transform.parent.*/transform.rotation);
        //GetComponentInParent<BretoBehaviour>().Ultime(ulti);
        ultiPref.SetActive(true);
    }
    
    private IEnumerator PutTrident(float delay) {
        yield return new WaitForSeconds(delay);
        if(!trident.activeSelf) {
            auto_explosion.SendEvent("OnPlay");
            trident.SetActive(true);
        }
    }

    private IEnumerator RunSonar(float delay) {
        GetComponentInParent<BretoBehaviour>().Skill2();
        yield return new WaitForSeconds(delay);
        StartCoroutine(Sonar());
    }

    private IEnumerator Sonar()
    {
        GameObject wave = PhotonNetwork.Instantiate(sonar.name, transform.position, transform.rotation);
        wave.transform.parent = transform;
        wave.GetComponent<ScanBehaviour>().source = GetComponentInParent<BretoBehaviour>();
        wave.SetActive(true);
        yield return new WaitForSeconds(temps_sonar/5);
        //for (int i = 0; i < 5; i++)
        //{
        //}
    }

    private IEnumerator TriggerVFXAfterDelay(VisualEffect vfx, float delay) {
        yield return new WaitForSeconds(delay);
        vfx.SendEvent("OnPlay");
    }

    public void Skill1Animation()
    {
        animator.SetTrigger("Dash");
        sort1.Play();
    }
    
    public void Skill2Animation()
    {
        animator.SetTrigger("Sonar");
        if(trident.activeSelf) {
            StartCoroutine(DestroyTrident(0.0f));
        }
        StartCoroutine(RunSonar(0.3f));
    }
    
    public GameObject UltimateAnimation()
    {
        animator.SetTrigger("Ulti");
        if(trident.activeSelf) {
            StartCoroutine(DestroyTrident(0.0f));
        }
        StartCoroutine(PutUlti(0.2f));
        StartCoroutine(DestroyUlti(lifetime_ulti));
        return ultiPref;
    }
}
