using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;


public class CameraWork : MonoBehaviourPun
{

    [Tooltip("The distance in the local x-z plane to the target")]
    private float distance = 35.0f;


    [Tooltip("The height we want the camera to be above the target")]
    private float height = 30.0f;


    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField] private Vector3 centerOffset = Vector3.zero;


    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField] private bool followOnStart = false;

    public Transform player;

    // cached transform of the target
    [SerializeField] public Transform cameraTransform;
    [SerializeField] public bool isFollowing;
    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;
    public float zoomLevel;
    float zoomPosition;


    public void Start()
    {
        if (photonView.IsMine)
        {
            //Mask for main Camera
            var team = gameObject.GetComponent<IDamageable>().team.Code;
            var layer = team == 1 ? "InvisibleDominion" : "InvisibleVeritas";
        
            var invisible_layer_mask=LayerMask.NameToLayer(layer);
            //var final_layer_mask=~ (1 <<invisible_layer_mask);//This inverts the value

            var minimap_layer_mask = LayerMask.NameToLayer("MinimapCamera");
            var final_layer_mask = ~(1 << invisible_layer_mask | 1 << minimap_layer_mask);
            //Debug.Log(invisible_layer_mask);
            Camera.main.cullingMask= final_layer_mask;

            //Mask for minimap Camera
            GameObject.FindGameObjectWithTag("minimapCam").GetComponent<Camera>().cullingMask = ~(1 << invisible_layer_mask);

            // Start following the target if wanted.
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }
        
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isFollowing = !isFollowing; 
            }
        }
    }


    public void LateUpdate()
    {
        // The transform target may not destroy on level load,
        // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }

        if (cameraTransform)
        {
            // only follow is explicitly declared
            if (isFollowing)
            {
                Follow();
            }
            else
            {
                MoveCamera();
                Zoom();
            }
        }

    }

    public void MoveCamera()
    {
        float moveAmount = 100f;
        float edgeSize = 30f;
        Vector3 vectorToGo = new Vector3(0, 0, 0);

        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            vectorToGo.x += moveAmount * Time.deltaTime;
        }
        if (Input.mousePosition.x < edgeSize)
        {
            vectorToGo.x -= moveAmount * Time.deltaTime;
        }
         
        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            vectorToGo.z += moveAmount * Time.deltaTime;
        }
        if (Input.mousePosition.y < edgeSize)
        {
            vectorToGo.z -= moveAmount * Time.deltaTime;
        }
        cameraTransform.position += vectorToGo * 50 * Time.deltaTime;
    }

    public void Zoom()
    {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        if (ScrollWheelChange != 0)
        {                                            
            float R = ScrollWheelChange * 15;
            float PosX = cameraTransform.eulerAngles.x + 90;
            float PosY = -1 * (cameraTransform.eulerAngles.y - 90);
            PosX = PosX / 180 * Mathf.PI;
            PosY = PosY / 180 * Mathf.PI;
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);
            float Y = R * Mathf.Cos(PosX);
            float CamX = cameraTransform.position.x;
            float CamY = cameraTransform.position.y;
            float CamZ = cameraTransform.position.z;
            cameraTransform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z);
        }
    }

    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        // we don't smooth anything, we go straight to the right camera shot
        Cut();
    }

    void Follow()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;


        cameraTransform.position = new Vector3(player.position.x, player.position.y + cameraOffset.y, player.position.z + cameraOffset.z);


        cameraTransform.LookAt(player.position + centerOffset);
    }


    void Cut()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;


        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);


        cameraTransform.LookAt(this.transform.position + centerOffset);
    }
}
