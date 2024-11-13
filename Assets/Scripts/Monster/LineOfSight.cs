using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private float chaseDistance = 5.0f; // Distance at which the monster starts chasing the player

    
    private bool hasSeenPlayerThisFrame = false;
    
    public void Update()
    {
        // Check if the player is within chase distance
        if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
        {
            hasSeenPlayerThisFrame = true;
            
        }
        hasSeenPlayerThisFrame = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the chase distance in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
    
    public bool HasSeenPlayerThisFrame()
    {
        return hasSeenPlayerThisFrame;
    }
}