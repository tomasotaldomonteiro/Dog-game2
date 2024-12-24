using System.Collections;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private GameObject currentTeleporter;
    private Fading fadeManager;
    private bool isTeleporting = false; // Prevents movement during teleportation
    private PlayerController playerMovement; 
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        fadeManager = FindObjectOfType<Fading>();
        playerMovement = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentTeleporter != null && !isTeleporting)
        {
            StartCoroutine(TeleportWithFade());
        }
    }

    private IEnumerator TeleportWithFade()
    {
        // Disable movement
        isTeleporting = true; 
        if (playerMovement != null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            playerMovement.enabled = false; // disable entire movement script
            animator.SetBool("Move", false); 
        }

        fadeManager.StartFadeOut();
        yield return new WaitForSeconds(fadeManager.fadeDuration); // Wait for fade-out to complete

        transform.position = currentTeleporter.GetComponent<Teleport>().GetDestination();

        fadeManager.StartFadeIn();
        yield return new WaitForSeconds(fadeManager.fadeDuration); // Wait for fade-in to complete

        // Enable movement
        isTeleporting = false; 
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleportor"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleportor") && collision.gameObject == currentTeleporter)
        {
            currentTeleporter = null;
        }
    }
}