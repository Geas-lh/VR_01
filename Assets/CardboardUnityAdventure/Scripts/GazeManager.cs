using System;
using UnityEngine;
using UnityEngine.UI;

public class GazeManager : MonoBehaviour
{
    public event Action OnGazeSelection;

    public static GazeManager Instance;

    [SerializeField] private GameObject gazeBarCanvas;
    [SerializeField] private Image fillIndicator;
    [Tooltip("Time in seconds")]
    [SerializeField] private float timeForSelection = 2.5f;

    private float timeCounter;
    private float timeProgress;
    private bool runTimer;

    public bool IsRunning => runTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (gazeBarCanvas != null)
            gazeBarCanvas.SetActive(false);

        if (fillIndicator != null)
            fillIndicator.fillAmount = Normalize();
    }

    private void Update()
    {
        if (runTimer)
        {
            timeProgress += Time.unscaledDeltaTime;
            AddValue(timeProgress);
        }
    }

    public void SetUpGaze(float timeForSelection)
    {
        this.timeForSelection = timeForSelection;
    }

    public void StartGazeSelection()
    {
        if (gazeBarCanvas == null || fillIndicator == null) return;

        gazeBarCanvas.SetActive(true);
        runTimer = true;
        timeProgress = 0;
        timeCounter = 0; // Resetear timer aquí también
        fillIndicator.fillAmount = 0;
    }

    public void CancelGazeSelection()
    {
        Debug.Log("CancelGazeSelection ejecutado en: " + gameObject.name);

        if (gazeBarCanvas != null)
            gazeBarCanvas.SetActive(false);
        else
            Debug.LogWarning("⚠ gazeBarCanvas es NULL");

        runTimer = false;
        timeProgress = 0;
        timeCounter = 0;

        if (fillIndicator != null)
            fillIndicator.fillAmount = 0;
    }

    private void AddValue(float val)
    {
        timeCounter = val;
        if (timeCounter >= timeForSelection)
        {
            timeCounter = 0;
            runTimer = false;
            OnGazeSelection?.Invoke();
        }

        if (fillIndicator != null)
            fillIndicator.fillAmount = Normalize();
    }

    private float Normalize()
    {
        return timeForSelection > 0 ? timeCounter / timeForSelection : 0;
    }
}
