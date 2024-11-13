using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public virtual void Interact(GameObject instigator)
    {
        Debug.Log("Interacted Interface Not Implemented" + instigator);
    }
}