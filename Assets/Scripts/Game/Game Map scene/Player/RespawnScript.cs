using System.Collections;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    private Transform respawnPoint;
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody2D rb;

    private float spawningAnimationDuration;                        // Store the spawning animation duration
    private bool isXMovementFrozen = false;                         // Track if x-axis movement is frozen

    void Awake(){
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips){
            if (clip.name == "Spawning"){
                spawningAnimationDuration = clip.length;            // duration of the spawning animation
                break;
            }
        }
    }

    public IEnumerator Respawn(){
        
        if (respawnPoint == null) {
            
            Debug.LogWarning("Respawn point is not set! Defaulting to the player's current position.");
            respawnPoint = transform; // Default to current position
        }
        playerController.enabled = false;
        
        Vector3 respawnPosition = new Vector3(
            respawnPoint.position.x, 
            respawnPoint.position.y, 
            transform.position.z                                    // Keep the Z position unchanged
        );
        
        transform.position = respawnPosition;

        // Stop sliding
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        animator.SetBool("Move", false);

        yield return new WaitForSeconds(spawningAnimationDuration); // Wait for the animation duration
        
        isXMovementFrozen = true;                                   // Freeze x-axis movement
        yield return new WaitForSeconds(0.3f);                      // Wait for the additional 0.3 seconds

                                
        isXMovementFrozen = false;                                  // Unfreeze x-axis movement and re-enable player control
        playerController.enabled = true;
    }

    void FixedUpdate(){
        
        if (isXMovementFrozen){
            
            rb.velocity = new Vector2(0f, rb.velocity.y);           // Prevent x-axis movement by zeroing out x velocity
        }
    }

    public void ChangeRespawnPointPosition(Transform newPosition) {
        
        respawnPoint = newPosition;
    }
}