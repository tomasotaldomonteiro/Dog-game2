using UnityEngine;

public class StopSlidingOnCollision : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;      // Stop horizontal/vertical sliding
            rb.angularVelocity = 0f;         // Stop rotational movement
        }
    }
}