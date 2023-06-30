using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttack : MonoBehaviour {
    public GameObject fish; // Le poisson à déplacer
    public Vector3 pointA; // Le point de départ
    public Vector3 pointB; // Le point d'arrivée
    public float speed = 1.0f; // La vitesse de déplacement

    public float erodeRate = 0.03f;
    public float erodeRefreshRate = 0.01f;
    public float erodeDelay = 1.25f;
    public SkinnedMeshRenderer erodeObject;

    private bool hasStartedEroding = false;

    private void Start()
    {
        if (fish != null)
        {
            fish.transform.position = pointA;
        }
    }

    private void Update()
    {
        // Vérifie si le poisson est défini
        if (fish != null)
        {
            // Déplace le poisson de pointA vers pointB
            fish.transform.position = Vector3.MoveTowards(fish.transform.position, pointB, speed * Time.deltaTime);

            // Fait tourner le poisson pour qu'il regarde vers pointB
            Vector3 direction = (pointB - fish.transform.position).normalized;
            if (direction != Vector3.zero)
            {
                fish.transform.rotation = Quaternion.LookRotation(direction);
            }

            // Si le poisson a atteint le point B, le détruit
            if (fish.transform.position == pointB)
            {
                Destroy(fish);
            }

            // Si le temps pour atteindre le point B est inférieur à erodeDelay et n'a pas commencé à éroder, commence à éroder
            float remainingDistance = Vector3.Distance(fish.transform.position, pointB);
            float remainingTime = remainingDistance / speed;
            if (!hasStartedEroding && remainingTime <= erodeDelay)
            {
                hasStartedEroding = true;
                StartCoroutine(ErodeObject());
            }
        }
    }

    IEnumerator ErodeObject()
    {
        float t = 0;
        while (t < 1)
        {
            t += erodeRate;
            erodeObject.material.SetFloat("_Erode", t);
            yield return new WaitForSeconds(erodeRefreshRate);
        }
    }
}
