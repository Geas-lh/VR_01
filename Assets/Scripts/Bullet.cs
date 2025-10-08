using UnityEngine;                 // Importa las librerías necesarias de Unity
using System.Collections;

public class Bullet : MonoBehaviour // Define una clase llamada "Bullet" que hereda de MonoBehaviour
{
    public float speed = 1f;         // Velocidad de la bala (puede ser ajustada desde el inspector)
    public Vector3 direction;        // Dirección hacia donde se moverá la bala (se puede establecer desde otro script)

    private float lifetime = 2f;     // Tiempo de vida de la bala en segundos antes de que se destruya automáticamente

    // Método Start: se ejecuta al iniciar el objeto
    void Start()
    {
        // En este caso no se hace nada al iniciar
    }

    // Método Update: se ejecuta una vez por cada frame
    void Update()
    {
        // Mueve la bala en la dirección especificada a la velocidad definida, teniendo en cuenta el tiempo entre frames
        transform.position += direction * speed * Time.deltaTime;

        // Resta al tiempo de vida el tiempo transcurrido desde el último frame
        lifetime -= Time.deltaTime;

        // Si el tiempo de vida ha llegado a cero o menos, destruye el objeto (la bala)
        if (lifetime <= 0)
        {
            Destroy(gameObject); // Elimina este objeto del juego
        }
    }

    // Método que se ejecuta automáticamente cuando la bala entra en contacto con otro collider
    void OnTriggerEnter(Collider collider)
    {
        // Verifica si el objeto con el que colisionó tiene la etiqueta "Enemy"
        if (collider.gameObject.tag == "Enemy")
        {
            // Destruye al enemigo
            Destroy(collider.gameObject);

            // Destruye la bala también
            Destroy(gameObject);
        }
    }
}
