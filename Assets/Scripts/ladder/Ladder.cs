using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
  private float vertical;
  private float speed = 8f;
  private bool touchingLadder;
  private bool climbing;
  
  [SerializeField] private Rigidbody2D rb;

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
    }
    else
    {
      rb.gravityScale = rb.gravityScale = 4F;
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
    }
  }
}
