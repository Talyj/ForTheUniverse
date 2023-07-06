using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryAnimationScript : MonoBehaviour
{
    public GameObject victoryText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayVictoryText()
    {
        victoryText.SetActive(true);
    }

    private void OnEnable()
    {
        GetComponent<Animation>().Play();
    }
}
