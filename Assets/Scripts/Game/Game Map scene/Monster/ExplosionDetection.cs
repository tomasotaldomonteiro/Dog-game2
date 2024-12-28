using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDetection : MonoBehaviour
{
    private bool hasExplodedThisFrame = false;

    public void OnExplosion()
    {
        hasExplodedThisFrame = true;
        Debug.Log($"{gameObject.name} was caught in an explosion!");

        // Optional: Trigger visual effects, animations, or sound here if needed
    }

    private void LateUpdate()
    {
        // Reset the flag at the end of each frame
        hasExplodedThisFrame = false;
    }

    public bool HasExplodedThisFrame()
    {
        return hasExplodedThisFrame;
    }
}