using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager Instance; // Singleton accesible desde otros scripts

    [Header("Configuración del puntero")]
    [SerializeField] private GameObject pointer;
    [SerializeField] private float maxDistancePointer = 15f;
    [SerializeField, Range(0, 1)] private float distPointerObject = 0.95f;
    [SerializeField] private float scaleSize = 0.025f;

    private const float _maxDistance = 15f;
    private readonly string interactableTag = "Interactable";

    private GameObject _gazedAtObject = null;

    // 👉 ESTE es el punto que otros scripts usarán
    public Vector3 hitPoint { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        // Se suscribe al evento de selección del gaze
        GazeManager.Instance.OnGazeSelection += GazeSelection;
    }

    private void GazeSelection()
    {
        // Envía mensaje de "clic" al objeto observado
        _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        RaycastHit hit;

        // Lanza un rayo hacia adelante desde la cámara
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            hitPoint = hit.point; // 👈 Actualiza la posición del impacto (para otros scripts)

            // Detecta si cambió el objeto observado
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // Sale del objeto anterior
                if (_gazedAtObject != null)
                    _gazedAtObject.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);

                // Nuevo objeto detectado
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection();
            }

            // Si el objeto tiene el tag "Interactable"
            if (hit.transform.CompareTag(interactableTag))
            {
                Debug.Log("Tag Interactable detectado: " + hit.transform.name);
                PointerOnGaze(hit.point);
            }
            else
            {
                Debug.Log("OutGaze");
                PointerOutGaze();
            }
        }
        else
        {
            // Si no hay ningún objeto enfrente
            if (_gazedAtObject != null)
                _gazedAtObject.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);

            _gazedAtObject = null;
            PointerOutGaze();
        }

        // Disparo con Cardboard trigger (toque o botón)
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void PointerOutGaze()
    {
        // Vuelve el puntero a su posición y tamaño por defecto
        pointer.transform.localScale = Vector3.one * 0.1f;
        pointer.transform.parent.localPosition = new Vector3(0, 0, maxDistancePointer);
        pointer.transform.parent.parent.rotation = transform.rotation;
        GazeManager.Instance.CancelGazeSelection();
    }

    private void PointerOnGaze(Vector3 hitPosition)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPosition);
        pointer.transform.localScale = Vector3.one * scaleFactor;
        pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPosition, distPointerObject);
    }

    private Vector3 CalculatePointerPosition(Vector3 start, Vector3 end, float t)
    {
        // Calcula una posición interpolada entre la cámara y el punto de impacto
        return start + t * (end - start);
    }
}
