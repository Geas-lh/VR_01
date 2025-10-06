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

    [Header("Animation")]
    public Animator playerAnimator; // 👈 Asignar manualmente
    public string hitTriggerName = "Hit"; // 👈 Nombre del Trigger en el Animator

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            Debug.LogError("❌ No se ha asignado el objeto Player.");
            return;
        }

        if (playerAnimator == null)
        {
            Debug.LogWarning("⚠ No se asignó el Animator. No se reproducirá animación.");
        }

        // Dirección inicial
        direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;

        // Mostrar HighScore
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
        {
            playerAnimator.ResetTrigger(hitTriggerName); // 🧼 Limpia el trigger
            playerAnimator.SetTrigger(hitTriggerName);   // 🚀 Lo activa
        }

        // HighScore
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "High Score: " + currentHighScore;
        }
    }
}
