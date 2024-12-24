using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;

    public Vector3 GetDestination()
    {
        // Return the destination position with a Z offset of 0.5
        return new Vector3(destination.position.x, destination.position.y, 0.0f);
    }
}
