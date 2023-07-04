using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : IDamageable
{
    //Photon
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject localPlayerInstance;
    PhotonView myPV;
    public GameObject Ui;
    public GameObject animation_click;
    //Movement Controlled by players
    Vector3 velocity;
    Rigidbody myRigidbody;
    Camera viewCamera;
    
    //Animator
    public Animator animator;
    Camera minimapCamera;

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
        if(minimapCamera == null)
        {
            minimapCamera = GameObject.FindGameObjectWithTag("minimapCam") ? GameObject.FindGameObjectWithTag("minimapCam").GetComponent<Camera>() : null;
        }
        if (GetCanMove())
        {
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Ray minimapRay = new Ray();
            if (minimapCamera != null)
            {
                minimapRay = minimapCamera.ScreenPointToRay(Input.mousePosition);
                //Debug.DrawRay(minimapRay.origin, minimapRay.direction * 500000);
            }
            //Debug.DrawRay(ray.origin, ray.direction * 500000);


            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                if (_navMeshAgent.remainingDistance >= 2f)
                {
                    LookAt(_navMeshAgent.destination);
                }
                else if (Cible)
                {
                    LookAt(Cible.transform.position);
                }
                else
                {
                    LookAt(point);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    //if(minimapCamera != null)
                    //{
                    //    if(groundPlane.Raycast(minimapRay, out rayDistance))
                    //    {
                    //        point = minimapRay.GetPoint(rayDistance);
                    //    }
                    //}
                    _navMeshAgent.ResetPath();
                    _navMeshAgent.SetDestination(point);
                    Instantiate(animation_click, new Vector3(point.x, 0.2f, point.z), Quaternion.identity);
                }
            }

            var stateId = Animator.StringToHash("Walk");
            if (animator.HasState(0,stateId))
            {
                
            }
            if (Vector3.Distance(_navMeshAgent.destination, transform.position) < 0.5f)
            {
                animator.SetBool("Walk", false);
            }
            else
            {
                animator.SetBool("Walk", true);
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
