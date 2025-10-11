using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventSmulator : MonoBehaviour
{
    public static UIEventSmulator Instance;

    private Camera gazeCam; // Cámara usada para raycast desde la vista
    public float gazeMaxDistance = 50f; // Distancia máxima del raycast

    void Start()
    {
        gazeCam = CameraPointerManager.Instance.gameObject.GetComponent<Camera>();
        // Llamamos aquí a la clase CameraPointerManager como instancia en UIEventVR
    }

    void Update()
    {
        // Aquí normalmente se harían comprobaciones por frame
    }

    void OnPointerClick(PointerEventData pointerEvent)
    {
        // Este es el evento que se ejecuta cuando el usuario hace clic sobre un elemento UI
        // Se recibe información del puntero (posición, tipo de click, etc.)
        ExecuteEvents.ExecuteHierarchy(
            pointerEvent.pointerCurrentRaycast.gameObject,
            pointerEvent,
            ExecuteEvents.pointerClickHandler
        );
        // Dispara el evento de click en el objeto UI actual
    }

    void OnPointerEnter(PointerEventData pointerEvent)
    {
        // Este se ejecuta cuando el puntero entra sobre un elemento UI
        GazeManager.Instance.SetGazedAt(true); // Indica al GazeManager que el puntero está sobre un objeto interactivo
        ExecuteEvents.ExecuteHierarchy(
            pointerEvent.pointerCurrentRaycast.gameObject,
            pointerEvent,
            ExecuteEvents.pointerEnterHandler
        );
        // Dispara el evento de "entrada" (hover)
    }

    void OnPointerExit(PointerEventData pointerEvent)
    {
        // Este se ejecuta cuando el puntero sale de un elemento UI
        GazeManager.Instance.SetGazedAt(false); // Indica al GazeManager que el puntero ya no está sobre algo
        ExecuteEvents.ExecuteHierarchy(
            pointerEvent.pointerCurrentRaycast.gameObject,
            pointerEvent,
            ExecuteEvents.pointerExitHandler
        );
        // Dispara el evento de "salida" (hover out)
    }

    private PointerEventData PlacePointer()
    {
        // Crea y posiciona un PointerEventData según la cámara del GazeManager
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = new Vector2(
            gazeCam.pixelWidth / 2,
            gazeCam.pixelHeight / 2
        );
        // Posiciona el puntero en el centro de la cámara (como un punto de mira)

        return pointer;
    }
}
