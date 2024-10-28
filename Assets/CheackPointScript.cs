using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheackPointScript : MonoBehaviour
{
    private RespawnScript respawn;

    void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            respawn.ChangeRespawnPointPosition(gameObject.transform);
        }
    }
}
