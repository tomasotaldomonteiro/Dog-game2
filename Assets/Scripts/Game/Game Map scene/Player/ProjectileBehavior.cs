using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectileBehavior : MonoBehaviour
{
    private float speed = 4.5f; 
    [SerializeField] private Rigidbody2D rb;  
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Vector2 launchDirection = (playerController.isFacingRight ? Vector2.right : Vector2.left) * speed;
        rb.AddForce(launchDirection, ForceMode2D.Impulse);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
