using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager instance;

    [SerializeField] private GameObject pointer;
    [SerializeField] private float maxDistancePointer = 15f;
    private readonly string interactableTag = "Interactable";
    private float scaleSize = 0.025f;

    [Range(0, 1)]
    [SerializeField] private float distPointerObject = 0.95f;

    private const float _maxDistance = 15f;
    private GameObject _gazedAtObject = null;

    [HideInInspector]
    public Vector3 hitPoint;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.OnGazeSelection += GazeSelection;
        }
    }

    private void GazeSelection()
    {
        if (_gazedAtObject != null)
        {
            SimulateClick(_gazedAtObject);
        }
    }

    public void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            hitPoint = hit.point;

            // Detecta nuevo objeto
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // Exit del anterior
                if (_gazedAtObject != null)
                {
                    SimulateExit(_gazedAtObject);
                }

                // Nuevo objeto
                _gazedAtObject = hit.transform.gameObject;

                if (_gazedAtObject != null)
                {
                    SimulateEnter(_gazedAtObject);
                    GazeManager.Instance?.StartGazeSelection();
                }
            }

            // Si es interactuable
            if (hit.transform != null && hit.transform.CompareTag(interactableTag))
            {
                Debug.Log("Se reconoció el Tag Interactable → " + hit.transform.name);
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze();
            }
        }
        else
        {
            // Nada detectado
            if (_gazedAtObject != null)
            {
                SimulateExit(_gazedAtObject);
            }
            _gazedAtObject = null;
            PointerOutGaze();
        }

        // Click manual (trigger Cardboard)
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            if (_gazedAtObject != null)
            {
                SimulateClick(_gazedAtObject);
            }
        }
    }

    private void PointerOutGaze()
    {
        if (pointer == null) return;

        pointer.transform.localScale = Vector3.one * 0.1f;

        if (pointer.transform.parent != null)
        {
            pointer.transform.parent.localPosition = new Vector3(0, 0, maxDistancePointer);

            if (pointer.transform.parent.parent != null)
            {
                pointer.transform.parent.parent.rotation = transform.rotation;
            }
        }

        GazeManager.Instance?.CancelGazeSelection();
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        if (pointer == null) return;

        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);
        pointer.transform.localScale = Vector3.one * scaleFactor;

        if (pointer.transform.parent != null)
        {
            pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, distPointerObject);
        }
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x + t * (p1.x - p0.x);
        float y = p0.y + t * (p1.y - p0.y);
        float z = p0.z + t * (p1.z - p0.z);
        return new Vector3(x, y, z);
    }

    // ----------- 🔹 Métodos con ExecuteEvents ----------------

private void SimulateClick(GameObject target)
{
    if (target == null) return;
    var pointerData = new PointerEventData(EventSystem.current);

    // Hover antes de click
    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerEnterHandler);

    // Click (Pressed)
    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);
    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);

    // Submit → activa el Toggle
    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.submitHandler);

    // Exit después del click (opcional)
    // ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerExitHandler);
}


    private void SimulateEnter(GameObject target)
    {
        if (target == null) return;
        var pointerData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerEnterHandler);
    }

    private void SimulateExit(GameObject target)
    {
        if (target == null) return;
        var pointerData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerExitHandler);
    }
}
