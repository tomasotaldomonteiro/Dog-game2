using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLost : AStateBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f; // Speed of movement
    [SerializeField] private float maxTimer = 5.0f; // Time to remain in "lost" state if no other action occurs

    
    private SpriteRenderer spriteRenderer;
    private LineOfSight monsterSawPlayer;
    private Animator animator;
    private Vector2 lastSeenPosition; // Position where the player was last seen
    private bool reachedLastSeenPosition;
    private float currentTimer;

    public override bool InitializeState()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        monsterSawPlayer = GetComponent<LineOfSight>();
        animator = GetComponent<Animator>();
        return true;
    }

    public override void OnStateStart()
    {
        // Set the last seen position of the player
        lastSeenPosition = monsterSawPlayer.GetLastSeenPosition();

        // Initialize the state
        animator.SetBool("Idle", false);
        animator.SetBool("Patrolling", true);
        animator.SetBool("Chasing", false);
        //animator.SetBool("Lost", true);

        currentTimer = maxTimer;
        reachedLastSeenPosition = false;
        spriteRenderer.color = Color.yellow;
    }

    public override void OnStateUpdate()
    {
        // If the timer runs out, transition to idle
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0)
            {
                currentTimer = 0;
                reachedLastSeenPosition = true; // Treat this as reaching the position to transition to Idle
            }
        }

        // Move toward the last seen position of the player
        if (!reachedLastSeenPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastSeenPosition, moveSpeed * Time.deltaTime);

            // Check if the monster has reached the last seen position
            if (Vector2.Distance(transform.position, lastSeenPosition) < 0.1f)
            {
                reachedLastSeenPosition = true;
            }
        }
    }

    public override void OnStateEnd()
    {
        // Reset any necessary variables when the state ends
    }

    public override int StateTransitionCondition()
    {
        // Transition to Idle when the last seen position is reached
        if (reachedLastSeenPosition)
        {
            return (int)EShowcaseMonsterStates.Idle;
        }

        // Transition to Chasing if the monster sees the player again
        if (monsterSawPlayer.HasSeenPlayerThisFrame())
        {
            return (int)EShowcaseMonsterStates.Chasing;
        }

        // Stay in the Lost state if no other conditions are met
        return (int)EShowcaseMonsterStates.Invalid;
    }
}
