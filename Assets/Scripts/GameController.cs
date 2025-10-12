using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance; // Singleton para acceso global

    // Referencias públicas (configurables desde el Inspector)
    public Camera gameCamera;
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    [SerializeField] public GameObject menu;          // Menú (por ejemplo, menú de pausa o derrota)
    [SerializeField] TMP_Text puntaje;                // Texto que muestra el puntaje

    // Configuraciones públicas
    public float enemySpawningCooldown = 1f;   // Tiempo entre spawneos
    public float enemySpawningDistance = 7f;   // Distancia desde la cámara
    public float shootingCooldown = 0.5f;      // Tiempo mínimo entre disparos

    // Variables internas
    private float enemySpawningTimer = 0f;
    private float shootingTimer = 0f;
    private int npuntaje = 0; // Puntaje actual del jugador

    private void Awake()
    {
        // Patrón Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas si lo necesitas
        }
    }

    void Start()
{
    Time.timeScale = 1f; // siempre reanudar cuando inicia la escena
}


    void Update()
    {
        shootingTimer -= Time.deltaTime;
        enemySpawningTimer -= Time.deltaTime;

        // Spawnear enemigos si el menú no está activo
        if (enemySpawningTimer <= 0f && (menu == null || !menu.activeSelf))
        {
            enemySpawningTimer = enemySpawningCooldown;

            GameObject enemyObject = Instantiate(enemyPrefab);

            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            enemyObject.transform.position = new Vector3(
                gameCamera.transform.position.x + Mathf.Cos(randomAngle) * enemySpawningDistance,
                0f,
                gameCamera.transform.position.z + Mathf.Sin(randomAngle) * enemySpawningDistance
            );

            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.direction = (gameCamera.transform.position - enemy.transform.position).normalized;
            enemy.transform.LookAt(gameCamera.transform.position);
        }

        // Raycast para disparo
        RaycastHit hit;
        if (Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemy") && shootingTimer <= 0f)
            {
                shootingTimer = shootingCooldown;

                GameObject bulletObject = Instantiate(bulletPrefab);
                bulletObject.transform.position = gameCamera.transform.position;

                Bullet bullet = bulletObject.GetComponent<Bullet>();
                bullet.direction = gameCamera.transform.forward;

                npuntaje += 100;
                puntaje.text = "Puntaje: " + npuntaje;
            }
        }
    }

    // ⚙️ Función llamada cuando el jugador pierde o colisiona con un enemigo
public void menu_principal()
{
    PlayerPrefs.SetInt("score", npuntaje);
    PlayerPrefs.Save();

    // Desactivar el puntero antes de salir
    var reticle = FindObjectOfType<CardboardReticlePointer>();
    if (reticle != null)
        reticle.gameObject.SetActive(false);

    Time.timeScale = 1f;
    Destroy(gameObject);

    UnityEngine.SceneManagement.SceneManager.LoadScene("Menu_principal");
}


}
