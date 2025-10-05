using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    public Player player;
    public float speed = 2.5f;
    public float speedIncrement = 0.5f;
    public float deflectionRadius = 2f;
    public float rotatingSpeed = 50f;

    public Rigidbody rb;
    public Vector3 direction;
    public int score = 0;

    [Header("UI")]
    public TextMeshProUGUI highScoreText;

    private Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            Debug.LogError("❌ No se ha asignado el objeto Player en el inspector.");
            return;
        }

        playerAnimator = player.GetComponent<Animator>();

        // Dirección inicial
        direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;

        // Mostrar HighScore inicial
        UpdateHighScoreText();
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;

        transform.Rotate(Vector3.up, rotatingSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.right, rotatingSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.forward, rotatingSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerHit();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        }
    }

    public void OnPlayerHit()
    {
        direction = (transform.position - player.transform.position).normalized;

        speed += speedIncrement;
        score++;

        if (playerAnimator != null)
            playerAnimator.SetTrigger("Hit");

        // Guardar y actualizar HighScore
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            UpdateHighScoreText();
            Debug.Log("🎉 Nuevo HighScore: " + score);
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "High Score: " + currentHighScore;
        }
        else
        {
            Debug.LogWarning("⚠ No se asignó el campo HighScoreText en el inspector.");
        }
    }
}
