using UnityEngine;
using UnityEngine.EventSystems;

public static class UIEventSimulator
{
    public static void SimulateClick(GameObject target)
    {
        if (target == null) return;

        var pointer = new PointerEventData(EventSystem.current);

        // Paso 1: Pressed (PointerDown)
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.pointerDownHandler);

        // Paso 2: Suelta el click (PointerUp)
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.pointerUpHandler);

        // Paso 3: Click real (SubmitHandler) → activa botones, toggles, etc.
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.submitHandler);
    }

    public static void SimulateEnter(GameObject target)
    {
        if (target == null) return;
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.pointerEnterHandler);
    }

    public static void SimulateExit(GameObject target)
    {
        if (target == null) return;
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.pointerExitHandler);
    }
}
