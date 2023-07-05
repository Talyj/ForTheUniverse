using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapCam : MonoBehaviour
{

	//attach this script to the camera

	[SerializeField]
	Camera cam; //for raycast instead of using Camera.main
	[SerializeField]
	GameObject camToMove; // the gameobject the camera is attached to


	RaycastHit hit;
	Ray ray;
	[SerializeField]
	LayerMask mask;//set to your ground layer you want the raycast to hit

	Vector3 movePoint;
	float YPos;
	[SerializeField]
	float offset;

	float mapWidth, mapHeight;


	void Start()
	{
		if (cam == null)
		{
			cam = GetComponent<Camera>();
		}
		mapHeight = 340;
		mapWidth = 700;
	}


	void Update() {
		
	}


	//this function dectects clicks on ui objects
	private bool IspointerOverUiObject()
	{
		PointerEventData EventDataCurrentPosition = new PointerEventData(EventSystem.current);
		EventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(EventDataCurrentPosition, result);
		return result.Count > 0;

	}
	
}
