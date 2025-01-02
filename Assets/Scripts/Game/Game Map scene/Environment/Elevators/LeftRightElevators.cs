using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightElevators : MonoBehaviour {
    
    [SerializeField] public Transform player;
    [SerializeField] private Transform elevatorswitch; 
    [SerializeField] private Transform rightpos; 
    [SerializeField] private Transform  leftpos;
    private SpriteRenderer elevator; 
    private Animator animator;
    private float speed = 3f;
    private bool isMovingRight; 

    void Start(){
        
        animator = GetComponent<Animator>();
        elevator = GetComponent<SpriteRenderer>();
        
        transform.position = leftpos.position;

        // Ensure the elevator starts at the right and doesn't move until interacted with
        isMovingRight = true;

    }
    
    void Update(){
        
        StartElevator();
        Animate();
    }

    void StartElevator()
    {
        // trigger the elevator to start moving if the player is near the switch and presses E
        if (Vector2.Distance(player.position, elevatorswitch.position) < 0.5f && Input.GetKeyDown(KeyCode.E)) {
         
            isMovingRight = !isMovingRight;
        }

        // Move the elevator based on the current direction
        if (isMovingRight){
            
            transform.position = Vector2.MoveTowards(transform.position, leftpos.position, Time.deltaTime * speed);
            animator.SetBool("Right", true); 
            
        }else{
            
            transform.position = Vector2.MoveTowards(transform.position, rightpos.position, Time.deltaTime * speed);
            animator.SetBool("Right", false); 
        }
    }

    void Animate() {
        
        if (transform.position.x >= leftpos.position.x || transform.position.x <= rightpos.position.x) {
            
            animator.SetBool("Idle", true); 
           // elevator.color = Color.green; // Resting
            
        }else {
            animator.SetBool("Idle", false); 
            //elevator.color = Color.red; // Moving
        }
    }
}