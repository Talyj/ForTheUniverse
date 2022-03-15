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
    Text[] costs;
    // Start is called before the first frame update
    void Start()
    {
        spells[0].sprite = stats.Skills[0].image;
        spells[1].sprite = stats.Skills[1].image;
        costs[0].text = stats.Skills[0].Cost.ToString();
        costs[1].text = stats.Skills[1].Cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.Skills[0].isCooldown)
        {
            spells[0].fillAmount -= 1 / stats.Skills[0].Cooldown * Time.deltaTime;//cd
            if(spells[0].fillAmount <= 0)
            {
                spells[0].fillAmount = 1;
            }
        }
        if (stats.Skills[1].isCooldown)
        {
            spells[1].fillAmount -= 1 / stats.Skills[1].Cooldown * Time.deltaTime;//cd
            if (spells[1].fillAmount <= 0)
            {
                spells[1].fillAmount = 1;
            }
        }

    }
}
