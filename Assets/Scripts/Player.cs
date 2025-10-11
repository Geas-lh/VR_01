using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

public class player : MonoBehaviour
{
    public bool menu = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // M�todo llamado cuando otro objeto colisiona con este (probablemente el jugador)
    void OnTriggerEnter(Collider collider)
    {

        // Si el objeto que colision� tiene la etiqueta "Enemy"
        if (collider.CompareTag("Enemy"))
        {
            // Llamar a la funci�n que maneja el fin del juego o men� principal
            GameController.instance.menu_principal();

        }
    }


}
