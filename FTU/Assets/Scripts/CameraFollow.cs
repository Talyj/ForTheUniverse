using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;//faire en sort que ce soit le joueur que l'on controlle
    float camOffSetZ;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;
        camOffSetZ = gameObject.transform.position.z - player.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _cam = new Vector3(player.position.x, gameObject.transform.position.y, player.position.z + camOffSetZ);

        gameObject.transform.position = _cam;
    }
}
