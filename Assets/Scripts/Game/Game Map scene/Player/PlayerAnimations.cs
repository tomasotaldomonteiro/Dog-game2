using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedAnimation : MonoBehaviour {
    
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerController dogMovement;
    
    // Store the animation duration                           
    private float deathAnimationDuration;
    private float spawningAnimationDuration;

    void Start() {
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dogMovement = GetComponent<PlayerController>();
        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips) {
            
            if (clip.name == "Death") {
                deathAnimationDuration = clip.length;
                break;
                
            }
        }
    }
    
    public IEnumerator PlayDeathAnimation(){                         // Returns an IEnumerator for the coroutine
    
        animator.SetTrigger("Death");
        dogMovement.enabled = false;
        
        // Stop sliding
        rb.velocity = Vector2.zero;      
        rb.angularVelocity = 0f;  
        
        yield return new WaitForSeconds(deathAnimationDuration);         // Wait for the animation to finish
      
    }
}
