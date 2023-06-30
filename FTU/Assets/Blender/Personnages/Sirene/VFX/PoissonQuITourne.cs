using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonQuITourne : MonoBehaviour
{
    public float speed = 1f; // La vitesse de rotation
    public float radius = 5f; // Le rayon de l'arc de cercle
    private Vector3 _center; // Le point autour duquel l'objet va tourner
    private float _angle; // L'angle actuel de l'objet sur l'arc de cercle

    private void Start()
    {
        // Prend la position actuelle comme centre de rotation
        _center = transform.position;
    }

    private void Update()
    {
        // Augmente l'angle en fonction de la vitesse et du temps écoulé depuis la dernière frame
        _angle += speed * Time.deltaTime;

        // Calcule la nouvelle position de l'objet
        var offset = new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle)) * radius;
        transform.position = _center + offset;

        // Fait en sorte que l'objet "regarde" vers le centre
        transform.LookAt(_center);
    }
}
