using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDoor : Door {

    private GameObject[] itemSlot;
    private Animator animator;
    private BoxCollider2D doorCollider;

    void Start() {
        
        doorCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        itemSlot = GameObject.FindGameObjectsWithTag("itemSlot");
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        
        GameObject player = other.gameObject;

        // Check if the player has the green key
        if (player.CompareTag("Player") && HasGreenKey()) {
            
            OpenDoor();
            RemoveGreenKey();
        }
    }

    private bool HasGreenKey() {
        for (int i = 0; i < itemSlot.Length; ++i) {
            if (itemSlot[i].transform.childCount > 0 && itemSlot[i].transform.GetChild(0).CompareTag("GreenKey")) {
                return true;
            }
        }
        return false;
    }

    private void RemoveGreenKey() {
        for (int i = 0; i < itemSlot.Length; ++i) {
            if (itemSlot[i].transform.childCount > 0 && itemSlot[i].transform.GetChild(0).CompareTag("GreenKey")) {
                itemSlot[i].transform.GetComponent<AfterOpeningDoor>().UseItem();
                break; 
                // Stop after finding and deleting the green key
            }
        }
    }

    private void OpenDoor() {
        
        animateDoor = true;
        doorCollider.enabled = false;
        
        Debug.Log("Door opened using the green key!");
    }

    void Update() {
        if (animateDoor) {
            animator.SetTrigger("OpeningDoor"); 
        }
    }

    public override void Interact(GameObject instigator) {
        Debug.Log("Interacting with the green door.");
        base.Interact(instigator);
    }
}