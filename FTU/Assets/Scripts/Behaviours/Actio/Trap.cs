using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviourPun
{
    public ActioBehaviour source;
    public IDamageable.DamageType typeDegats;
    public float degats;
    public Skills skills;
    public Animator animator;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator DestroyTrap(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (CanTarget(col.gameObject))
        {
            var colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (var collider in colliders)
            {
                if (CanTarget(collider.gameObject))
                {
                    animator.enabled = true;
                    StartCoroutine(DestroyTrap(lifetime));
                    float health = collider.GetComponent<IDamageable>().GetHealth();
                    float DmgPerHeal = health / 4;
                    degats = skills.Damage + DmgPerHeal;
                    Debug.Log("trap " + degats);
                    collider.GetComponent<IDamageable>().TakeDamage(degats, typeDegats);
                }
            }
        }
    }

    private bool CanTarget(GameObject target)
    {
        if (target.GetComponent<IDamageable>())
        {
            if (target.GetComponent<IDamageable>().team.Code != source.team.Code)
            {
                return true;
            }
        }
        return false;
    }

}
