using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //[SerializeField]
    PlayerStats stats;
    [SerializeField]
    Image[] spells;
    [SerializeField]
    Image[] cd;
    [SerializeField]
    Image health, mana;
    [SerializeField]
    Image healthCible, manaCible;
    [SerializeField]
    TMP_Text[] costsCible;
    [SerializeField]
    TMP_Text[] costs;
    [SerializeField]
    GameObject statsPanel;

    [SerializeField]
    private TMP_Text levelText, healthText, manaText;

    [SerializeField] private TMP_Text[] statsTexts;

    [SerializeField]
    GameObject cibleHp, ciblePm;

    // Start is called before the first frame update
    void Start()
    {
        stats = gameObject.GetComponentInParent<PlayerStats>();
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
        if( stats.Cible != null)
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
            /*cibleHp.SetActive(false);
            ciblePm.SetActive(false);
            costsCible[0].enabled = false;
            costsCible[1].enabled = false;*/
        }
        //stats a modfier
        statsTexts[4].text = stats.GetResMag().ToString();
        statsTexts[3].text = stats.GetResPhys().ToString();
        statsTexts[1].text = stats.GetDegMag().ToString();
        statsTexts[0].text = stats.GetDegPhys().ToString();
        statsTexts[2].text = stats.GetMoveSpeed().ToString();
        //costs[6].text += "Gold : " + stats.gold + "\n";

        levelText.text = stats.GetLvl().ToString();
        float percentHP = ((stats.GetHealth() * 100) / stats.GetMaxHealth()) / 100;
        health.fillAmount = percentHP;
        healthText.text = stats.GetHealth() + " / " + stats.GetMaxHealth();
        float percentMana = ((stats.GetMana() * 100) / stats.GetMaxMana()) / 100;
        mana.fillAmount = percentMana;
        manaText.text = stats.GetMana() + " / " + stats.GetMaxMana();

        if (stats.GetSkill1().isCooldown)
        {
            cd[0].fillAmount -= 1 / stats.GetSkill1().Cooldown * Time.deltaTime;//cd
            if(cd[0].fillAmount <= 0)
            {
                cd[0].fillAmount = 10f;
            }
        }
        if (stats.GetSkill2().isCooldown)
        {
            cd[1].fillAmount -= 1 / stats.GetSkill2().Cooldown * Time.deltaTime;//cd
            if (cd[1].fillAmount <= 0)
            {
                cd[1].fillAmount = 1f;
            }
            
        }
        if (stats.GetUlt().isCooldown)
        {
            cd[2].fillAmount -= 1 / stats.GetUlt().Cooldown * Time.deltaTime;//cd
            if (cd[2].fillAmount <= 0)
            {
                cd[2].fillAmount = 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            statsPanel.SetActive(true);
        }
        
        if (Input.GetKeyUp(KeyCode.C))
        {
            statsPanel.SetActive(false);
        }

    }
}
