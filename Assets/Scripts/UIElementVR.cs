using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIElementVR : MonoBehaviour
{
    [Header("Eventos del puntero VR")]
    public UnityEvent OnXRPointerEnter;
    public UnityEvent OnXRPointerExit;
    public UnityEvent OnXRPointerClick;

    private Camera xrCamera;

    void Start()
    {
        // Obtenemos la cámara principal desde el CameraPointerManager
        xrCamera = CameraPointerManager.Instance.gameObject.GetComponent<Camera>();
    }

    // 👉 Se llama cuando el usuario "hace clic" con el puntero VR
    public void OnPointerClickXR()
    {
        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerClickHandler);

        // Dispara el evento público asignable en el inspector
        OnXRPointerClick.Invoke();
    }

    // 👉 Se llama cuando el puntero VR entra en este elemento (hover)
    public void OnPointerEnterXR()
    {
        // Puedes agregar animaciones o sonidos aquí si quieres
        OnXRPointerEnter.Invoke();

        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerEnterHandler);
    }

    // 👉 Se llama cuando el puntero VR sale del elemento
    public void OnPointerExitXR()
    {
        OnXRPointerExit.Invoke();

        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerExitHandler);
    }

    // 📍 Crea un evento PointerEventData basado en la posición del puntero VR
    private PointerEventData PlacePointer()
    {
        Vector3 screenPos = xrCamera.WorldToScreenPoint(CameraPointerManager.Instance.hitPoint);
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = new Vector2(screenPos.x, screenPos.y);
        return pointer;
    }
}
