using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterExplodedManager : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private float explosionBurnAnimationDuration;
    private bool animationFinished = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Retrieve burn animation duration
        if (animator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Exploded")
                {
                    explosionBurnAnimationDuration = clip.length;
                    break;
                }
            }
        }
    }

    public void StartExplodedAnimation()
    {
        if (animator == null || rb == null || col == null)
        {
            Debug.LogError("MonsterDiesManager is missing essential components.");
            return;
        }

        // Trigger the burn animation
        animator.SetTrigger("Exploded");

        // Stop physics and disable the collider
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        // Start the coroutine to wait for the animation to finish
        StartCoroutine(WaitForExplodedAnimation());
    }

    private IEnumerator WaitForExplodedAnimation()
    {
        yield return new WaitForSeconds(explosionBurnAnimationDuration);
        animationFinished = true;
        DestroyParentAndAllChildren();
    }

    public bool IsExplodedAnimationFinished()
    {
        return animationFinished;
    }

    private void DestroyParentAndAllChildren()
    {
        // Destroy the parent and all its children
        Transform parent = transform.parent;

        if (parent != null)
        {
            Destroy(parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
