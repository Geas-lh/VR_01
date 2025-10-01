using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float ballProximity = 4f;
    public Animator racketAnimator;  // Asignar en el inspector

    void Start()
    {
        Ball ball = FindObjectOfType<Ball>();
        if (ball != null)
        {
            // No más referencia al animator en Ball si no lo vas a usar desde ahí
        }
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Ball ball = hit.transform.GetComponent<Ball>();

            if (ball != null)
            {
                // ✅ Condición: la pelota está cerca y VIENE hacia el jugador
                if (ball.transform.position.z - transform.position.z < ballProximity && ball.direction.z < 0)
                {
                    // ✅ Reproducir animación antes de que la dirección cambie
                    if (racketAnimator != null)
                    {
                        racketAnimator.SetTrigger("Hit");
                    }

                    // ✅ Ahora sí invierte la dirección
                    ball.OnPlayerHit();
                }
            }
        }
    }
}
