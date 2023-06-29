using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Deplacement : MonoBehaviour
{
    public Animator animator;
    public VisualEffect sort1; 
    public VisualEffect sort2; 
    public VisualEffect ulti; 
    public string animationTriggerName = "Ulti";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(TriggerVFXAfterDelay(sort1, 0));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(TriggerVFXAfterDelay(sort2, 0));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger(animationTriggerName);

            StartCoroutine(TriggerVFXAfterDelay(ulti, 1.05f));
        }
    }

    private IEnumerator TriggerVFXAfterDelay(VisualEffect vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Lancez le VFX
        vfx.SendEvent("OnPlay");
    }
}
