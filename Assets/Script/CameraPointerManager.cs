using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public Transform pointer;          // Objeto que se moverá y escalará (debe asignarse en el Inspector)
    public float scaleSize = 1f;       // Tamaño base para el escalado
    public float disPointerObject = 0.5f; // Factor de interpolación entre la cámara y el hit point

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            PointerOnGaze(hit.point);
        }
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);
        pointer.transform.localScale = Vector3.one * scaleFactor;
        pointer.transform.position = CalculatePointerPosition(transform.position, hitPoint, disPointerObject);
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x + t * (p1.x - p0.x);
        float y = p0.y + t * (p1.y - p0.y);
        float z = p0.z + t * (p1.z - p0.z);

        return new Vector3(x, y, z);
    }
}
