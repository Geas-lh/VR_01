using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoManager : MonoBehaviour
{
    [Header("Luces Disco")]
    public List<Light> discoLights;

    [Header("Objetos que cambian de color")]
    public List<Renderer> discoObjects;

    [Header("Intervalo de cambio (segundos)")]
    public float colorChangeInterval = 0.5f;

    void Start()
    {
        StartCoroutine(ChangeColorsLoop());
    }

    IEnumerator ChangeColorsLoop()
    {
        while (true)
        {
            ChangeLightColors();
            ChangeObjectColors();
            yield return new WaitForSeconds(colorChangeInterval);
        }
    }

    void ChangeLightColors()
    {
        foreach (Light light in discoLights)
        {
            light.color = GetRandomColor();
        }
    }

    void ChangeObjectColors()
    {
        foreach (Renderer rend in discoObjects)
        {
            Color randomColor = GetRandomColor();
            if (rend.material != null)
            {
                rend.material.color = randomColor;
            }
        }
    }

    Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
