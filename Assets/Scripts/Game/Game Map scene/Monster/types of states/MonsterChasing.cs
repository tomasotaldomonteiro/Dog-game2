using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChasing : AStateBehaviour
{
    [SerializeField] public bool isFacingRight;
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float maxTimer = 5.0f; // Start timer at 5 seconds
    
    private SpriteRenderer spriteRenderer;
    private LineOfSight monsterSawPlayer = null;
    private ToxicCollisionDetector monsterTouchedToxic = null;
    private float currentTimer;
    public bool timerReachedZero { get; private set; }
    private Animator animator;
    private Vector2  previousPosition;
    
    public override bool InitializeState()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();    
        monsterSawPlayer = GetComponent<LineOfSight>();
        monsterTouchedToxic = GetComponent<ToxicCollisionDetector>();
        animator = GetComponent<Animator>();

        // Check if necessary components are assigned
        if (playerTransform == null && monsterTouchedToxic !=null && monsterSawPlayer == null)
            return false;

        return true;
    }

    public override void OnStateStart()
    {
        currentTimer = maxTimer;
        timerReachedZero = false;
        spriteRenderer.color = Color.red;
        
        previousPosition = transform.position; // Initialize previous position
    }

    public override void OnStateUpdate()
    {
        animator.SetBool("Idle", false); 
        animator.SetBool("Patrolling", false); 
        animator.SetBool("Chasing", true); 
        
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
            
            Vector2 currentPosition = transform.position; // Get the current position

            // Check direction based on movement
            if (currentPosition.x > previousPosition.x)
            {
                Debug.Log("Moving right");
                transform.localScale = new Vector3(1, 1, 1); // Face right
            }
            else if (currentPosition.x < previousPosition.x)
            {
                Debug.Log("Moving left");
                transform.localScale = new Vector3(-1, 1, 1); // Face left
            }

            previousPosition = currentPosition; // Update previous position


            // Move towards the player
            if (transform.position.x > playerTransform.position.x)
            {
          
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                
            }
            else if (transform.position.x < playerTransform.position.x)
            {
                
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                
            }
        }
    }

    public override void OnStateEnd() {
    }

    public override int StateTransitionCondition() {
        
        if (timerReachedZero) {
            
            return (int)EShowcaseMonsterStates.Patrolling;
        }
        if (!monsterSawPlayer.HasSeenPlayerThisFrame()) {
            
            return (int)EShowcaseMonsterStates.Lost;
        }
        if (monsterTouchedToxic.HasTouchedToxicThisFrame()) {
            
            return (int)EShowcaseMonsterStates.Death; 
        }
        
        return (int)EShowcaseMonsterStates.Invalid;
    }
}