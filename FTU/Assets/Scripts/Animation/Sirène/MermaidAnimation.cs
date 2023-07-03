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
    public GameObject windPrefab; 
    public GameObject charmePrefab; 

    private GameObject currentFish; // Les objets actuels
    private GameObject currentFlaque; 

    public Vector3 pointA; // Le point de depart POINT A A NE PAS CHANGER 
    public Vector3 pointB; // Le point d'arrivee POINT B EN FONCTION DU SPELL A VOIR AVEC JULIEN

    public float speed = 1.0f; // La vitesse dU poisson
    public float life_flaque = 5.0f;


    void Update() {

        /*if (Input.GetKeyDown(KeyCode.A)) { // Sort 1
            animator.SetTrigger("Auto");
            StartCoroutine(TriggerVFXAfterDelay(sort1, 0.9f));
            StartCoroutine(SpawnFish(0.9f));
        }

        if(Input.GetKeyDown(KeyCode.Z)) { // Sort 2
            animator.SetTrigger("Sort2");
            StartCoroutine(TriggerVFXAfterDelay(sort2, 0.5f));
        }*/

        /*if (Input.GetKey(KeyCode.Z)) // Animation de quand elle avance
        {
            animator.SetBool("Walk", true);
        } else {
            animator.SetBool("Walk", false);
        }*/

        /*if (Input.GetKeyDown(KeyCode.R)) { // Ulti
            animator.SetTrigger("Ulti");
            ulti.Play(); 
        }*/

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

        var source = GetComponentInParent<MermaidBehaviour>();
        
        var dir = transform.forward;
        currentFish.GetComponent<PoissoinProjBehaviour>().SetDamages(source.GetDegMag(), IDamageable.DamageType.magique);
        currentFish.GetComponent<PoissoinProjBehaviour>().source = source;
        currentFish.GetComponent<PoissoinProjBehaviour>().team = source.team;
        currentFish.GetComponent<Rigidbody>().AddForce(dir.normalized * 30f, ForceMode.Impulse);
        //GetComponentInParent<MermaidBehaviour>().poissoin = currentFish;
        //GetComponentInParent<MermaidBehaviour>().Poissoin(currentFish);
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
        GameObject newVFX = vfx.gameObject;
        if (vfx == sort2)
        {
            GameObject parentVFX = new GameObject("VFXObject");
            parentVFX.transform.position = GetComponentInParent<Transform>().position;
            parentVFX.transform.rotation = GetComponentInParent<Transform>().rotation;
            //Debug.Log(newVFX);
            newVFX = PhotonNetwork.Instantiate(newVFX.name, parentVFX.transform.position, parentVFX.transform.rotation);
            newVFX.transform.parent = parentVFX.transform;

            GameObject wind = PhotonNetwork.Instantiate(windPrefab.name, parentVFX.transform.position, parentVFX.transform.rotation);
            wind.transform.parent = parentVFX.transform;
            
            var source = GetComponentInParent<MermaidBehaviour>();
            
            wind.GetComponent<WindAreaBehaviour>().SetDamages(source.GetDegMag(), IDamageable.DamageType.magique);
            wind.GetComponent<WindAreaBehaviour>().direction = newVFX.transform.forward;
            wind.GetComponent<WindAreaBehaviour>().source = source;
        }

        newVFX.GetComponent<VisualEffect>().SendEvent("OnPlay");

        if (vfx == sort2)
        {
            StartCoroutine(DestroyVFX(newVFX.GetComponent<VisualEffect>(), newVFX.GetComponent<VisualEffect>().GetFloat("TsunamiLifetime")));
        }
        
    }
    
    
    public GameObject Skill1Animation()
    {
        animator.SetTrigger("Auto");
        StartCoroutine(TriggerVFXAfterDelay(sort1, 0.9f));
        StartCoroutine(SpawnFish(0.9f));
        return currentFish;
    }

    public void Skill2Animation()
    {
        animator.SetTrigger("Sort2");
        StartCoroutine(TriggerVFXAfterDelay(sort2, 0.5f));
    }

    public void UltimateAnimation()
    {
        ulti.gameObject.SetActive(true);
        animator.SetTrigger("Ulti");
        var source = GetComponentInParent<MermaidBehaviour>();
        var area = PhotonNetwork.Instantiate(charmePrefab.name, transform.position, Quaternion.identity);
        area.transform.parent = ulti.transform;
        area.GetComponent<CharmAreaBehaviour>().source = source;
        ulti.Play(); 
    }
}
