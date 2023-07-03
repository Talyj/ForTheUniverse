using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BigEdAnimation : MonoBehaviour {
    public VisualEffect sort1;
    public VisualEffect shield;
    public VisualEffect ulti;

    
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) { 
             StartCoroutine(TriggerVFXAfterDelay(sort1, 0.0f));
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(TriggerVFXAfterDelay(ulti, 0.0f));
        }

        if (Input.GetKeyDown(KeyCode.E)) { 
             StartCoroutine(TriggerVFXAfterDelay(shield, 0.0f));
         }
    }

    private IEnumerator TriggerVFXAfterDelay(VisualEffect vfx, float delay) {
        yield return new WaitForSeconds(delay);
        vfx.SendEvent("OnPlay");
    }
}
