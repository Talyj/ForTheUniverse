using UnityEngine;


public class CameraWork : MonoBehaviour
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
    Transform cameraTransform;
    bool isFollowing;
    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;

    public void Start()
    {
        var invisible_layer_mask=LayerMask.NameToLayer("InvisibleDominion");
        invisible_layer_mask=~ (1 <<invisible_layer_mask);//This inverts the value
        Debug.Log(invisible_layer_mask);
        Camera.main.cullingMask= invisible_layer_mask;
        
        // Start following the target if wanted.
        if (followOnStart)
        {
            OnStartFollowing();
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


        // only follow is explicitly declared
        if (isFollowing)
        {
            Follow();
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
