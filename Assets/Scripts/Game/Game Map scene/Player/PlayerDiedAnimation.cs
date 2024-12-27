using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedAnimation : MonoBehaviour {
    
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerController dogMovement;
    
    // Store the animation duration                           
    private float death1AnimationDuration;
    private float death2AnimationDuration;
    private float spawningAnimationDuration;

    void Start() {
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dogMovement = GetComponent<PlayerController>();
        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips) {
            
            if (clip.name == "Death") {
                
                death1AnimationDuration = clip.length;
                
            }else if (clip.name == "ToxicDeath") {
                
                death2AnimationDuration = clip.length;
            }
        }
    }
    
    public IEnumerator PlayDeathAnimationOne(){                         // Returns an IEnumerator for the coroutine
    
        animator.SetTrigger("Death");
        dogMovement.enabled = false;
        
        // Stop sliding
        rb.velocity = Vector2.zero;      
        rb.angularVelocity = 0f;  
        
        yield return new WaitForSeconds(death1AnimationDuration);         // Wait for the animation to finish
      
    }
    public IEnumerator PlayDeathAnimationTwo(){                         // Returns an IEnumerator for the coroutine
    
        animator.SetTrigger("ToxicDeath");
        dogMovement.enabled = false;
        
        // Stop sliding
        rb.velocity = Vector2.zero;      
        rb.angularVelocity = 0f;  
        
        yield return new WaitForSeconds(death2AnimationDuration);         // Wait for the animation to finish
      
    }
}
