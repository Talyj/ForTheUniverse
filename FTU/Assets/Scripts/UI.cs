using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    PlayerStats stats;
    [SerializeField]
    Image[] spells;
    [SerializeField]
    Image health, mana;
    [SerializeField]
    Text[] costs;
    // Start is called before the first frame update
    void Start()
    {
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
            if(spells[0].fillAmount <= 0)
            {
                spells[0].fillAmount = 1;
            }
        }
        if (stats.GetSkill2().isCooldown)
        {
            spells[1].fillAmount -= 1 / stats.GetSkill2().Cooldown * Time.deltaTime;//cd
            if (spells[1].fillAmount <= 0)
            {
                spells[1].fillAmount = 1;
            }
            if (stats.GetUlt().isCooldown)
            {
                spells[2].fillAmount -= 1 / stats.GetUlt().Cooldown * Time.deltaTime;//cd
                if (spells[2].fillAmount <= 0)
                {
                    spells[2].fillAmount = 1;
                }
            }
        }

    }
}
