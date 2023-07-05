using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPassifs : MonoBehaviourPun
{
    public static void StartPassif(GameObject player, int passif)
    {
        switch (passif)
        {
            case 0:
                Debug.Log("No passif");
                break;
            case 1:
                //StartCoroutine(ID5GriffeDominion(player));
                break;

        }
    }

    public static IEnumerator ID5GriffeDominion(GameObject gameObject)
    {
        var stack = 0;
        while(gameObject.GetComponent<IDamageable>().GetHealth() > 0)
        {
            Debug.LogError("GriffeDominionEffect");
            yield return new WaitForSeconds(1f);
        }
        yield return 0;
    }
}
