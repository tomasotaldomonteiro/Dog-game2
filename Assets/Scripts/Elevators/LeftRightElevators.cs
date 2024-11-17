using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightElevators : MonoBehaviour
{
    public Transform player;
    public Transform elevatorswitch;
    public Transform leftpos; // The left position
    public Transform rightpos; // The right position
    public SpriteRenderer elevator;
    public float speed;
    private bool isMovingLeft; // Tracks the direction of movement

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial position to the right position
        transform.position = leftpos.position;

        // Ensure the elevator starts at the right and doesn't move until interacted with
        isMovingLeft = true;

    }

    // Update is called once per frame
    void Update()
    {
        StartElevator();
        DisplayColor();
    }

    void StartElevator()
    {
        // Only trigger the elevator to start moving if the player is near the switch and presses E
        if (Vector2.Distance(player.position, elevatorswitch.position) < 0.5f && Input.GetKeyDown(KeyCode.E))
        {
            // Toggle the elevator's direction
            isMovingLeft = !isMovingLeft;
        }

        // Move the elevator based on the current direction
        if (isMovingLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, leftpos.position, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, rightpos.position, Time.deltaTime * speed);
        }
    }

    void DisplayColor()
    {
        // Change the color based on the elevator's position
        if (transform.position.x <= leftpos.position.x || transform.position.x >= rightpos.position.x)
        {
            elevator.color = Color.green; // Resting at either end
        }
        else
        {
            elevator.color = Color.red; // Moving
        }
    }
}