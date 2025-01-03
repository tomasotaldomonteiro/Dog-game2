using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

public class PlayerController : MonoBehaviour{
    
    private Transform elevatorTransform;
    private Vector3 lastElevatorPosition;
    
    private Rigidbody2D _rb;
    private Collider2D _col;
    private RaycastHit2D _groundCheck;
    private Animator animator;
    
    [Header("Projectile")]
    public ProjectileBehavior projectilePrefab;
    public Transform LaunchOffset;
    private bool isCooldown = false;
 
    

    [Header("Run")]
    [SerializeField] private float topSpeed = 14f;
    private float decceleration = 12f;
    private float acceleration = 12f;
    private float horizontal_input;
    
    [Header("Jump")]
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private float holdJumpMultiplier = 1.5f; // Multiplier when holding jump
    [SerializeField] private float maxJumpHoldTime = 0.2f;
    [SerializeField] private float maxJumpHeight = 3f;   // Max duration for holding jump to increase height
    private float jumpHoldTimer;
    private float jumpStartY;
    
    private float jumpBufferTime = 0.1f;
    private float jumpBufferTimer;
    private float coyoteTime = 0.1f;
    private float coyoteTimeTimer;
    private bool isGrounded;

    [Header("Physics")]
    private float fallingTopSpeed = 20f;
    private float gravity = 20f;

    [Header("Gravity Control")]
    private float gravityIncreaseRate = 5f;
    private float maxGravityScale = 100f;
    private float currentGravityScale;

    [Header("Details")]
    [SerializeField] private LayerMask groundMask;
    public bool isFacingRight;
    public bool atHighSpeeds;
    private float atHighSpeedTime = 1f;

    //private GameObject throwDirection;
    private float atHighSpeedTimer;

   

    private void Start(){
        
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        isFacingRight = true;
        fallingTopSpeed *= -1;
        atHighSpeeds = false;
        atHighSpeedTimer = atHighSpeedTime;
    }
    
    private void Update() {
        
        if (!atHighSpeeds){                                     // Disable main movement and jump when climbing
            movement();
            jump();
        }
        
        clampFalling();
        checkGround();
        turnCheck();
        highSpeeds();
        gravityControl(); 
        
        if (elevatorTransform != null) {
            
            // Calculate the difference in the elevator's movement
            Vector3 elevatorMovement = elevatorTransform.position - lastElevatorPosition;

            // Move the player by the same amount
            transform.position += elevatorMovement;

            // Update the last position of the elevator
            lastElevatorPosition = elevatorTransform.position;
        }
    }
   
    
    
    public bool ThrowStone() {
        
        if (isCooldown || SceneStartSpawning.IsSpawning) return false;                              // Prevent throwing during cooldown
        
        Instantiate(projectilePrefab, LaunchOffset.position, transform.rotation);

        return true; 
    }


    private void gravityControl() {
        
        if (!isGrounded) {
            
            currentGravityScale += gravityIncreaseRate * Time.deltaTime;
            currentGravityScale = Mathf.Min(currentGravityScale, maxGravityScale); 
            _rb.gravityScale = currentGravityScale; 
            
        }else {
            
            currentGravityScale = gravity; 
            _rb.gravityScale = gravity;
        }
    }
    private void movement() {

        horizontal_input = Input.GetAxisRaw("Horizontal");
        
         //if the player is pressing left or right (horizontal_input != 0) the velocity increases either 1/-1
        if (horizontal_input != 0 && Mathf.Abs(_rb.velocity.x) < topSpeed) {
        
            _rb.velocity += new Vector2(horizontal_input * acceleration, 0) * Time.deltaTime;
            animator.SetBool("Move", true); 
            
         //if the player is trying to move in right direction while already exceeding the topSpeed. if true then caps.
        }else if (horizontal_input > 0 && _rb.velocity.x > topSpeed){
        
            _rb.velocity = new Vector2(topSpeed, _rb.velocity.y);
            animator.SetBool("Move", true);
            
         //if the player is trying to move in left direction while already exceeding the topSpeed. if true then caps.
        }else if (horizontal_input < 0 && _rb.velocity.x < -topSpeed) {
            
            _rb.velocity = new Vector2(-topSpeed, _rb.velocity.y);
            animator.SetBool("Move", true); 
            
         // Set move to false when idle and zeroes velocity immediately to prevent sliding
        }else if (horizontal_input == 0) {
            
            _rb.velocity = new Vector2(0, _rb.velocity.y); 
            //animator.SetBool("Move", false);
        }
        
        
         //slowdown code if no right input is made but still moving
        if (horizontal_input <= 0 && _rb.velocity.x > 0.15){
            
            _rb.velocity += new Vector2(-decceleration, 0) * Time.deltaTime;
            animator.SetBool("Move", true);
        
         //slowdown code if no left input is made but still moving
        }else if (horizontal_input >= 0 && _rb.velocity.x < -0.15){

            _rb.velocity += new Vector2(decceleration, 0) * Time.deltaTime;
            animator.SetBool("Move", true);
        }
        
        
         //slowdown code if no input is made but still moving
        if (horizontal_input == 0 && _rb.velocity.x <= 0.15 && _rb.velocity.x >= -0.15){
            
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            animator.SetBool("Move", false);
        }
    }
    public bool IsMoving() {
        
        return Mathf.Abs(_rb.velocity.x) > 0.1f; // Check if the player is moving horizontally
    }

