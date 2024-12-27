using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicCollisionDetector : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask toxicLayer; // Layer for toxic tiles

    private bool hasTouchedToxicThisFrame = false; // If the object touched a toxic tile in this frame
    private Vector2 detectionBoxSize; // Size of the detection area
    private Vector2 boxCenter; // The calculated box center for the detection area

    
    private void Awake() {
        
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    private void Update()
    {
        if (boxCollider == null) return;

        // Set the detection area based on the BoxCollider2D's size and offset
        detectionBoxSize = boxCollider.size;
        boxCenter = (Vector2)transform.position + boxCollider.offset;

        // Check for toxic collision
        Collider2D hit = Physics2D.OverlapBox(boxCenter, detectionBoxSize, 0f, toxicLayer);
        if (hit != null)
        {
            hasTouchedToxicThisFrame = true;
            return;
        }

        hasTouchedToxicThisFrame = false;
    }

    public bool HasTouchedToxicThisFrame()
    {
        return hasTouchedToxicThisFrame;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection area in the editor
        Gizmos.color = hasTouchedToxicThisFrame ? Color.red : Color.blue; // Red if toxic detected, blue otherwise
        Gizmos.DrawWireCube(boxCenter, detectionBoxSize);
    }
}