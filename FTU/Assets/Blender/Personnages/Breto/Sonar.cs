using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour {

    public float temps_sonar;
    public float taille = 5f;

    private Material material;
    private Color originalColor;

    void Start() {
        transform.localScale = new Vector3(0, 0, 0);
    }

    private void Update() {
        transform.localScale += new Vector3(1, 1, 1) * (temps_sonar/5) * Time.deltaTime;

        if(transform.localScale.x >= taille) {
            Destroy(gameObject);
        }
    }
}
