using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private float bounce = 30f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // Only activate bounce if the jump button is NOT currently held
            if (player != null && !Input.GetButton("Jump"))
            {
                player.isBouncingFromJumpPad = true;
                player.DisableJumpForBounce(0.2f); 

                Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0); 
                playerRigidbody.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.isBouncingFromJumpPad = false;
            }
        }
    }
}
