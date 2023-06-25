using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

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

    NavMeshAgent _navMeshAgent;

    //Animator anim;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
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
            //Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //Vector3 moveVelocity = moveInput.normalized * GetMoveSpeed();
            //Move(moveVelocity);
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
                if (Input.GetMouseButtonDown(1))
                {
                    _navMeshAgent.SetDestination(point);
                }
                //if (_navMeshAgent.remainingDistance <= 2f)
                //{
                //    _navMeshAgent.ResetPath();
                //}
            }
        }
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
