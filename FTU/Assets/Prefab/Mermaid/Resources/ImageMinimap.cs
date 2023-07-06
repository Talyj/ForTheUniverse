using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMinimap : MonoBehaviour
{
    public Quaternion initialRotation;

    void Update() 
    {
        transform.rotation = initialRotation;
    }
}
