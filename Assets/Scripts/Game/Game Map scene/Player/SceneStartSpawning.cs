using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartSpawning : MonoBehaviour
{                         
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody2D rb;
    
    private float firstspawningAnimationDuration;    
    private bool isXMovementFrozen = false; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips){
            if (clip.name == "FirstSpawn"){
                firstspawningAnimationDuration = clip.length;            // duration of the spawning animation
                break;
            }
        }
    
        if (SceneManager.GetActiveScene().buildIndex == 1)                  // Manually trigger the spawning logic if the current scene is Scene 1
        {
            StartScene1Spawning();
        }
    }

    private void StartScene1Spawning()
    {
        StartCoroutine(SpawningSequence());
    }

    private IEnumerator SpawningSequence()
    {
        
        playerController.enabled = false; 
        
        // Stop sliding
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        
        animator.SetBool("Move", false);
        
        yield return new WaitForSeconds(firstspawningAnimationDuration);
        
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
}