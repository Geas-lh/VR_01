using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI infoText;     
    public TextMeshProUGUI aciertosText;
    public TextMeshProUGUI fallosText;

    [Header("Game Objects")]
    public GameObject ball;
    public Player player;
    public Cup[] cups;

    private float resetTimer = 3f;

    private int aciertos = 0;
    private int fallos = 0;
    private bool resultadoProcesado = false;  // <- NUEVO: evita que se sume varias veces

    void Start()
    {
        // Cargar valores guardados
        aciertos = PlayerPrefs.GetInt("Aciertos", 0);
        fallos = PlayerPrefs.GetInt("Fallos", 0);

        UpdateScoreText();
        infoText.text = "¡Elige el vaso correcto!";

        StartCoroutine(ShuffleRoutine());
    }

    void Update()
    {
        // Solo procesar el resultado una vez
        if (player.picked && !resultadoProcesado)
        {
            resultadoProcesado = true; // <- evita múltiples sumas

            if (player.won)
            {
                infoText.text = "¡Ganaste!";
                aciertos++;
            }
            else
            {
                infoText.text = "Perdiste :( ¡Intenta de nuevo!";
                fallos++;
            }

            // Guardar progreso
            PlayerPrefs.SetInt("Aciertos", aciertos);
            PlayerPrefs.SetInt("Fallos", fallos);
            PlayerPrefs.Save();

            UpdateScoreText();
        }

        // Esperar para reiniciar la escena
        if (resultadoProcesado)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void UpdateScoreText()
    {
        aciertosText.text = "Aciertos: " + aciertos;
        fallosText.text = "Fallos: " + fallos;
    }

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSeconds(1f);

        foreach (Cup cup in cups)
            cup.MoveUp();

        yield return new WaitForSeconds(0.5f);

        Cup targetCup = cups[Random.Range(0, cups.Length)];
        targetCup.ball = ball;
        ball.transform.position = new Vector3(
            targetCup.transform.position.x,
            ball.transform.position.y,
            targetCup.transform.position.z
        );

        yield return new WaitForSeconds(1f);

        foreach (Cup cup in cups)
            cup.MoveDown();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {
            Cup cup1 = cups[Random.Range(0, cups.Length)];
            Cup cup2 = cup1;

            while (cup2 == cup1)
                cup2 = cups[Random.Range(0, cups.Length)];

            Vector3 temp = cup1.targetPosition;
            cup1.targetPosition = cup2.targetPosition;
            cup2.targetPosition = temp;

            yield return new WaitForSeconds(0.75f);
        }

        player.canPick = true;
    }
}
