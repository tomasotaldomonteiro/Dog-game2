using System.Collections;
using UnityEngine;

public class MonsterDiesManager : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private float burnAnimationDuration;
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
                if (clip.name == "Burned")
                {
                    burnAnimationDuration = clip.length;
                    break;
                }
            }
        }
    }

    public void StartBurnAnimation()
    {
        if (animator == null || rb == null || col == null)
        {
            Debug.LogError("MonsterDiesManager is missing essential components.");
            return;
        }

        // Trigger the burn animation
        animator.SetTrigger("Burned");

        // Stop physics and disable the collider
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        // Start the coroutine to wait for the animation to finish
        StartCoroutine(WaitForBurnAnimation());
    }

    private IEnumerator WaitForBurnAnimation()
    {
        yield return new WaitForSeconds(burnAnimationDuration);
        animationFinished = true;
        DestroyParentAndAllChildren();
    }

    public bool IsBurnAnimationFinished()
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
