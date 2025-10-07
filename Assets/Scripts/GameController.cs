using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;  // Alias para evitar conflicto con System.Random

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI infoText;      // Texto para mensajes tipo "Ganaste", "Perdiste"
    public TextMeshProUGUI scoreText;     // Nuevo texto para mostrar el score
    public GameObject ball;
    public Player player;
    public Cup[] cups;

    private float resetTimer = 3f;

    private int score = 0; // Variable para el score

    // Use this for initialization
    void Start()
    {
        UpdateScoreText();
        infoText.text = "Elige el vaso correcto!";

        StartCoroutine(ShuffleRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.picked)
        {
            if (player.won)
            {
                infoText.text = "Ganaste!";
                score++;  // Aumentar score
                UpdateScoreText();
            }
            else
            {
                infoText.text = "Perdiste :( Intentar de Nuevo!";
            }

            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSeconds(1f);

        foreach (Cup cup in cups)
        {
            cup.MoveUp();
        }

        yield return new WaitForSeconds(0.5f);

        Cup targetCup = cups[Random.Range(0, cups.Length)];
        targetCup.ball = ball;
        ball.transform.position = new Vector3(
            targetCup.transform.position.x,
            ball.transform.position.y,
            targetCup.transform.position.z
        );

        yield return new WaitForSeconds(1.0f);

        foreach (Cup cup in cups)
        {
            cup.MoveDown();
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < 5; i++)
        {
            Cup cup1 = cups[Random.Range(0, cups.Length)];
            Cup cup2 = cup1;

            while (cup2 == cup1)
            {
                cup2 = cups[Random.Range(0, cups.Length)];
            }

            Vector3 cup1Position = cup1.targetPosition;

            cup1.targetPosition = cup2.targetPosition;
            cup2.targetPosition = cup1Position;

            yield return new WaitForSeconds(0.75f);
        }

        player.canPick = true;
    }
}
