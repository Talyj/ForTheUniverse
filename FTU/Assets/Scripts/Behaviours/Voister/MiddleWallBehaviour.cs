using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleWallBehaviour : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        gameObject.transform.LookAt(new Vector3(0, 0, 0));
        gameObject.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, 20 * Time.deltaTime);
    }
}
