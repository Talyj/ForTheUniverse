using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviourPun/*, IPunObservable*/
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
            animator.enabled = true;
            StartCoroutine(DestroyTrap(lifetime));
            float DmgPerHeal;
            float heal = col.gameObject.GetComponent<IDamageable>().GetHealth();
            DmgPerHeal = heal / 4;
            degats = skills.Damage + DmgPerHeal;
            Debug.Log("trap " + degats);
            col.gameObject.GetComponent<IDamageable>().TakeCC(IDamageable.ControlType.stun, 1.25f);
            col.gameObject.GetComponent<IDamageable>().TakeDamage(degats, typeDegats, source.photonView.ViewID);
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

    //void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(teamCode);
    //    }
    //    else
    //    {
    //        teamCode = (byte)stream.ReceiveNext();
    //    }
    //}

}
