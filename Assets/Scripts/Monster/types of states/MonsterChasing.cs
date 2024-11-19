using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChasing : AStateBehaviour
{
    
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float maxTimer = 5.0f; // Start timer at 5 seconds
    
    private SpriteRenderer spriteRenderer;
    private LineOfSight monsterSawPlayer = null;
    private float currentTimer;
    public bool timerReachedZero { get; private set; }

    
    public override bool InitializeState()
    {
        monsterSawPlayer = GetComponent<LineOfSight>();

        // Check if necessary components are assigned
        if (playerTransform == null || monsterSawPlayer == null)
            return false;

        return true;
    }

    public override void OnStateStart()
    {
        currentTimer = maxTimer;
        timerReachedZero = false;
        spriteRenderer.color = Color.red;
    }

    public override void OnStateUpdate()
    {
        // Check if the monster can see the player
        if (monsterSawPlayer.HasSeenPlayerThisFrame())
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                if (currentTimer <= 0)
                {
                    currentTimer = 0; // Stop at 0
                    timerReachedZero = true; // Set boolean to true when timer reaches 0
                }
            }

            // Move towards the player
            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1); // Face left
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
            else if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Face right
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
        }
    }

    public override void OnStateEnd()
    {
    }

    public override int StateTransitionCondition()
    {
        if (timerReachedZero)
        {
            return (int)EShowcaseMonsterStates.Patrolling;
        }
        return (int)EShowcaseMonsterStates.Invalid;
    }
}