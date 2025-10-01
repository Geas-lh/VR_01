using UnityEngine;
using TMPro; // Importar el namespace de TextMesh Pro
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Player player;
    public Ball ball;
    public TextMeshProUGUI scoreText; // <- Para Canvas UI

    private float gameOverTimer = 3f;

    void Start () {
    
    }
    
    void Update () {
        bool isGameOver = ball.transform.position.z < player.transform.position.z;

        if (!isGameOver) {
            scoreText.text = "Score: " + ball.score;
        } else {
            scoreText.text = "Game over!\nYour final score: " + ball.score;

            gameOverTimer -= Time.deltaTime;
            if (gameOverTimer <= 0f) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
