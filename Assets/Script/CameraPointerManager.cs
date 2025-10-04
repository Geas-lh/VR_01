using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public Transform racket;               // La raqueta que será el puntero
    public Vector3 racketOffset = new Vector3(0.3f, -0.3f, 0.5f); // Posición relativa respecto a la cámara
    public float disPointerObject = 0.5f;  // Offset antes del punto de impacto
    public float defaultDistance = 2f;     // Distancia fija cuando no apunta a ningún objeto

    void Update()
    {
        // Posición base de la raqueta según la cámara + offset
        Vector3 basePosition = transform.position + transform.TransformDirection(racketOffset);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            // Posición objetivo: cerca del punto de impacto
            racket.position = hit.point - direction * disPointerObject;
        }
        else
        {
            // Si no toca nada, posición base + distancia adelante
            racket.position = basePosition + transform.forward * defaultDistance;
        }

        // La raqueta siempre mira hacia donde apunta la cámara
        racket.rotation = Quaternion.LookRotation(transform.forward);
    }
}
