using System.Collections;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private GameObject currentTeleporter;
    private Fading fadeManager;
    private bool isTeleporting = false; // Prevents movement during teleportation
    private PlayerController playerMovement; // Reference to the player's movement script
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        fadeManager = FindObjectOfType<Fading>();
        playerMovement = GetComponent<PlayerController>(); // Ensure you have a PlayerMovement script for controlling movement
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
        isTeleporting = true; // Disable movement
        if (playerMovement != null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            playerMovement.enabled = false; // Disable movement script
            animator.SetBool("Move", false); 
        }

        fadeManager.StartFadeOut();
        yield return new WaitForSeconds(fadeManager.fadeDuration); // Wait for fade-out to complete

        transform.position = currentTeleporter.GetComponent<Teleport>().GetDestination();

        fadeManager.StartFadeIn();
        yield return new WaitForSeconds(fadeManager.fadeDuration); // Wait for fade-in to complete

        isTeleporting = false; // Enable movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Re-enable movement script
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