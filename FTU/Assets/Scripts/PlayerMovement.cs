using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : IDamageable
{
    //Photon
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject localPlayerInstance;
    PhotonView myPV;
    public GameObject Ui;
    //Movement Controlled by players
    Vector3 velocity;
    Rigidbody myRigidbody;
    Camera viewCamera;

    //Movement AI
    public int current;
    public bool pathDone;

    //Animator anim;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        //anim = GetComponent<Animator>();
        myPV = GetComponent<PhotonView>();
        if (myPV.IsMine)
        {
            localPlayerInstance = gameObject;
        }
        if (!myPV.IsMine)
        {
            if (Ui)
            {
                Ui.SetActive(false);
            }
            return;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    public void CameraWork()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (myPV.IsMine)
            {
                _cameraWork.player = gameObject.transform;
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }


    public void MovementPlayer()
    {
        if (GetCanMove())
        {
            // Movement input
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Vector3 moveVelocity = moveInput.normalized * GetMoveSpeed();
            Move(moveVelocity);
            //anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
            //anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
            // Look input
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);

            }
        }
    }

    public void MovementAI(Transform[] moveTo)
    {
        if (GetCanMove() && GetCanAct())
        {
            if (Vector3.Distance(transform.position, moveTo[current].position) > 10)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTo[current].position.x, transform.position.y, moveTo[current].position.z), GetMoveSpeed() * Time.deltaTime);
            }
            else current = (current + 1)/* % targets.Length*/;
        }
        if (current == moveTo.Length) pathDone = true;
    }

    public IEnumerator WalkToward()
    {
        try
        {
            var dist = Vector3.Distance(transform.position, Cible.transform.position);
            //while (transform.position != Cible.transform.position)
            if (dist > gameObject.GetComponent<IDamageable>().GetAttackRange())
            {
                SetIsMoving(true);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z), GetMoveSpeed() * Time.deltaTime);
            }
            SetIsMoving(false);
        }
        catch(NullReferenceException e)
        {
            Cible = null;
        }

        yield return 0;
    }

    public void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);

    }
    void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

     void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
}
