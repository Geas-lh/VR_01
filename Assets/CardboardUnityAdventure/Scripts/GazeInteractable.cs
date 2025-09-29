using UnityEngine;
using UnityEngine.UI;

public class GazeInteractable : MonoBehaviour
{
    [Header("Objeto 3D opcional")]
    [SerializeField] private GameObject targetToToggle;

    [Header("UI opcional")]
    [SerializeField] private Toggle uiToggle;

    [Header("UI opcional (si es botón normal)")]
    [SerializeField] private Button uiButton;

    public void OnPointerEnterXR()
    {
        // Aquí no activamos nada todavía
        Debug.Log($"{name} >>> ENTER (puntero encima)");
    }

    public void OnPointerExitXR()
    {
        // Aquí tampoco desactivamos nada, el estado se mantiene
        Debug.Log($"{name} >>> EXIT (puntero afuera)");
    }

    public void OnPointerClickXR()
    {
        // 🔹 Si es un Toggle: cambia de estado y lo mantiene
        if (uiToggle != null)
        {
            bool nuevoEstado = !uiToggle.isOn;
            uiToggle.isOn = nuevoEstado;
            uiToggle.onValueChanged.Invoke(nuevoEstado); // dispara eventos

            Debug.Log($"{name} >>> TOGGLE cambiado a: {nuevoEstado}");
        }

        // 🔹 Si es un Botón clásico: ejecuta el click 1 sola vez
        if (uiButton != null)
        {
            uiButton.onClick.Invoke();
            Debug.Log($"{name} >>> BOTÓN click ejecutado");
        }

        // 🔹 Si es un objeto 3D: alterna encendido/apagado
        if (targetToToggle != null)
        {
            bool nuevoEstado = !targetToToggle.activeSelf;
            targetToToggle.SetActive(nuevoEstado);
            Debug.Log($"{name} >>> Objeto 3D cambiado a: {nuevoEstado}");
        }
    }
}
