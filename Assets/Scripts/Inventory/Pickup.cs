using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject item;
    private bool isInRange; // Flag to check if player is in range of the item

    private void Start() {
        
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        // Player is in range of the item
        if (other.CompareTag("Player")) {
            isInRange = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        // Player has left the range of the item
        if (other.CompareTag("Player")) {
            isInRange = false; 
        }
    }
    
    private void Update() {
        
        // Check if player presses "E" while in range
        if (isInRange && Input.GetKeyDown(KeyCode.E)) {
            PickUpItem();
        }
    }
    
    private void PickUpItem() {
        
        for (int i = 0; i < inventory.slots.Length; i++) { 
            
            if (inventory.isFull[i] == false) {
                
                //pickup then is full
                inventory.isFull[i] = true;
                    
                // Instantiate item as a child of the i slot with its world position rather than matching exactly with the slot’s initial position
                GameObject newItem = Instantiate(item, inventory.slots[i].transform, false);
                
                // Ensure the item centers in the UI elements transform component slot
                RectTransform rectTransform = newItem.GetComponent<RectTransform>();
                    
                //anchoredPosition is a property of the RectTransform that represents the position of the UI element relative to its anchor points in its parent.
                //By setting anchoredPosition to (0, 0), you center the newItemButton within the slot, aligning it perfectly in the middle
                rectTransform.anchoredPosition = Vector2.zero;
                
                // Destroy the picked-up item
                Destroy(gameObject); 
                Destroy(transform.parent.gameObject);
                break;
            }
        }
    }
}
