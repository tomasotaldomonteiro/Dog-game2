using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollectible : MonoBehaviour
{
    private InventoryCollectible cinventory;
    public GameObject citem;
    public int collectibleID; // Unique ID for the collectible
    private bool cisInRange; // Flag to check if player is in range of the item
    public static bool Endingtwo = false;

    private void Start()
    {
        cinventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryCollectible>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player is in range of the item
        if (other.CompareTag("Player"))
        {
            cisInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Player has left the range of the item
        if (other.CompareTag("Player"))
        {
            cisInRange = false;
        }
    }

    private void Update()
    {
        // Check if player presses "E" while in range
        if (cisInRange && Input.GetKeyDown(KeyCode.E))
        {
            CPickUpItem();
        }
    }

    private void CPickUpItem()
    {
        if (collectibleID >= 0 && collectibleID < cinventory.collectibleSlots.Length)
        {
            int slotIndex = collectibleID;

            // Check if the corresponding slot is empty
            if (!cinventory.collectibleIsFull[slotIndex])
            {
                // Mark slot as full
                cinventory.collectibleIsFull[slotIndex] = true;

                // Instantiate item in the correct slot
                GameObject newItem = Instantiate(citem, cinventory.collectibleSlots[slotIndex].transform, false);

                // Center the item in the slot
                RectTransform rectTransform = newItem.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = Vector2.zero;

                // Destroy the picked-up item in the world
                Destroy(gameObject);

                // Check for the ending condition
                Finish();
            }
            else
            {
                Debug.Log("Slot " + slotIndex + " is already full.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid collectible ID: " + collectibleID);
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
