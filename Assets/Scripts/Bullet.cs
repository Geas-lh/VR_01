using UnityEngine;                 // Importa las librer�as necesarias de Unity
using System.Collections;

public class Bullet : MonoBehaviour // Define una clase llamada "Bullet" que hereda de MonoBehaviour
{
    public float speed = 1f;         // Velocidad de la bala (puede ser ajustada desde el inspector)
    public Vector3 direction;        // Direcci�n hacia donde se mover� la bala (se puede establecer desde otro script)

    private float lifetime = 2f;     // Tiempo de vida de la bala en segundos antes de que se destruya autom�ticamente

    // M�todo Start: se ejecuta al iniciar el objeto
    void Start()
    {
        // En este caso no se hace nada al iniciar
    }

    // M�todo Update: se ejecuta una vez por cada frame
    void Update()
    {
        // Mueve la bala en la direcci�n especificada a la velocidad definida, teniendo en cuenta el tiempo entre frames
        transform.position += direction * speed * Time.deltaTime;

        // Resta al tiempo de vida el tiempo transcurrido desde el �ltimo frame
        lifetime -= Time.deltaTime;

        // Si el tiempo de vida ha llegado a cero o menos, destruye el objeto (la bala)
        if (lifetime <= 0)
        {
            Destroy(gameObject); // Elimina este objeto del juego
        }
    }

    // M�todo que se ejecuta autom�ticamente cuando la bala entra en contacto con otro collider
    void OnTriggerEnter(Collider collider)
{
    if (collider.gameObject.CompareTag("Enemy"))
    {
        GameObject enemy = collider.gameObject;

        if (enemy != null)
        {
            // Desactivar primero para que Cardboard deje de interactuar con él
            enemy.SetActive(false);

            // Destruir un poco después
            Destroy(enemy, 0.1f);
        }

        // Destruir la bala
        Destroy(gameObject);
    }
}

}
