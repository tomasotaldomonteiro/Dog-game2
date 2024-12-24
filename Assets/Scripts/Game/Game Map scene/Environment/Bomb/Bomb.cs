using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour {

    private Rigidbody2D _rb;
    [SerializeField] private Collider2D _col;

    [Header("Explosion")]
    [SerializeField] private Collider2D _explosionArea;
    [SerializeField] private float explosionVelocity;

    [Header("Physics")]
    [SerializeField] private float decceleration = 8f;
    [SerializeField] private float fallingTopSpeed = 20f;
    [SerializeField] private float gravity = 20f;

    [Header("Details")]
    [SerializeField] private Collider2D _groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask dynamicObjectMask;
    [SerializeField] private bool touchedSomething = false;
    private Rigidbody2D playerRigidbody2D;
    private Vector2 dif;

    // Start is called before the first frame update
    void Start() {

        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CircleCollider2D>();
        fallingTopSpeed *= -1;
        _rb.gravityScale = gravity;

    }

    // Update is called once per frame
    void Update() {

        clamping();
    }

    private void FixedUpdate() {
        
        checkGround();
    }

    public void triggerExplosion() {
        
        playerRigidbody2D = FindAnyObjectByType<PlayerController>().gameObject.GetComponent<Rigidbody2D>();

        if (_explosionArea.IsTouchingLayers(playerMask) == true) {
            
            playerRigidbody2D.gravityScale = 8;
            FindAnyObjectByType<PlayerController>().atHighSpeeds = true;
            dif = FindAnyObjectByType<PlayerController>().gameObject.GetComponent<Transform>().position - transform.position;
            playerRigidbody2D.velocity = dif.normalized * explosionVelocity;
            //print(playerRigidbody2D.velocity);

        }

        FindAnyObjectByType<SpawningBomb>().bombCount = 0;
        FindAnyObjectByType<SpawningBomb>().bombCanDetonate = false;
        Destroy(gameObject);
    }

    private void clamping(){

        if (_rb.velocity.y < fallingTopSpeed){
            
            _rb.velocity = new Vector2(_rb.velocity.x, fallingTopSpeed);
        }

        if (_rb.velocity.x > 0.15){

            _rb.velocity += new Vector2(-decceleration, _rb.velocity.y) * Time.deltaTime;
            
        }else if (_rb.velocity.x < -0.15){
            
            _rb.velocity += new Vector2(decceleration, _rb.velocity.y) * Time.deltaTime;
        }

        if (_rb.velocity.x <= 0.15 && _rb.velocity.x >= -0.15){

            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }

    }

    private void checkGround(){

        if (_groundCheck.IsTouchingLayers(groundMask) == true) {

            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            //print("ground");
        }
    
    }

    private void OnCollisionEnter2D(Collision2D collision){
        
        if (touchedSomething == false) {

            touchedSomething = true;
            FindAnyObjectByType<SpawningBomb>().GetComponent<SpawningBomb>().bombCanDetonate = true;
        }
    }
}
