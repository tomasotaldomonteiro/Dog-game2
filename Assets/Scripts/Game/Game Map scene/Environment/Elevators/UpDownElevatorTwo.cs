using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownElevatorTwo : MonoBehaviour {
    
    [SerializeField] public Transform player;
    [SerializeField] private Transform elevatorswitch; 
    [SerializeField] private Transform downpos; 
    [SerializeField] private Transform upperpos;
    private SpriteRenderer elevator; 
    private Animator animator;
    private float speed = 3f;
    private bool iselevatordown; 


    void Start() {
        
        animator = GetComponent<Animator>();
        elevator = GetComponent<SpriteRenderer>();
        
        transform.position = upperpos.position;

        // Ensure the elevator starts up and doesn't move until interacted with
        iselevatordown = false;
    }

    void Update() {
        
        StartElevator();
        Animate();
    }

    void StartElevator() {
        
        // trigger the elevator to start moving if the player is near the switch and presses E
        if (Vector2.Distance(player.position, elevatorswitch.position) < 0.5f && Input.GetKeyDown(KeyCode.E)) {
           
            iselevatordown = !iselevatordown;
        }
        
        if (iselevatordown){
            
            transform.position = Vector2.MoveTowards(transform.position, downpos.position, Time.deltaTime * speed);
            animator.SetBool("Down", false); 
            
        }else{
            
            transform.position = Vector2.MoveTowards(transform.position, upperpos.position, Time.deltaTime * speed);
            animator.SetBool("Down", true); 
        }
    }

    void Animate() {
        
        if (transform.position.y >= downpos.position.y || transform.position.y <= upperpos.position.y) {
            
            //elevator.color = Color.green; // Resting at either end
            animator.SetBool("Idle", true); 
            
        }else {
            animator.SetBool("Idle", false); 
            //elevator.color = Color.red; // Moving
        }
    }
}