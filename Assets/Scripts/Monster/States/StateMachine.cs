using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private List<AStateBehaviour> stateBehaviours = new List<AStateBehaviour>();
    [SerializeField] private int defaultState = 0;
    private AStateBehaviour currentState = null;
    bool InitializeStates()
    {
        for (int i = 0; i < stateBehaviours.Count; ++i)
        {
            AStateBehaviour stateBehaviour = stateBehaviours[i];
            Debug.Log("1st state" + stateBehaviour);
            if (stateBehaviour && stateBehaviour.InitializeState())
            {
                stateBehaviour.AssociatedStateMachine = this;
                continue;
            }
    
            Debug.Log($"StateMachine On {gameObject.name} has failed to initalize the state {stateBehaviours[i]?.GetType().Name}!");
            return false;
        }
    
        return true;
    }
    
    void Start()
    {
        if (!InitializeStates())
        {
            // Stop This class from executing
            this.enabled = false;
            return;
        }
        
        if (stateBehaviours.Count > 0)
        {
            int firstStateIndex = defaultState < stateBehaviours.Count ? defaultState : 0;
    
            currentState = stateBehaviours[firstStateIndex];
            currentState.OnStateStart();
        }
        else
        {
            Debug.Log($"StateMachine On {gameObject.name} is has no state behaviours associated with it!");
        }
    }
    
    void Update()
    {
        
        currentState.OnStateUpdate();
        
        int newState = currentState.StateTransitionCondition();
        if (IsValidNewStateIndex(newState))
        {
            currentState.OnStateEnd();
            currentState = stateBehaviours[newState];
            currentState.OnStateStart();
        }
        
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     Debug.Log(currentState);
        // }
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     currentState.OnStateEnd();
        //     currentState = stateBehaviours[0];
        //     currentState.OnStateStart();
        // }
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     currentState.OnStateEnd();
        //     currentState = stateBehaviours[1];
        //     currentState.OnStateStart();
        //     currentState.OnStateUpdate();
        // }
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     currentState.OnStateEnd();
        //     currentState = stateBehaviours[2];
        //     currentState.OnStateStart();
        // }
        //
        //
        
        
        
       
    
        
    }
    
    public bool IsCurrentState(AStateBehaviour stateBehaviour)
    {
        return currentState == stateBehaviour;
    }
    
    public void SetState(int index)
    {
        if (IsValidNewStateIndex(index))
        {
            currentState.OnStateEnd();
            currentState = stateBehaviours[index];
            currentState.OnStateStart();
        }
    }
    
    private bool IsValidNewStateIndex(int stateIndex)
    {
        return stateIndex < stateBehaviours.Count && stateIndex >= 0;
    }
    
    public AStateBehaviour GetCurrentState()
    {
        return currentState;
    }
}
