using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ConsAnimation : MonoBehaviour
{
    public Animator animator;
    public VisualEffect sort1; 
    public VisualEffect sort2; 
    public VisualEffect ulti; 

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("Auto");
            StartCoroutine(TriggerVFXAfterDelay(sort1, 0));
        }
        
        if(Input.GetKeyDown(KeyCode.Z)) {
            animator.SetTrigger("Sort2");
            
            StartCoroutine(TriggerVFXAfterDelay(sort2, 0));
        }*/

        /*if (Input.GetKey(KeyCode.Z))
        {
            animator.SetBool("Walk", true);
            StartCoroutine(TriggerVFXAfterDelay(sort2, 0));
        } else {
            animator.SetBool("Walk", false);
        }*/

        /*if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Ulti");
            StartCoroutine(TriggerVFXAfterDelay(ulti, 1.05f));
        }*/
    }

    private IEnumerator TriggerVFXAfterDelay(VisualEffect vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Lancez le VFX
        vfx.SendEvent("OnPlay");
    }
    
    public void Skill1Animation()
    {
        animator.SetTrigger("Auto");
        StartCoroutine(TriggerVFXAfterDelay(sort1, 0));
    }
    
    public void Skill2Animation()
    {
        animator.SetTrigger("Sort2");
            
        StartCoroutine(TriggerVFXAfterDelay(sort2, 0));
    }
    
    public void UltimateAnimation()
    {
        animator.SetTrigger("Ulti");
        StartCoroutine(TriggerVFXAfterDelay(ulti, 1.05f));
    }
}
