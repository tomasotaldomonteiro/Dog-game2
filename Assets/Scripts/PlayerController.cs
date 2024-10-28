using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _col;
    private RaycastHit2D _groundCheck;
    
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

    [Header("Physics")]
    [SerializeField] private float fallingTopSpeed = 20f;
    [SerializeField] private float gravity = 20f;

    [Header("Details")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] public bool isFacingRight;
    [SerializeField] private bool isGrounded;
    [SerializeField] public bool atHighSpeeds;
    [SerializeField] private float atHighSpeedTime = 1f;

    private float atHighSpeedTimer;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
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

        clampFalling();

        checkGround();

        turnCHeck();

        highSpeeds();

    }

    private void movement()
    {

        horizontal_input = Input.GetAxisRaw("Horizontal");

        if (horizontal_input != 0 && _rb.velocity.x < topSpeed && _rb.velocity.x > -topSpeed)
        {

            _rb.velocity += new Vector2(horizontal_input * acceleration,0) * Time.deltaTime;

        } else if (horizontal_input != 0 && _rb.velocity.x > topSpeed)
        {

            _rb.velocity = new Vector2(topSpeed, _rb.velocity.y);

        }else if (horizontal_input != 0 && _rb.velocity.x < -topSpeed)
        {

            _rb.velocity = new Vector2(-topSpeed, _rb.velocity.y);

        }

        if (horizontal_input <= 0 && _rb.velocity.x > 0.15)
        {

            _rb.velocity += new Vector2(-decceleration, 0) * Time.deltaTime;

        } else if (horizontal_input >= 0 && _rb.velocity.x < -0.15)
        {

            _rb.velocity += new Vector2(decceleration, 0) * Time.deltaTime;

        }

        if (horizontal_input == 0 && _rb.velocity.x <= 0.15 && _rb.velocity.x >= -0.15)
        {

            _rb.velocity = new Vector2(0, _rb.velocity.y);

        }

    }

    private void jump()
    {

        //WHEN WE PRESS THE JUMP BUTTON

        if (jumpBufferTimer > 0 && coyoteTimeTimer > 0)
        {

            _rb.velocity = new Vector2(_rb.velocity.x, jumpPower);

            jumpBufferTimer = 0;
        
        }

        //WHEN WE RELEASE THE JUMP BUTTON

        if (Input.GetButtonUp("Jump") && _rb.velocity.y >= 0)
        {

            _rb.gravityScale = gravity * 3;

            coyoteTimeTimer = 0f;


        }

        //COYOTE TIME

        if (isGrounded)
        {

            coyoteTimeTimer = coyoteTime;

        }
        else 
        {

            coyoteTimeTimer -= Time.deltaTime;


        }

        //JUMP BUFFERING

        if (Input.GetButtonDown("Jump"))
        {

            jumpBufferTimer = jumpBufferTime;

        }
        else
        {

            jumpBufferTimer -= Time.deltaTime;


        }

        //RESET GRAVITY

        if (isGrounded)
        {

            _rb.gravityScale = gravity;

        }
    }

    private void clampFalling() 
    {
    
        if (_rb.velocity.y < fallingTopSpeed) 
        {

            _rb.velocity = new Vector2(_rb.velocity.x, fallingTopSpeed);
        
        }
    
    }

    private void checkGround() 
    {

        Color _color = Color.green;

        _groundCheck = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, 0.1f, groundMask);

        isGrounded = _groundCheck.collider != null ? true : false; 

        

        if (_groundCheck.collider != null)
        {

            _color = Color.red;

        }
        else if (coyoteTimeTimer > 0)
        {

            _color = Color.blue;

        }else if (jumpBufferTimer > 0)
        {

            _color = Color.cyan;

        }

        //Draws the _groundCheck lower and side bounds to give feedback while testing in the editor
        Debug.DrawRay(_col.bounds.center + new Vector3(_col.bounds.extents.x, 0), Vector2.down * (_col.bounds.extents.y + 0.05f), _color);
        Debug.DrawRay(_col.bounds.center - new Vector3(_col.bounds.extents.x, 0), Vector2.down * (_col.bounds.extents.y + 0.05f), _color);
        Debug.DrawRay(_col.bounds.center - new Vector3(_col.bounds.extents.x, _col.bounds.extents.y + 0.05f), Vector2.right * (_col.bounds.extents.x * 2), _color);
        //Debug.Log(_groundCheck.collider);

    }

    private void turnCHeck()
    {

        if (isFacingRight && horizontal_input < 0)
        {

            isFacingRight = false;
            GetComponentInChildren<SpriteRenderer>().flipX = true;

        } else if (!isFacingRight && horizontal_input > 0)
        {
        
            isFacingRight = true;
            GetComponentInChildren<SpriteRenderer>().flipX = false;

        }
        
    }

    void highSpeeds()
    {
    
        if (atHighSpeeds)
        {

            atHighSpeedTimer -= Time.deltaTime;

        }
        
        if (atHighSpeedTimer < 0 && atHighSpeeds)
        {

            atHighSpeeds = false;

            atHighSpeedTimer = atHighSpeedTime;

        }
    
    }

}
