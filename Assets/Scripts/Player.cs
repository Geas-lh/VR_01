using UnityEngine;

public class Player : MonoBehaviour
{
    public float ballProximity = 4f;
    public Animator animator;

    public float hitCooldown = 0.5f; // Tiempo mínimo entre animaciones
    private float lastHitTime = -Mathf.Infinity; // Última vez que se activó la animación

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Ball ball = hit.transform.GetComponent<Ball>();
            if (ball != null)
            {
                float zDistance = ball.transform.position.z - transform.position.z;

                // Condición para animación
                if (zDistance < ballProximity + 2 && ball.direction.z < 0)
                {
                    // Solo permite activar el trigger si ha pasado suficiente tiempo
                    if (Time.time - lastHitTime > hitCooldown)
                    {
                        animator.ResetTrigger("Hit");
                        animator.SetTrigger("Hit");
                        lastHitTime = Time.time;
                    }
                }

                // Condición para golpear la pelota
                if (zDistance <= ballProximity)
                {
                    ball.OnPlayerHit();
                    Debug.Log("Hit");
                }
                else
                {
                    Debug.Log("no cumple distancia: " + zDistance);
                    Debug.Log("ball.direction.z: " + ball.direction.z);
                }
            }
        }
    }
}
