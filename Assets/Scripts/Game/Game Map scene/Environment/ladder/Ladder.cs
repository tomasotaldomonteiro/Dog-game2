using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
  private float vertical;
  private float speed = 8f;
  private bool touchingLadder;
  private bool climbing;
  private Animator animator;
  private Rigidbody2D rb;
  private BoxCollider2D boxCollider;

  private Vector2 climbingOffset = new Vector2(0, 0.1f); // Offset when climbing
  private Vector2 climbingSize = new Vector2(0.5f, 0.9f); // Size when climbing
  private Vector2 originalOffset;
  private Vector2 originalSize; 
  
  
  private void Awake()
  {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    boxCollider = GetComponent<BoxCollider2D>();

    // Store the original collider properties
    originalOffset = boxCollider.offset;
    originalSize = boxCollider.size;
  }


  void Update()
  {
    vertical = Input.GetAxis("Vertical");

    if (touchingLadder && Mathf.Abs(vertical) > 0)
    {
      climbing = true;
    }
   
  }

  private void FixedUpdate()
  {
    if (climbing)
    {
      rb.gravityScale = 0f;
      rb.velocity = new Vector2(rb.velocity.x, speed * vertical);
      animator.SetBool("Move", false);
      animator.SetBool("Climbing", true);
      
      boxCollider.offset = climbingOffset;
      boxCollider.size = climbingSize;
    }
    else
    {
      rb.gravityScale = 4F;
      animator.SetBool("Climbing", false);
      
      boxCollider.offset = originalOffset;
      boxCollider.size = originalSize;
    }
  }
 
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Ladder"))
    {
      touchingLadder = true;
      rb.gravityScale = 0f;
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.CompareTag("Ladder"))
    {
      touchingLadder = false;
      climbing = false;
      rb.gravityScale = 4f;
      
      boxCollider.offset = originalOffset;
      boxCollider.size = originalSize;
    }
  }
}
