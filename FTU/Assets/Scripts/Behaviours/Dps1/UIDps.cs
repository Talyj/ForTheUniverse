using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDps : MonoBehaviour
{
    [SerializeField]
    Dps1 stats;
    [SerializeField]
    Image[] spells;
    [SerializeField]
    Image health, mana;
    [SerializeField]
    Image healthCible, manaCible;
    [SerializeField]
    Text[] costsCible;
    [SerializeField]
    Text[] costs;

    [SerializeField]
    GameObject cibleHp, ciblePm;

    // Start is called before the first frame update
    void Start()
    {
        stats = gameObject.GetComponent<Dps1>();
        BaseStats();
    }

    void BaseStats()
    {
        spells[0].sprite = stats.GetSkill1().image;
        spells[1].sprite = stats.GetSkill2().image;
        spells[2].sprite = stats.GetUlt().image;
        costs[0].text = stats.GetSkill1().Cost.ToString();
        costs[1].text = stats.GetSkill2().Cost.ToString();
        costs[2].text = stats.GetUlt().Cost.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (stats.Cible != null)
        {
            cibleHp.SetActive(true);
            ciblePm.SetActive(true);
            costsCible[0].enabled = true;
            costsCible[1].enabled = true;
            float percentHPCible = ((stats.Cible.GetComponent<IDamageable>().GetHealth() * 100) / stats.Cible.GetComponent<IDamageable>().GetMaxHealth()) / 100;
            healthCible.fillAmount = percentHPCible;
            costsCible[0].text = stats.Cible.GetComponent<IDamageable>().GetHealth() + " / " + stats.Cible.GetComponent<IDamageable>().GetMaxHealth();
            float percentManaCible = ((stats.Cible.GetComponent<IDamageable>().GetMana() * 100) / stats.Cible.GetComponent<IDamageable>().GetMaxMana()) / 100;
            manaCible.fillAmount = percentManaCible;
            costsCible[1].text = stats.Cible.GetComponent<IDamageable>().GetMana() + " / " + stats.Cible.GetComponent<IDamageable>().GetMaxMana();
        }
        else
        {
            cibleHp.SetActive(false);
            ciblePm.SetActive(false);
            costsCible[0].enabled = false;
            costsCible[1].enabled = false;
        }
        //stats a modfier
        costs[6].text = "RM : " + stats.GetRM() + "\n";
        costs[6].text += "Armor : " + stats.GetArmor() + "\n";
        costs[6].text += "AP : " + stats.GetAP() + "\n";
        costs[6].text += "AD : " + stats.GetAD() + "\n";
        costs[6].text += "MS : " + stats.GetMoveSpeed() + "\n";

        costs[5].text = "Niveau : " + stats.GetLvl();
        float percentHP = ((stats.GetHealth() * 100) / stats.GetMaxHealth()) / 100;
        health.fillAmount = percentHP;
        costs[3].text = stats.GetHealth() + " / " + stats.GetMaxHealth();
        float percentMana = ((stats.GetMana() * 100) / stats.GetMaxMana()) / 100;
        mana.fillAmount = percentMana;
        costs[4].text = stats.GetMana() + " / " + stats.GetMaxMana();

        if (stats.GetSkill1().isCooldown)
        {
            spells[0].fillAmount -= 1 / stats.GetSkill1().Cooldown * Time.deltaTime;//cd
            if (spells[0].fillAmount <= 0)
            {
                spells[0].fillAmount = 1f;
            }
        }
        if (stats.GetSkill2().isCooldown)
        {
            spells[1].fillAmount -= 1 / stats.GetSkill2().Cooldown * Time.deltaTime;//cd
            if (spells[1].fillAmount <= 0)
            {
                spells[1].fillAmount = 1f;
            }

        }
        if (stats.GetUlt().isCooldown)
        {
            spells[2].fillAmount -= 1 / stats.GetUlt().Cooldown * Time.deltaTime;//cd
            if (spells[2].fillAmount <= 0)
            {
                spells[2].fillAmount = 1f;
            }
        }

    }
}
