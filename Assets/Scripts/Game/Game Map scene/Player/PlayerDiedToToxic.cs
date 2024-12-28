using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedToToxic : MonoBehaviour {
    
    [SerializeField] private PlayerDiedAnimation dogAnimations;
    [SerializeField] private LineOfSight ratLineOfSight; 
    private Shake shake;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    
    void Awake() {
        
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
    
        if(other.gameObject.CompareTag("Player")) {
            
            playerCollider = other.collider;
            StartCoroutine(HandleDeathAndRespawn(playerCollider));
        }
        
    }
    private IEnumerator HandleDeathAndRespawn(Collider2D playerCollider)
    {
        shake.ShakeCamera();
        
        // disable collisions between player and rat
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), true); 
        ratLineOfSight.TemporarilyDisableSight(true);
        
        rb.velocity = Vector2.zero; // Stop all movement
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        // Wait for the animation coroutine to finish
        yield return StartCoroutine(dogAnimations.PlayDeathAnimationTwo()); 
        
        
        // Re-enable collisions after the animation is done
        rb.bodyType = RigidbodyType2D.Dynamic; 
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), false); 
        ratLineOfSight.TemporarilyDisableSight(false);
        
    }
}