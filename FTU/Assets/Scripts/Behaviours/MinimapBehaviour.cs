using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MinimapBehaviour : MonoBehaviourPun
{
	Camera cam;
	GameObject camToMove;
	RaycastHit hit;
	Ray ray;
	RawImage rawImage;
	[SerializeField] LayerMask mask;//set to your ground layer you want the raycast to hit

	Vector3 movePoint;
	float YPos;
	[SerializeField]
	float offset;

	[Range(-50f, 50f)]
	public float valueX;

	[Range(-50f, 50f)]
	public float valueY;

	[Range(-50f, 50f)]
	public float valueZ;

	PlayerMovement player;

	// Start is called before the first frame update
	void Start()
    {
        if (photonView.IsMine)
        {
			valueX = 1f;
			valueY = 1f;
			valueZ = 1f;
			var minimapCam = GameObject.FindGameObjectWithTag("minimapCam");
            if (minimapCam)
            {
                GetComponent<RawImage>().texture = minimapCam.GetComponent<Camera>().targetTexture;
				rawImage = GetComponent<RawImage>();
				var cameras = FindObjectsOfType<Camera>().Where(x => x.CompareTag("minimapCam"));
				if(cameras.Count() > 0)
                {
					cam = cameras.First();
                }
            }
			player = gameObject.GetComponentInParent<PlayerMovement>();
		}
    }

	void Update()
	{
		//to move camera :
		//if (IspointerOverUiObject())
		//{
		//	if (Input.GetMouseButton(0))
		//	{


		//		ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);


		//		if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
		//		{
		//			YPos = camToMove.transform.position.y;

		//			movePoint = new Vector3(hit.point.x, YPos, hit.point.z - offset);
		//			camToMove.transform.position = movePoint;

		//		}
		//	}
		//}

		if (IspointerOverUiObject())
		{
			//ray = cam.ScreenPointToRay(Input.mousePosition);

			//Debug.DrawRay(ray.origin, (ray.direction /** new Vector2(1, 1)*/) * 4500);

			//         ray.origin = new Vector3(ray.origin.x - 1000, ray.origin.y, ray.origin.z + 80);
			//         Debug.LogError(ray.origin);
			//ray.direction  = new Vector3(ray.direction.x * valueX, ray.direction.y * valueY, ray.direction.z * valueZ);

			//Debug.DrawRay(ray.origin, ray.direction * 500000, Color.red);

			//if (Input.GetMouseButtonDown(0))
			//{

			//	float rayDistance;
			//	Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

			//	if (groundPlane.Raycast(ray, out rayDistance))
			//             {
			//		var point = ray.GetPoint(rayDistance);
			//                 player._navMeshAgent.ResetPath();
			//                 player._navMeshAgent.SetDestination(point);
			//             }
			//             //if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
			//             //{
			//             //	//your function move a selected unit to clicked area, etc.
			//             //}
			//         }

			Vector2 curosr = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			//if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RawImage>().rectTransform, new Vector2(Input.mousePosition.x, Input.mousePosition.y), cam, out curosr))
			//{

				Texture texture = GetComponent<RawImage>().texture;
				Rect rect = GetComponent<RawImage>().rectTransform.rect;

				float coordX = Mathf.Clamp(0, (((curosr.x - rect.x) * texture.width) / rect.width), texture.width);
				float coordY = Mathf.Clamp(0, (((curosr.y - rect.y) * texture.height) / rect.height), texture.height);

				float calX = coordX / texture.width;
				float calY = coordY / texture.height;


				curosr = new Vector2(calX, calY);

				CastRayToWorld(curosr);
			//}
		}
	}

	private void CastRayToWorld(Vector2 vec)
	{
        Ray MapRay = cam.ScreenPointToRay(new Vector3(vec.x * cam.pixelWidth, 0, vec.y * cam.pixelHeight));
        //Ray MapRay = cam.ScreenPointToRay(new Vector3(vec.x * cam.pixelWidth, vec.y * cam.pixelHeight));

        RaycastHit miniMapHit;
		Debug.DrawRay(MapRay.origin, MapRay.direction * 5000);
		if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity, mask))
		{
			Debug.Log("miniMapHit: " + miniMapHit.collider.gameObject);
			Debug.LogError(miniMapHit.point);
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
