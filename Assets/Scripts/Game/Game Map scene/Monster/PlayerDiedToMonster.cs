using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedToMonster : MonoBehaviour {
    
    [SerializeField] private RespawnScript respawn;
    [SerializeField] private PlayerDiedAnimation dogAnimations;
    [SerializeField] private LineOfSight ratLineOfSight; 
    private Shake shake;
    private Collider2D playerCollider;
    
    void Awake() {
        
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        respawn = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnScript>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("Player")) {
            
            playerCollider = other.collider;
            StartCoroutine(HandleDeathAndRespawn(playerCollider));
        }
        
    }
    private IEnumerator HandleDeathAndRespawn(Collider2D playerCollider) {
        
        shake.ShakeCamera();
        
        // disable collisions between player and rat
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), true); 
        ratLineOfSight.TemporarilyDisableSight(true);

        // Wait for the animation coroutine to finish
        yield return StartCoroutine(dogAnimations.PlayDeathAnimationOne()); 
        
        // Re-enable collisions after the animation is done
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), false); 
        ratLineOfSight.TemporarilyDisableSight(false);
        
        yield return StartCoroutine(respawn.Respawn());
    }
}
