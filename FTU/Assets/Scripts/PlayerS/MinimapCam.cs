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

		if (Input.GetMouseButtonDown(1) && IspointerOverUiObject())
		{


			Vector3 mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
			Vector3 worldPos = new Vector3(mousePos.x * mapWidth, 0, mousePos.y * mapHeight);

			// Adjust for map proportions
			worldPos.x *= (mapWidth / (float)Screen.width);
			worldPos.z *= (mapHeight / (float)Screen.height);


			//character.transform.position = worldPos;
			Debug.Log(worldPos);


			//if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
			//{

			//    //your function move a selected unit to clicked area, etc.
			//    Debug.LogError("ça bouge");
			//}
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
