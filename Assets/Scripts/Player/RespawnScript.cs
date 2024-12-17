using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public void ChangeRespawnPointPosition(Transform newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        gameObject.transform.position = respawnPoint.position;
    }
}
