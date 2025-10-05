using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public Player player;
    public Ball ball;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Start()
    {
        gameOverPanel.SetActive(false); // Asegúrate que esté oculto al inicio
    }

    void Update()
    {
        if (isGameOver) return;

        if (ball.transform.position.z < player.transform.position.z)
        {
            TriggerGameOver();
        }
        else
        {
            scoreText.text = "Score: " + ball.score;
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        scoreText.text = "Game Over!\nYour final score: " + ball.score;

        gameOverPanel.SetActive(true); // Mostrar el panel UI

        // ✋ Detener la pelota
        if (ball != null && ball.rb != null)
        {
            ball.rb.velocity = Vector3.zero;
            ball.rb.isKinematic = true;      // Detiene las físicas
            ball.enabled = false;            // Desactiva el script Ball (score/rotación)
        }

        // 🔁 Opcional: Puedes desactivar otros scripts aquí si lo necesitas
    }

    // Llamado desde botón en el panel
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Llamado desde botón en el panel
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cambia el nombre por el real
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
