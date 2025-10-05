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
            GameObject hitObj = hit.transform.gameObject;

            if (_gazedAtObject != hitObj)
            {
                if (_gazedAtObject != null)
                {
                    SimulateExit(_gazedAtObject);
                }

                _gazedAtObject = hitObj;

                if (_gazedAtObject != null)
                {
                    SimulateEnter(_gazedAtObject);
                    GazeManager.Instance?.StartGazeSelection();
                }
            }
            else
            {
                // Si el objeto sigue igual y el timer no corre, reinicia el gaze
                if (!GazeManager.Instance.IsRunning)
                {
                    GazeManager.Instance?.StartGazeSelection();
                }
            }

            if (hitObj.CompareTag(interactableTag))
            {
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze();
                GazeManager.Instance?.CancelGazeSelection();
            }
        }
        else
        {
            if (_gazedAtObject != null)
            {
                SimulateExit(_gazedAtObject);
            }

            _gazedAtObject = null;
            PointerOutGaze();
            GazeManager.Instance?.CancelGazeSelection();
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

    private void SimulateClick(GameObject target)
    {
        if (target == null) return;
        var pointerData = new PointerEventData(EventSystem.current);

        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.submitHandler);
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
