using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPlayer : MonoBehaviour {
    
    [SerializeField] private float explosionRadius = 5f; 
    [SerializeField] private float explosionForce = 7f; 
    [SerializeField] private LayerMask playerMask; 

    public void triggerExplosion() {
        
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, explosionRadius, playerMask);
        
        if (playerCollider != null) {
            
            Rigidbody2D playerRigidbody = playerCollider.GetComponent<Rigidbody2D>();

            if (playerRigidbody != null) {
                
                // Calculate direction and apply force
                Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
                playerRigidbody.velocity = direction * explosionForce;

                
                PlayerController playerController = playerCollider.GetComponent<PlayerController>();
                
                if (playerController != null) {
                    
                    playerController.atHighSpeeds = true;
                }

                Debug.Log("Explosion applied to player with velocity: " + playerRigidbody.velocity);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}