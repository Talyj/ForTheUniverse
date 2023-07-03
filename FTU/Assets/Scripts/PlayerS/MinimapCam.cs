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



	void Start()
	{
		if (cam == null)
		{
			cam = GetComponent<Camera>();
		}
	}


	void Update()
	{
		//to move camera :
		if (IspointerOverUiObject())
		{
			if (Input.GetMouseButton(0))
			{


				ray = cam.ScreenPointToRay(Input.mousePosition);


				if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
				{
					YPos = camToMove.transform.position.y;

					movePoint = new Vector3(hit.point.x, YPos, hit.point.z - offset);
					camToMove.transform.position = movePoint;

				}
			}
		}

		if (IspointerOverUiObject())
		{
			ray = cam.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin, ray.direction * 500);
			if (Input.GetMouseButtonDown(1))
			{




				if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
				{

					//your function move a selected unit to clicked area, etc.
					Debug.LogError("ça bouge");
				}
			}
		}

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
