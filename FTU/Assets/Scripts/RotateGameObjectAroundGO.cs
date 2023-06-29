using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObjectAroundGO : MonoBehaviour
{
    public Transform objectToRotate;
    [Range(0.0f, 20.0f)]
    public float distance;
    private float lastDistance;
    private Transform center;

    // Start is called before the first frame update
    void Start()
    {
        center = this.transform;
        lastDistance = distance;
        objectToRotate.position = new Vector3(distance, 0, 0) + center.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(distance - lastDistance) > 0.00001f)
        {
            objectToRotate.position = new Vector3(distance, 0, 0) + center.position;
        }
        objectToRotate.RotateAround(center.position, Vector3.up, 1f );
        lastDistance = distance;
    }
}
