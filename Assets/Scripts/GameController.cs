using System.Collections;
using TMPro; // Para mostrar texto en pantalla con TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // --- Referencias públicas configurables desde el Inspector ---
    public Camera gameCamera;          // Cámara principal del jugador (usada para disparar)
    public GameObject bulletPrefab;    // Prefab de la bala
    public GameObject enemyPrefab;     // Prefab del enemigo
    [SerializeField] GameObject menu;  // Referencia al menú principal
    [SerializeField] TMP_Text puntaje; // Texto UI para mostrar la puntuación


    // --- Variables configurables ---
    public float enemySpawningCooldown = 1f;   // Tiempo entre apariciones de enemigos
    public float enemySpawningDistance = 7f;   // Distancia desde la cámara donde aparecen
    public float shootingCooldown = 0.5f;      // Tiempo entre disparos permitidos

    // --- Variables internas ---
    private float enemySpawningTimer = 0;      // Temporizador para generar enemigos
    private float shootingTimer = 0;           // Temporizador para disparos
    private int npuntaje = 0;                  // Puntuación actual del jugador

    void Start()
    {
        // Inicialización: se pueden resetear variables o preparar estado del juego
    }

    // --- Detecta colisiones con el jugador (Game Over si toca un enemigo) ---
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            menu_principal(); // Llama al menú principal si colisiona con un enemigo
        }
    }

    // --- Función que se llama una vez por frame ---
    void Update()
    {
        // Reduce los temporizadores de disparo y aparición de enemigos
        shootingTimer -= Time.deltaTime;
        enemySpawningTimer -= Time.deltaTime;

        // --- Spawning de enemigos ---
        if (enemySpawningTimer <= 0 && menu.activeSelf == false)
        {
            enemySpawningTimer = enemySpawningCooldown;

            // Instancia un nuevo enemigo
            GameObject enemyObject = Instantiate(enemyPrefab);

            // Genera un ángulo aleatorio alrededor de la cámara
            float randomAngle = UnityEngine.Random.Range(0, Mathf.PI * 2);

            // Posiciona el enemigo en un círculo alrededor de la cámara
            enemyObject.transform.position = new Vector3(
                gameCamera.transform.position.x + Mathf.Cos(randomAngle) * enemySpawningDistance,
                gameCamera.transform.position.y,
                gameCamera.transform.position.z + Mathf.Sin(randomAngle) * enemySpawningDistance
            );

            // Hace que el enemigo mire hacia el centro (jugador)
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.direction = (gameCamera.transform.position - enemy.transform.position).normalized;
            enemy.transform.LookAt(Vector3.zero);
        }

        // --- Detección de disparos mediante Raycast ---
        RaycastHit hit;
        if (Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out hit))
        {
            // Si el jugador mira a un enemigo y puede disparar
            if (hit.transform.CompareTag("Enemy") && shootingTimer <= 0)
            {
                shootingTimer = shootingCooldown; // Reinicia el cooldown del disparo

                // Crea una bala desde la cámara
                GameObject bulletObject = Instantiate(bulletPrefab);
                bulletObject.transform.position = gameCamera.transform.position;

                // Asigna dirección a la bala
                Bullet bullet = bulletObject.GetComponent<Bullet>();
                bullet.direction = gameCamera.transform.forward;

                // Aumenta el puntaje
                npuntaje += 100;
                puntaje.text = "Puntaje: " + npuntaje;
            }
        }
    }

    // --- Vuelve al menú principal ---
    void menu_principal()
    {
        UnityEngine.Debug.Log("Volviendo al menú principal...");
        SceneManager.LoadScene("Menu_principal");
    }
}
