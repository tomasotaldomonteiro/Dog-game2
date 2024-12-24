using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable 
{
    protected bool animateDoor = false;
    private void Update()
    {
        if (animateDoor)
        {
            
        }
    }

    public virtual void Interact(GameObject instigator)
    {
        Debug.Log("Dorr Interacted");
        animateDoor = true;
    }
   
}