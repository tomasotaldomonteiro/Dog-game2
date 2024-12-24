using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject items;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnDroppedItem() {
        Vector2 playerPos = new Vector2(player.position.x + 1.0f, player.position.y);
        Instantiate(items, playerPos, Quaternion.identity);
    }
}
