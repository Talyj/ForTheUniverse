using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.VFX;

public class MermaidAnimation : MonoBehaviour {
    public Animator animator;
    public VisualEffect sort1; 
    public VisualEffect sort1_explosion; 
    public VisualEffect sort2; 
    public ParticleSystem ulti;

    public GameObject fishPrefab; // Les prefabs 
    public GameObject flaquePrefab; 

    private GameObject currentFish; // Les objets actuels
    private GameObject currentFlaque; 

    public Vector3 pointA; // Le point de depart POINT A A NE PAS CHANGER 
    public Vector3 pointB; // Le point d'arrivee POINT B EN FONCTION DU SPELL A VOIR AVEC JULIEN

    public float speed = 1.0f; // La vitesse dU poisson
    public float life_flaque = 5.0f; 


    void Update() {

        if (Input.GetKeyDown(KeyCode.A)) { // Sort 1
            animator.SetTrigger("Auto");
            StartCoroutine(TriggerVFXAfterDelay(sort1, 0.9f));
            StartCoroutine(SpawnFish(0.9f));
        }

        if(Input.GetKeyDown(KeyCode.Z)) { // Sort 2
            animator.SetTrigger("Sort2");
            StartCoroutine(TriggerVFXAfterDelay(sort2, 0.5f));
        }

        /*if (Input.GetKey(KeyCode.Z)) // Animation de quand elle avance
        {
            animator.SetBool("Walk", true);
        } else {
            animator.SetBool("Walk", false);
        }*/

        if (Input.GetKeyDown(KeyCode.R)) { // Ulti
            animator.SetTrigger("Ulti");
            ulti.Play(); 
        }

        //CHECK POUR LE SORT 1 

        if (currentFish != null) {
            currentFish.transform.position = Vector3.MoveTowards(currentFish.transform.position, transform.TransformPoint(pointB), speed * Time.deltaTime); // Deplace le poisson de pointA vers pointB

            Vector3 direction = (transform.TransformPoint(pointB) - currentFish.transform.position).normalized; // Fait tourner le poisson pour qu'il regarde vers pointB
            if (direction != Vector3.zero) {
                currentFish.transform.rotation = Quaternion.LookRotation(direction);
            }
            
            if (currentFish.transform.position == transform.TransformPoint(pointB)) { // Met la flaque
                sort1_explosion.transform.position = transform.TransformPoint(pointB);
                StartCoroutine(TriggerVFXAfterDelay(sort1_explosion, 0.0f));

                currentFlaque = Instantiate(flaquePrefab, transform.TransformPoint(pointB) - new Vector3(0, pointB[1], 0), Quaternion.identity);
                Destroy(currentFish);
            }

            if (Input.GetKeyDown(KeyCode.A)) {  // Si on relance un poisson, supprime le precedent,  PAS NECESSAIRE SI IL Y A DU TEMPS ENTRE DEUX SPELL 1
                Destroy(currentFish);
            }
        }

        if(currentFlaque != null) { // Detruit la flaque au bout de X SECONDES
            StartCoroutine(DestroyFlaque(life_flaque));
        }
        
    }

    private IEnumerator SpawnFish(float delay) {
        yield return new WaitForSeconds(delay);
        currentFish =  PhotonNetwork.Instantiate(fishPrefab.name, transform.TransformPoint(pointA), Quaternion.identity);
        //GetComponentInParent<MermaidBehaviour>().poissoin = currentFish;
        GetComponentInParent<MermaidBehaviour>().Poissoin(currentFish);
    }

    private IEnumerator DestroyFlaque(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(currentFlaque);
    }
    
    private IEnumerator DestroyVFX(VisualEffect vfx,float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(vfx.transform.parent.gameObject);
    }

    private IEnumerator TriggerVFXAfterDelay(VisualEffect vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        VisualEffect newVFX = vfx;
        if (vfx == sort2)
        {
            GameObject parentVFX = new GameObject("VFXObject");
            parentVFX.transform.position = vfx.gameObject.GetComponentInParent<Transform>().position;
            parentVFX.transform.rotation = vfx.gameObject.GetComponentInParent<Transform>().rotation;
            newVFX = Instantiate(vfx, parentVFX.transform.position, parentVFX.transform.rotation , parentVFX.transform);
        }

        newVFX.SendEvent("OnPlay");

        if (vfx == sort2)
        {
            StartCoroutine(DestroyVFX(newVFX, newVFX.GetFloat("TsunamiLifetime")));
        }
        
    }
}
