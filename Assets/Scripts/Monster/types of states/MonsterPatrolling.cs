using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrolling : AStateBehaviour {
    
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private bool fromAtoB = true;
    [SerializeField] private float moveSpeed;
    
    private SpriteRenderer spriteRenderer;
    
    private LineOfSight monsterSawPlayer = null;
    
    [SerializeField] private float maxTimer = 5.0f; // Start timer at 5 seconds
    private float currentTimer;
    public bool timerReachedZero { get; private set; }
    

    public override bool InitializeState()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterSawPlayer = GetComponent<LineOfSight>();
        return true;
    }

    public override void OnStateStart()
    {
        currentTimer = maxTimer;
        timerReachedZero = false;
        Debug.Log("Patrol state started");
        spriteRenderer.color = Color.red; // Change color to indicate patrol state
    }

    public override void OnStateUpdate()
    {
        Debug.Log("Patrol state update");
        Debug.Log(fromAtoB);
        
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0)
            {
                currentTimer = 0; // Stop at 0
                timerReachedZero = true; // Set boolean to true when timer reaches 0
            }
        }
        
        if (fromAtoB)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.transform.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointB.transform.position) < 0.1f)
            {
                // Flip the sprite to face left
                transform.localScale = new Vector3(-1, 1, 1);
                fromAtoB = false; // Switch direction to move towards point A
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA.transform.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointA.transform.position) < 0.1f)
            {
                // Flip the sprite to face right
                transform.localScale = new Vector3(1, 1, 1);
                fromAtoB = true; // Switch direction to move towards point B
            }
        }
    }

    public override void OnStateEnd()
    {
        // Cleanup or reset logic when the state ends, if needed
    }

    public override int StateTransitionCondition()
    {
        if (GetComponent<MonsterPatrolling>().timerReachedZero)
        {
            return (int)EShowcaseMonsterStates.Idle;
        }

        if (monsterSawPlayer.HasSeenPlayerThisFrame())
        {
            return (int)EShowcaseMonsterStates.Chasing;
        }

        return (int)EShowcaseMonsterStates.Invalid;
    }

}