using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : AStateBehaviour {
    
    [SerializeField] private float maxTimer = 5.0f; // Start timer at 5 seconds
    
    private SpriteRenderer spriteRenderer;
    private LineOfSight monsterSawPlayer = null;
    private ToxicCollisionDetector monsterTouchedToxic = null;
    private float currentTimer;
    public bool timerReachedZero { get; private set; }
    private Animator animator;
    

    public override bool InitializeState() {
        
        spriteRenderer = GetComponent<SpriteRenderer>();    
        monsterSawPlayer = GetComponent<LineOfSight>();
        monsterTouchedToxic = GetComponent<ToxicCollisionDetector>();
        animator = GetComponent<Animator>();
        
        return monsterSawPlayer != null && monsterTouchedToxic !=null && spriteRenderer != null;
    }

    public override void OnStateStart() {
        
        currentTimer = maxTimer;
        timerReachedZero = false;
        spriteRenderer.color = Color.green; 
    }

    public override void OnStateUpdate() {
        
        animator.SetBool("Idle", true); 
        animator.SetBool("Patrolling", false); 
        animator.SetBool("Chasing", false); 
        
        if (currentTimer > 0) {
            
            currentTimer -= Time.deltaTime;
            
            if (currentTimer <= 0) {
                
                currentTimer = 0; // Stop at 0
                timerReachedZero = true; // Set boolean to true when timer reaches 0
            }
        }
    }

    public override void OnStateEnd() {
    }
    
    public override int StateTransitionCondition() {
        
        if (timerReachedZero) {
            
            return (int)EShowcaseMonsterStates.Patrolling;
        }

        if (monsterSawPlayer.HasSeenPlayerThisFrame()) {
            
            return (int)EShowcaseMonsterStates.Chasing;
        }
        if (monsterTouchedToxic.HasTouchedToxicThisFrame()) {
            
            return (int)EShowcaseMonsterStates.Death;
        }
        
        return (int)EShowcaseMonsterStates.Invalid;
    }
}

