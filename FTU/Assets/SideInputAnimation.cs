using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideInputAnimation : MonoBehaviour
{
    public GameObject[] sidesInput;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation()
    {
        
        foreach (var animation in sidesInput)
        {
            animation.GetComponent<Animation>().Play();
        }
    }
}
