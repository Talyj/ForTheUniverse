using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MouseClicked : MonoBehaviour
{
    public VisualEffect ve;
    // Start is called before the first frame update
    void Start() {
        ve.SendEvent("OnPlay");
        StartCoroutine(DestroyAfterPlay(0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private IEnumerator DestroyAfterPlay(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
