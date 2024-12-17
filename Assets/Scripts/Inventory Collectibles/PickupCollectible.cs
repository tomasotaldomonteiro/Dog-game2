using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollectible : MonoBehaviour
{
    private InventoryCollectible cinventory;
    public GameObject citem;
    private bool cisInRange; // Flag to check if player is in range of the item
    public static bool Endingtwo = false;

    private void Start() {
        
        cinventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryCollectible>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        // Player is in range of the item
        if (other.CompareTag("Player")) {
            cisInRange = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        // Player has left the range of the item
        if (other.CompareTag("Player")) {
            cisInRange = false; 
        }
    }
    
    private void Update() {
        
        // Check if player presses "E" while in range
        if (cisInRange && Input.GetKeyDown(KeyCode.E)) {
            CPickUpItem();
        }
    }
    
    private void CPickUpItem() {
        
        for (int i = 0; i < cinventory.collectibleSlots.Length; i++) { 
            
            if (cinventory.collectibleIsFull[i] == false) {
                
                //pickup then is full
                cinventory.collectibleIsFull[i] = true;
                    
                // Instantiate item as a child of the i slot with its world position rather than matching exactly with the slot’s initial position
                GameObject newItem = Instantiate(citem, cinventory.collectibleSlots[i].transform, false);
                
                // Ensure the item centers in the UI elements transform component slot
                RectTransform rectTransform = newItem.GetComponent<RectTransform>();
                    
                //anchoredPosition is a property of the RectTransform that represents the position of the UI element relative to its anchor points in its parent.
                //By setting anchoredPosition to (0, 0), you center the newItemButton within the slot, aligning it perfectly in the middle
                rectTransform.anchoredPosition = Vector2.zero;
                
                // Destroy the picked-up item
                Destroy(gameObject); 
                
                Finish();
                break;
            }
        }
    }

    public void Finish()
    {
        Endingtwo = true; // Assume ending two is true by default
        for (int i = 0; i < cinventory.collectibleSlots.Length; i++)
        {
            if (!cinventory.collectibleIsFull[i])
            {
                Endingtwo = false; // If any slot is not full, set Endingtwo to false
                break;
            }
        }
        Debug.Log("Endingtwo is now: " + Endingtwo);
    }
}
