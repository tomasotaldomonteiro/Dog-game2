using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] private Transform currentRespawnPoint;
    private Rigidbody2D rb; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Method to update the respawn point
    public void ChangeRespawnPointPosition(Transform newRespawnPoint)
    {
        currentRespawnPoint = newRespawnPoint;
    }

    // Method to handle respawning the player
    public void Respawn()
    {
        if (currentRespawnPoint != null)
        {
            // Move the player to the respawn point
            transform.position = currentRespawnPoint.position;

            // Reset velocity to stop momentum
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f; 
            }
        }
        else
        {
            Debug.LogWarning("No respawn point set! RIP");
        }
    }
}