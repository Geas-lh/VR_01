using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance; //para declarar una instancia del GameController es decir para entrar a sus metodos.
    // Referencias públicas (configurables desde el Inspector)
    public Camera gameCamera;
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    [SerializeField] public GameObject menu;          // Menú (por ejemplo, menú de pausa o derrota)
    [SerializeField] TMP_Text puntaje;         // Texto que muestra el puntaje

    // Configuraciones públicas (ajustables desde el Inspector)
    public float enemySpawningCooldown = 1f;   // Tiempo entre spawneo de enemigos (segundos)
    public float enemySpawningDistance = 7f;   // Distancia radial desde la cámara donde aparecen los enemigos
    public float shootingCooldown = 0.5f;      // Tiempo mínimo entre disparos (segundos)

    // Variables privadas para control interno
    private float enemySpawningTimer = 0;      // Temporizador para spawnear enemigos
    private float shootingTimer = 0;           // Temporizador para permitir nuevo disparo
    private int npuntaje = 0;                  // Puntuación actual del jugador

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Use this for initialization
    void Start()
    {
        // Aquí podrías inicializar cosas como sonidos, cargar niveles, etc.
        // En este caso, no se necesita nada adicional.
    }

    // Update is called once per frame
    void Update()
    {
        // Reducir temporizadores en cada frame
        shootingTimer -= Time.deltaTime;
        enemySpawningTimer -= Time.deltaTime;

        // Si el temporizador de spawn ha terminado Y el menú NO está activo, spawnear un enemigo
        if (enemySpawningTimer <= 0f && !menu.activeSelf)
        {
            // Reiniciar el temporizador de spawn
            enemySpawningTimer = enemySpawningCooldown;

            // Instanciar un enemigo
            GameObject enemyObject = Instantiate(enemyPrefab);

            // Generar un ángulo aleatorio (0 a 2π radianes)
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);

            // Posicionar al enemigo en un círculo alrededor de la cámara
            enemyObject.transform.position = new Vector3(
                gameCamera.transform.position.x + Mathf.Cos(randomAngle) * enemySpawningDistance,
                0f, // Altura fija (plano 2D)
                gameCamera.transform.position.z + Mathf.Sin(randomAngle) * enemySpawningDistance
            );

            // Obtener el componente Enemy del objeto instanciado
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            // Calcular dirección hacia la cámara (para movimiento del enemigo)
            enemy.direction = (gameCamera.transform.position - enemy.transform.position).normalized;

            // Hacer que el enemigo mire hacia la cámara (CORREGIDO: antes miraba a Vector3.zero)
            enemy.transform.LookAt(gameCamera.transform.position);


        }

        // Raycast desde la cámara hacia adelante
        RaycastHit hit;
        if (Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out hit))
        {
            // Si el rayo golpeó un enemigo Y el temporizador de disparo permite disparar
            if (hit.transform.CompareTag("Enemy") && shootingTimer <= 0f)
            {
                // Reiniciar el temporizador de disparo
                shootingTimer = shootingCooldown;

                // Instanciar una bala en la posición de la cámara
                GameObject bulletObject = Instantiate(bulletPrefab);
                bulletObject.transform.position = gameCamera.transform.position;

                // Obtener el componente Bullet de la bala
                Bullet bullet = bulletObject.GetComponent<Bullet>();
                // Asignar la dirección de la bala (hacia donde mira la cámara)
                bullet.direction = gameCamera.transform.forward;

                // Incrementar puntuación en 100 puntos
                npuntaje += 100;
                // Actualizar el texto visual del puntaje
                puntaje.text = "Puntaje: " + npuntaje;
            }
        }

    }

    // Función para mostrar el menú principal (puede ser reinicio, pausa, derrota, etc.)
    public void menu_principal()
    {
        // Ejemplo: activar el menú y pausar el juego
        menu.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego (opcional)
    }

}