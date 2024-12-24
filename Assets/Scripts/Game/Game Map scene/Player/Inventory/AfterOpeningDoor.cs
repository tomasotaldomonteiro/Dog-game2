using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterOpeningDoor : MonoBehaviour {
    
    private Inventory inventory;
    public int i;
    private void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void Update() {
        if (transform.childCount <= 0) {
            inventory.isFull[i] = false;
        }
    }
    public void UseItem() {
        
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}

