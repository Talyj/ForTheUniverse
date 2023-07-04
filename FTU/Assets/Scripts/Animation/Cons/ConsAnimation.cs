using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static IDamageable;

public class ConsAnimation : MonoBehaviour
{
    public Animator animator;
    public VisualEffect sort1; 
    public VisualEffect sort2; 
    public VisualEffect ulti;
    public GameObject projStart;
    public GameObject beam;
    private ConsBehaviour source;

    private void Start()
    {
        source = GetComponentInParent<ConsBehaviour>();
    }
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
        var proj = PhotonNetwork.Instantiate(beam.name, projStart.transform.position, Quaternion.identity);
        var dir = new Vector3(projStart.transform.position.x, 0 ,projStart.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        proj.GetComponent<ProjCons>().SetDamages(source.GetDegPhys(), DamageType.physique);
        proj.GetComponent<ProjCons>().source = source;
        proj.GetComponent<ProjCons>().team.Code = source.team.Code;
        proj.GetComponent<Rigidbody>().AddForce(dir.normalized * 30f, ForceMode.Impulse);
    }
    
    public void Skill2Animation()
    {
        animator.SetTrigger("Sort2");
        StartCoroutine(TriggerVFXAfterDelay(sort2, 0));
        StartCoroutine(skill2());
    }

    IEnumerator skill2()
    {
        switch (source._passiveCounter)
        {
            case 1:
            case 2:
                source.SetDegPhys(source.GetDegPhys() * 1.1f);
                source.SetAttackSpeed(source.GetAttackSpeed() / 1.1f);
                yield return new WaitForSeconds(source.skills[1].CastTime * 2);
                source.SetDegPhys(source.GetDegPhys() / 1.1f);
                source.SetAttackSpeed(source.GetAttackSpeed() * 1.1f);
                break;
            case 3:
            case 4:
            case 5:
                source.SetDegPhys(source.GetDegPhys() * 1.15f);
                source.SetAttackSpeed(source.GetAttackSpeed() / 1.15f);
                yield return new WaitForSeconds(source.skills[1].CastTime * 2);
                source.SetDegPhys(source.GetDegPhys() / 1.15f);
                source.SetAttackSpeed(source.GetAttackSpeed() * 1.15f);
                break;
            case 6:
            case 7:
            case 8:
                source.SetDegPhys(source.GetDegPhys() * 1.2f);
                source.SetAttackSpeed(source.GetAttackSpeed() / 1.2f);
                yield return new WaitForSeconds(source.skills[1].CastTime * 2);
                source.SetDegPhys(source.GetDegPhys() / 1.2f);
                source.SetAttackSpeed(source.GetAttackSpeed() * 1.2f);
                break;
            case 9:
                source.SetDegPhys(source.GetDegPhys() * 1.8f);
                source.SetAttackSpeed(source.GetAttackSpeed() / 1.8f);
                yield return new WaitForSeconds(source.skills[1].CastTime * 2);
                source.SetDegPhys(source.GetDegPhys() / 1.8f);
                source.SetAttackSpeed(source.GetAttackSpeed() * 1.8f);
                break;
            default:
                source.SetDegPhys(source.GetDegPhys() * 2f);
                source.SetAttackSpeed(source.GetAttackSpeed() / 2f);
                yield return new WaitForSeconds(source.skills[1].CastTime * 2);
                source.SetDegPhys(source.GetDegPhys() / 2f);
                source.SetAttackSpeed(source.GetAttackSpeed() * 2f);
                break;
        }
        source._passiveCounter = 0;
    }

    public void UltimateAnimation()
    {
        animator.SetTrigger("Ulti");
        StartCoroutine(TriggerVFXAfterDelay(ulti, 1.05f));
    }
}