    private void jump(){
       
        // JUMP BUFFERING
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime;
            jumpStartY = transform.position.y;
            jumpHoldTimer = 0f; // Reset hold timer when jump starts
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // WHEN WE PRESS THE JUMP BUTTON
        if (jumpBufferTimer > 0 && coyoteTimeTimer > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpPower);
            animator.SetTrigger("Jump");
            jumpBufferTimer = 0;
        }

        // HOLD JUMP TO INCREASE HEIGHT
        if (Input.GetButton("Jump") && !isGrounded)
        {
            if (jumpHoldTimer < maxJumpHoldTime && (transform.position.y - jumpStartY) < maxJumpHeight)
            {
                _rb.velocity += new Vector2(0, holdJumpMultiplier * Time.deltaTime);
                jumpHoldTimer += Time.deltaTime;
            }
        }

        // COYOTE TIMER
        if (isGrounded) {
            
            coyoteTimeTimer = coyoteTime;
            animator.SetBool("IsGrounded", true);
            
        }else {
            
            coyoteTimeTimer -= Time.deltaTime;
            animator.SetBool("IsGrounded", false);

        }

        // WHEN WE RELEASE THE JUMP BUTTON
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0) {
            
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f); // Cut jump height
        }
        
         //RESET GRAVITY
        if (isGrounded){
            _rb.gravityScale = gravity;
        }

        
    }

    private void clampFalling(){
        
        if (_rb.velocity.y < fallingTopSpeed){
            _rb.velocity = new Vector2(_rb.velocity.x, fallingTopSpeed);
        }
    }

    private void checkGround() {

        Color _color = Color.green;
        _groundCheck = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, 0.1f, groundMask);
        isGrounded = _groundCheck.collider != null ? true : false; 

        //Debug.Log("Is Grounded: " + isGrounded); 
        
        if (_groundCheck.collider != null){
            
            _color = Color.red;
            
        }else if (coyoteTimeTimer > 0){
            
            _color = Color.blue;
            
        }else if (jumpBufferTimer > 0){
            
            _color = Color.cyan;
        }

        
        Debug.DrawRay(_col.bounds.center + new Vector3(_col.bounds.extents.x, 0), Vector2.down * (_col.bounds.extents.y + 0.05f), _color);
        Debug.DrawRay(_col.bounds.center - new Vector3(_col.bounds.extents.x, 0), Vector2.down * (_col.bounds.extents.y + 0.05f), _color);
        Debug.DrawRay(_col.bounds.center - new Vector3(_col.bounds.extents.x, _col.bounds.extents.y + 0.05f), Vector2.right * (_col.bounds.extents.x * 2), _color);
        

    }

    private void turnCheck(){
        float scaleX = transform.localScale.x;
        if (scaleX < 0 && horizontal_input > 0 || scaleX > 0 && horizontal_input < 0)
        {
            isFacingRight = !isFacingRight; 
            scaleX *= -1;
            transform.localScale = new Vector3(scaleX, 1, 1);
        }
    }

    void highSpeeds(){
    
        if (atHighSpeeds){

            atHighSpeedTimer -= Time.deltaTime;
        }
        
        if (atHighSpeedTimer < 0 && atHighSpeeds){

            atHighSpeeds = false;
            atHighSpeedTimer = atHighSpeedTime;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Elevator"))
        {
            elevatorTransform = collision.gameObject.transform;
            lastElevatorPosition = elevatorTransform.position; // Track the elevator's initial position
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Elevator"))
        {
            elevatorTransform = null; // Stop tracking the elevator
        }
    }

}
