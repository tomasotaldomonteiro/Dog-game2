using UnityEngine;

public class StopSlidingOnCollision : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // When the player collides with the rat
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;      // Stop sliding
            rb.angularVelocity = 0f;         // Stop rotation

            Debug.Log("Player touched rat");
        }
    }
    
    
    // When the player stays colliding with the rat
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;      // Stop horizontal/vertical sliding
            rb.angularVelocity = 0f;         // Stop rotational movement
        }
    }
}