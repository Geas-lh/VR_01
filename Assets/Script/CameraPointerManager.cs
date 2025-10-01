using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public Transform pointer;              // La raqueta como puntero
    public float disPointerObject = 0.5f;  // Qué tan lejos del punto de impacto colocar el puntero
    public string targetTag = "Interactable"; // El tag que debe tener el objeto para activar el puntero

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag(targetTag))
            {
                pointer.gameObject.SetActive(true);
                PointerOnGaze(hit.point);
            }
            else
            {
                pointer.gameObject.SetActive(false);
            }
        }
        else
        {
            pointer.gameObject.SetActive(false);
        }
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        pointer.transform.position = CalculatePointerPosition(transform.position, hitPoint, disPointerObject);
        // No cambia la escala: se mantiene constante
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        return Vector3.Lerp(p0, p1, t);
    }
}
