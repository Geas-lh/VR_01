using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIElementVR : MonoBehaviour
{
    public UnityEvent OnXRPointerEnter;
    public UnityEvent OnXRPointerExit;
    private Camera _xrCamera;

    // Start is called before the first frame update
    void Start()
    {
        _xrCamera = CameraPointerManager.instance.gameObject.GetComponent<Camera>(); 
        // Llamamos aquí a la clase CameraPointerManager como instance en UIElementXR;
    }

    // Update is called once per frame
    public void OnPointerClickXR()
    {
        PointerEventData pointerEvent = PlacePointer(); 
        // Este es el elemento que nos permitirá hacer clic sobre el elemento UI 
        // pero para ello se necesita una posición la cual está en la función PlacePointer
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerClickHandler); 
        // Ejecutamos el evento al hacer clic.
    }

    public void OnPointerEnterXR()
    {
        GazeManager.Instance.SetUpGaze(2.5f); // Reducimos el tiempo de carga del evento;
        OnXRPointerEnter.Invoke(); // Llamamos al EventTrigger si no hay problema;

        PointerEventData pointerEvent = PlacePointer(); 
        // Este es el elemento que nos permitirá hacer clic sobre el elemento UI 
        // pero para ello se necesita una posición la cual está en la función PlacePointer
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerEnterHandler); 
        // Ejecutamos el evento al hacer clic.
    }

    public void OnPointerExitXR()
    {
        GazeManager.Instance.SetUpGaze(2.5f); // Reducimos el tiempo de carga del evento;
        OnXRPointerExit.Invoke(); // Llamamos al EventTrigger si no hay problema;

        PointerEventData pointerEvent = PlacePointer(); 
        // Este es el elemento que nos permitirá hacer clic sobre el elemento UI 
        // pero para ello se necesita una posición la cual está en la función PlacePointer
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerExitHandler); 
        // Ejecutamos el evento al hacer clic.
    }

    private PointerEventData PlacePointer()
    {
        Vector3 screenPos = _xrCamera.WorldToScreenPoint(CameraPointerManager.instance.hitPoint);
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(screenPos.x, screenPos.y);
        return eventData;
    }
}
