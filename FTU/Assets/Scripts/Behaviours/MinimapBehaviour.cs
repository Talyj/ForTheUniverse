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

	private float offsetx;
	private float offsety;

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
		if (!photonView.IsMine) {
			return;
		}

		if (IspointerOverUiObject()) {
			
			if (IspointerOverUiObject() && Input.GetMouseButtonDown(0)) {
				Vector2 localCursor;

				// Using TryRebuildCanvasRenderMode just in case something changes the RenderMode on the canvas at runtime.
				if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, rawImage.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam, out localCursor))
					return;
			
				Rect r = rawImage.rectTransform.rect;

				if (r.Contains(localCursor)) {
					if(localCursor.x > 0.0f) {
						offsetx = 20.0f;
						offsety = -1.0f;
					} else {
						offsetx = -20.0f;
						offsety = 1.0f;
					}

					if(localCursor.x < 20.0f && localCursor.x > -20.0f) {
						offsetx = 0.0f;
						offsety = 0.0f;
					}

					player._navMeshAgent.ResetPath();
        			player._navMeshAgent.SetDestination(new Vector3(localCursor.x+offsetx, 0, localCursor.y+offsety));
				}
			}
		}
	}

	private bool IspointerOverUiObject() {
		PointerEventData EventDataCurrentPosition = new PointerEventData(EventSystem.current);
		EventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(EventDataCurrentPosition, result);
		return result.Count > 0;
	}
}
