using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;    // Permite cambiar de escenas
using UnityEngine.EventSystems;       // Permite manejar eventos del sistema de UI

public class MenuController : MonoBehaviour
{
    private Camera xrCamera; // Cámara VR usada para detectar la posición del puntero o la mirada

    void Start()
    {
        // Obtiene la cámara desde el objeto administrado por CameraPointerManager
        // CameraPointerManager.Instance hace referencia a un objeto singleton que maneja el puntero en VR
        xrCamera = CameraPointerManager.Instance.gameObject.GetComponent<Camera>();
    }

    // --- Funciones para cambiar de escena ---

    public void menu_principal()
    {
        // Carga la escena del menú principal
        SceneManager.LoadScene("Menu_principal");
    }

    public void reiniciar()
    {
        // Reinicia el juego cargando nuevamente la escena principal de VR
        SceneManager.LoadScene("Juego_principal_vr");
    }

    public void game_over()
    {
        // Carga la escena de Game Over
        SceneManager.LoadScene("Menu_Game_Over");
    }

    // --- Función para simular un clic en VR (por ejemplo, cuando el jugador mira un botón y confirma con un input) ---
    public void OnPointerClickXR()
    {
        // Crea un evento de clic sobre el UI
        PointerEventData pointerEvent = PlacePointer();

        // Ejecuta el evento de clic sobre este mismo objeto (this.gameObject)
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerClickHandler);
    }

    // --- Genera la información necesaria para el clic del puntero ---
    private PointerEventData PlacePointer()
    {
        // Convierte el punto donde el puntero VR está mirando (hitpoint) a coordenadas de pantalla
        Vector3 screenPos = xrCamera.WorldToScreenPoint(CameraPointerManager.Instance.hitPoint);

        // Crea un nuevo evento de datos de puntero, necesario para ejecutar un clic en UI
        var pointer = new PointerEventData(EventSystem.current);

        // Asigna la posición en pantalla del puntero (donde el usuario está mirando o apuntando)
        pointer.position = new Vector2(screenPos.x, screenPos.y);

        // Devuelve el evento preparado
        return pointer;
    }
}
