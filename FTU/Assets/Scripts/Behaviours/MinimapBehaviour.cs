using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MinimapBehaviour : MonoBehaviourPun
{
	// Start is called before the first frame update
	void Start()
    {
        if (photonView.IsMine)
        {
			var minimapCam = GameObject.FindGameObjectWithTag("minimapCam");
            if (minimapCam)
            {
                GetComponent<RawImage>().texture = minimapCam.GetComponent<Camera>().targetTexture;
            }            
        }
    }

	void Update()
	{

	}
}
