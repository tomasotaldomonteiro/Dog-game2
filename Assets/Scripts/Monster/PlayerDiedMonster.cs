using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedMonster : MonoBehaviour
{
    [SerializeField] private RespawnScript respawn;

    void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnScript>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            respawn.Respawn();
        }
    }
}
