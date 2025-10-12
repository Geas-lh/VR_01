using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadOrRestartPractica02()
{
    Time.timeScale = 1f;
    var controller = FindObjectOfType<GameController>();
    if (controller != null)
        Destroy(controller.gameObject);

    string targetScene = "Practica02";
    var currentScene = SceneManager.GetActiveScene().name;

    if (currentScene == targetScene)
        SceneManager.LoadScene(currentScene); // reinicia si ya estás ahí
    else
        SceneManager.LoadScene(targetScene);  // si no, la carga desde cero
}


    public void QuitGame()
    {
        Application.Quit();
    }
}

