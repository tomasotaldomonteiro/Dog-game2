using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCollectible : MonoBehaviour
{
    public bool[] collectibleIsFull;
    public GameObject[] collectibleSlots;
    
    private void Start()
    {
        // Initialize inventory slots as empty
        collectibleIsFull = new bool[collectibleSlots.Length];
        for (int i = 0; i < collectibleIsFull.Length; i++)
        {
            collectibleIsFull[i] = false;
        }
    }
}