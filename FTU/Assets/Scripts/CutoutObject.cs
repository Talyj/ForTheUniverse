using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    
    public Transform targetObject;
    [SerializeField]
    LayerMask fogLayer;

    Camera mainCam;
    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCam.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, fogLayer);
        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].SetVector("_CutOut_position", cutoutPos);
                materials[j].SetFloat("_CutOut_Size", .1f);
                materials[j].SetFloat("_Falloff_Size", .05f);
            }
        }
    }
}
