using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour{
    
    public bool isBouncingFromJumpPad = false;  // Flag to indicate bounce state
    private float bounceDisableJumpTimer = 0f;
    private Rigidbody2D _rb;
    private Collider2D _col;
    private RaycastHit2D _groundCheck;
    private MovingPlatform currentPlatform; 
    private Animator animator;

    
    
    private float horizontal_input;

    [Header("Run")]
    [SerializeField] private float topSpeed = 8f;
    [SerializeField] private float decceleration = 8f;
    [SerializeField] private float acceleration = 8f;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 40f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferTimer;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeTimer;
    [SerializeField] private float maxJumpHeight = 1f; // Maximum height for jumping
    private float initialJumpY; // Store the initial Y position when the jump starts


    [Header("Physics")]
    [SerializeField] private float fallingTopSpeed = 20f;
    [SerializeField] private float gravity = 20f;

    [Header("Gravity Control")]
    [SerializeField] private float gravityIncreaseRate = 5f; // Rate at which gravity increases
    [SerializeField] private float maxGravityScale = 100f;    // Maximum gravity scale
    private float currentGravityScale; 
    
    [Header("Details")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] public bool isFacingRight;
    [SerializeField] private bool isGrounded;
    [SerializeField] public bool atHighSpeeds;
    [SerializeField] private float atHighSpeedTime = 1f;

    private float atHighSpeedTimer;

    // Start is called before the first frame update
    private void Start(){
        
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        isFacingRight = true;
        fallingTopSpeed *= -1;
        atHighSpeeds = false;
        atHighSpeedTimer = atHighSpeedTime;
    }

    // Update is called once per frame
 private void Update()
    {
        if (!atHighSpeeds)
        {
            movement();
            jump();
        }

        if (bounceDisableJumpTimer > 0) bounceDisableJumpTimer -= Time.deltaTime;

        clampFalling();
        checkGround();
        turnCheck();
        highSpeeds();
        gravityControl();
    }

    private void gravityControl()
    {
        if (!isGrounded)
        {
            // Increase the gravity scale while falling
            currentGravityScale += gravityIncreaseRate * Time.deltaTime;
            currentGravityScale = Mathf.Min(currentGravityScale, maxGravityScale); // Limit the gravity scale
            _rb.gravityScale = currentGravityScale; // Apply the current gravity scale
        }
        else
        {
            // Reset the gravity scale when grounded
            currentGravityScale = gravity; // Or reset to your original gravity value
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

    private void jump(){
        if (isBouncingFromJumpPad || bounceDisableJumpTimer > 0) return; // Skip jump if bouncing or timer is active

        //JUMP BUFFERING
        if (Input.GetButtonDown("Jump")){
            jumpBufferTimer = jumpBufferTime;
            initialJumpY = transform.position.y; 
        }else{
            jumpBufferTimer -= Time.deltaTime;
        }
        
         //WHEN WE PRESS THE JUMP BUTTON
        if (jumpBufferTimer > 0 && coyoteTimeTimer > 0){
            _rb.velocity = new Vector2(_rb.velocity.x, jumpPower);
            jumpBufferTimer = 0;
        }
        
         //STOP BIG "FLOATING" WHEN AFTER REACHING MAX HEIGHT
        if (!isGrounded && Input.GetButton("Jump")) {
            if (transform.position.y >= initialJumpY + maxJumpHeight) {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpPower * 3f);
            }
        }
        
         //COYOTE TIMER
        if (isGrounded){
            coyoteTimeTimer = coyoteTime;
        }else{
            coyoteTimeTimer -= Time.deltaTime;
        }

         //WHEN WE RELEASE THE JUMP BUTTON
        if (Input.GetButtonUp("Jump") && _rb.velocity.y >= 0){
            _rb.gravityScale = gravity * 1.6f;
            coyoteTimeTimer = 0f;
        }
        
         //RESET GRAVITY
        if (isGrounded){
            _rb.gravityScale = gravity;
        }

        
    }



    public void DisableJumpForBounce(float duration)
    {
        bounceDisableJumpTimer = duration;
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

        
        if (_groundCheck.collider != null){
            
            _color = Color.red;
            
        }else if (coyoteTimeTimer > 0){
            
            _color = Color.blue;
            
        }else if (jumpBufferTimer > 0){
            
            _color = Color.cyan;
        }

        //Draws the _groundCheck lower and side bounds to give feedback while testing in the editor
        Debug.DrawRay(_col.bounds.center + new Vector3(_col.bounds.extents.x, 0), Vector2.down * (_col.bounds.extents.y + 0.05f), _color);
        Debug.DrawRay(_col.bounds.center - new Vector3(_col.bounds.extents.x, 0), Vector2.down * (_col.bounds.extents.y + 0.05f), _color);
        Debug.DrawRay(_col.bounds.center - new Vector3(_col.bounds.extents.x, _col.bounds.extents.y + 0.05f), Vector2.right * (_col.bounds.extents.x * 2), _color);
        //Debug.Log(_groundCheck.collider);

    }

    private void turnCheck(){

        if (isFacingRight && horizontal_input < 0){

            isFacingRight = false;
            GetComponentInChildren<SpriteRenderer>().flipX = true;

        }else if (!isFacingRight && horizontal_input > 0){
            
            isFacingRight = true;
            GetComponentInChildren<SpriteRenderer>().flipX = false;
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
    
    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag("MovingPlatform")) {
            
            currentPlatform = collision.gameObject.GetComponent<MovingPlatform>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag("MovingPlatform")){
            
            currentPlatform = null;
        }
    }

    
}
