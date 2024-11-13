using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDoor : Door
{
    float Scale = 1.0f;
    float Timer = 0f;

    private GameObject[] itemSlot;


    void Start() {
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
                itemSlot[i].transform.GetComponent<SlotDelete>().DropItem();
                break; // Stop after finding and dropping the green key
            }
        }
    }

    private void OpenDoor() {
        transform.localScale = Vector3.zero; // Or any other logic to "open" the door
        Debug.Log("Door opened using the green key!");
    }

    void Update() {
        if (animateDoor) {
            Scale = Mathf.Clamp01(Scale -= Time.deltaTime);
            transform.localScale = Vector3.one * Scale;
            Timer += Time.deltaTime;
            transform.rotation = Quaternion.Euler( 720.0f * Timer * Vector3.one);
        }
    }

    public override void Interact(GameObject instigator) {
        Debug.Log("Interacting with the green door.");
        base.Interact(instigator);
    }
}


