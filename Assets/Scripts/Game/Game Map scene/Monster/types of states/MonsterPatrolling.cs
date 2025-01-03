using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrolling : AStateBehaviour {
    
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private bool fromAtoB = true;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxTimer = 5.0f; // Start timer at 5 seconds
    
    private SpriteRenderer spriteRenderer;
    private LineOfSight monsterSawPlayer = null;
    private ToxicCollisionDetector monsterTouchedToxic = null;
    private ExplosionDetection explosionDetection = null;
    private float currentTimer;
    public bool timerReachedZero { get; private set; }
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2  previousPosition;

    public override bool InitializeState()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterSawPlayer = GetComponent<LineOfSight>();
        monsterTouchedToxic = GetComponent<ToxicCollisionDetector>();
        explosionDetection = GetComponent<ExplosionDetection>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        return spriteRenderer != null && monsterSawPlayer != null && monsterTouchedToxic != null && animator != null;
    }

    public override void OnStateStart()
    {
        animator.SetBool("Idle", false); 
        animator.SetBool("Patrolling", true); 
        animator.SetBool("Chasing", false); 
        
        currentTimer = maxTimer;
        timerReachedZero = false;
        //spriteRenderer.color = Color.blue;
        
        previousPosition = transform.position; // Initialize previous position
    }

    public override void OnStateUpdate()
    {
        rb.angularVelocity = 0f;
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
            //Debug.Log("Moving right");
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }
        else if (currentPosition.x < previousPosition.x)
        {
            //Debug.Log("Moving left");
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }

        previousPosition = currentPosition; // Update previous position
        if (fromAtoB)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.transform.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointB.transform.position) < 0.3f)
            {
                fromAtoB = false; // Switch direction to move towards point A
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA.transform.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointA.transform.position) < 0.3f)
            {
                fromAtoB = true; // Switch direction to move towards point B
            }
        }
    }

    public override void OnStateEnd() {
     
    }

    public override int StateTransitionCondition() {
        
        if (timerReachedZero) {
            
            return (int)EShowcaseMonsterStates.Idle;
        }

        if (monsterSawPlayer.HasSeenPlayerThisFrame()) {
            
            return (int)EShowcaseMonsterStates.Chasing;
        }
        if (monsterTouchedToxic.HasTouchedToxicThisFrame()) {
            
            return (int)EShowcaseMonsterStates.Death; 
        }
        if (explosionDetection.HasExplodedThisFrame()) {
            
            return (int)EShowcaseMonsterStates.Exploded; 
        }
       

        return (int)EShowcaseMonsterStates.Invalid;
    }

}