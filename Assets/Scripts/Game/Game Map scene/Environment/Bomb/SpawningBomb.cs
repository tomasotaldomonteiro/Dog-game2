using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawningBomb : MonoBehaviour
{
    
    [SerializeField] private GameObject bombPrefab;

    [SerializeField] public int bombCount;
    [SerializeField] public bool bombCanDetonate = false;

    private float horizontal_input;
    private float vertical_input;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

        horizontal_input = Input.GetAxisRaw("Horizontal");
        vertical_input = Input.GetAxisRaw("Vertical");

        DropingBomb();

        spawningBomb();

        DetonateBomb();

    }

    private void DropingBomb()
    {
    
        if (Input.GetButtonDown("Fire2") && bombCount == 0)
        {

            print("Drop Bomb");
            GameObject bomb = Instantiate(bombPrefab,transform.position,Quaternion.identity);

            bombCount = 1;

        }

    }

    private void spawningBomb()
    {
    
        if (Input.GetButtonDown("Fire1") && bombCount == 0)
        {

            print("Launch Bomb");

            LaunchingBomb();

            bombCount = 1;

        }
    
    }

    private void LaunchingBomb()
    {

        Vector2 bombPosition = new Vector2(transform.position.x, transform.position.y);
        GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);

        if (horizontal_input == 0 && vertical_input == 0)
        {

            if (gameObject.GetComponent<PlayerController>().isFacingRight)
            {

                bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(22, 11);

            }
            else
            {

                bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(-22, 11);

            }

        }


        if (horizontal_input > 0 && horizontal_input < 0 && vertical_input == 0)
        {

            bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(22 * horizontal_input, 11);

            print("a");

        }

    }
    private void DetonateBomb() 
    {

        if (Input.GetButtonDown("Fire1") && bombCount == 1 && bombCanDetonate == true || Input.GetButtonDown("Fire2") && bombCount == 1 && bombCanDetonate == true)
        {

            print("Detonate Bomb");
            FindObjectOfType(typeof(Bomb)).GetComponent<Bomb>().triggerExplosion();

        }

    }
}
