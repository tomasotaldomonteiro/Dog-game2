using System.Collections;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    private Transform respawnPoint;
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody2D rb;

    private float spawningAnimationDuration; // Store the spawning animation duration
    private bool isXMovementFrozen = false; // Track if x-axis movement is frozen

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        // Find the duration of the spawning animation
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Spawning")
            {
                spawningAnimationDuration = clip.length;
                break;
            }
        }
    }

    public IEnumerator Respawn()
    {
        animator.SetTrigger("Spawning");
        playerController.enabled = false;

        transform.position = respawnPoint.position;

        // Stop sliding
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        animator.SetBool("Move", false);

        yield return new WaitForSeconds(spawningAnimationDuration); // Wait for the animation duration

        // Freeze x-axis movement
        isXMovementFrozen = true;

        yield return new WaitForSeconds(0.3f); // Wait for the additional 0.3 seconds

        // Unfreeze x-axis movement and re-enable player control
        isXMovementFrozen = false;
        playerController.enabled = true;
    }

    void FixedUpdate()
    {
        if (isXMovementFrozen)
        {
            // Prevent x-axis movement by zeroing out x velocity
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    public void ChangeRespawnPointPosition(Transform newPosition)
    {
        respawnPoint = newPosition;
    }
}