using UnityEngine;
using System.Collections;


public class Ball : MonoBehaviour {

    public Player player;
    public Animator racketAnimator;  // Asignar desde Player o inspector
    public float speed = 2.5f;
    public float speedIncrement = 0.5f;
    public float deflectionDepth = 6f;
    public float deflectionRadius = 2f;
    public float rotatingSpeed = 50f;
    public Vector3 direction;

    public int score = 0;

    void Start () {
        if (player == null)
            player = FindObjectOfType<Player>();
    }
    
    void Update () {
        transform.position += direction.normalized * speed * Time.deltaTime;

        if (transform.position.z >= deflectionDepth && direction.z > 0) {
            float randomAngle = Random.Range(0f, 2 * Mathf.PI);

            Vector3 targetPosition = new Vector3(
                player.transform.position.x + Mathf.Cos(randomAngle) * deflectionRadius,
                player.transform.position.y + Mathf.Sin(randomAngle) * deflectionRadius,
                player.transform.position.z
            );

            direction = (targetPosition - transform.position).normalized;
        }

        // Rotating logic
        transform.RotateAround(transform.position, Vector3.up, rotatingSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.right, rotatingSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.forward, rotatingSpeed * Time.deltaTime);
    }

    public void OnPlayerHit()
    {
        if (player == null) return;

        if (transform.position.z > player.transform.position.z)
        {
            direction *= -1;
            speed += speedIncrement;
            score++;

        }
    }
        

}
