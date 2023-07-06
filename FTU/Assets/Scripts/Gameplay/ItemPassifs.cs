using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ItemPassifs : MonoBehaviourPun
{
    public ItemStats[] listItems;

    public void StartPassif(GameObject player, int passif, int isNotConso)
    {
        //Systeme foireux mais nsm à voir plus tard
        switch (passif * isNotConso)
        {
            case 0:
                Debug.Log("No passif");
                break;
            case 1:
                StartCoroutine(ID5GriffeDominion(player));
                break;
            case 300:
                HealthPotion(player);
                break;
            case 301:
                ManaPotion(player);
                break;
            case 302:
                StrangeMushRoom(player);
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

    public void HealthPotion(GameObject gameObject)
    {
        gameObject.GetComponent<IDamageable>().SetHealth(gameObject.GetComponent<IDamageable>().GetHealth() + gameObject.GetComponent<IDamageable>().GetMaxHealth() * 0.1f);
    }

    public void ManaPotion(GameObject gameObject)
    {
        gameObject.GetComponent<IDamageable>().SetMana(gameObject.GetComponent<IDamageable>().GetMana() + gameObject.GetComponent<IDamageable>().GetMaxMana() * 0.1f);
    }

    public void StrangeMushRoom(GameObject gameObject)
    {
        var manaRegen = Random.Range(0.01f, 0.1f);
        var healthRegen = Random.Range(0.01f, 0.1f);

        gameObject.GetComponent<IDamageable>().SetHealth(gameObject.GetComponent<IDamageable>().GetHealth() + gameObject.GetComponent<IDamageable>().GetMaxHealth() * healthRegen);
        gameObject.GetComponent<IDamageable>().SetMana(gameObject.GetComponent<IDamageable>().GetMana() + gameObject.GetComponent<IDamageable>().GetMaxMana() * manaRegen);
    }
}
