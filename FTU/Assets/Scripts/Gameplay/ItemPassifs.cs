using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ItemPassifs : MonoBehaviourPun
{
    public ItemStats[] listItems;

    public void StartPassif(GameObject player, int passif)
    {
        switch (passif)
        {
            case 0:
                Debug.Log("No passif");
                break;
            case 1:
                StartCoroutine(ID5GriffeDominion(player));
                break;

        }
    }

    public IEnumerator ID5GriffeDominion(GameObject gameObject)
    {
        while (gameObject.GetComponent<IDamageable>().GetHealth() > 0)
        {
            Debug.LogError("GriffeDominionEffect");
            yield return new WaitForSeconds(1f);
        }
        yield return 0;
    }
}
