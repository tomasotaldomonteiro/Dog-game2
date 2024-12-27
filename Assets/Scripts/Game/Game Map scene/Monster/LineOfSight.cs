using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private Vector2 sightBoxSize = new Vector2(7f, 1.5f); // Width and height of the sight rectangle
    [SerializeField] private float sightOffsetBehind = 2f; // How far behind the character the sight extends
    [SerializeField] private LayerMask playerLayer; // Layer for the player

    private bool hasSeenPlayerThisFrame = false;
    private bool isTemporarilyDisabled = false; 
    
    private Vector2 lastSeenPosition;
    private Vector2 boxCenter; // Store the calculated box center for dynamic Gizmos

    private float facingDirection = 1f;
    
    private void Update()
    {
        if (isTemporarilyDisabled) return;
        
        if (transform.localScale.x > 0)
            facingDirection = 1f; // Facing right
        else
            facingDirection = -1f; // Facing left
        
        // Define the center of the box based on the character's facing direction
        boxCenter = (Vector2)transform.position + new Vector2((sightBoxSize.x / 2 - sightOffsetBehind)* facingDirection, 0);

        // Check if the player is within the box
        Collider2D hit = Physics2D.OverlapBox(boxCenter, sightBoxSize, 0f, playerLayer);
        if (hit != null && hit.transform == playerTransform)
        {
            hasSeenPlayerThisFrame = true;
            lastSeenPosition = playerTransform.position;
            return;
        }
        

        hasSeenPlayerThisFrame = false;
    }
    
    public void TemporarilyDisableSight(bool disable) // Add a public method
    {
        hasSeenPlayerThisFrame = false;
        isTemporarilyDisabled = disable;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the sight rectangle in the editor dynamically
        Gizmos.color = hasSeenPlayerThisFrame ? Color.red : Color.blue; // Red if player detected, blue otherwise
        Gizmos.DrawWireCube(boxCenter, sightBoxSize);

        // Optional: Show the last seen position as a small sphere
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lastSeenPosition, 0.2f);
    }

    public bool HasSeenPlayerThisFrame()
    {
        return hasSeenPlayerThisFrame;
    }

    public Vector2 GetLastSeenPosition()
    {
        return lastSeenPosition;
    }
}