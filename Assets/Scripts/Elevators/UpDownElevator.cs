using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownElevator : MonoBehaviour
{
    public Transform player;
    public Transform elevatorswitch;
    public Transform downpos;
    public Transform upperpos;
    public SpriteRenderer elevator;
    public float speed;
    private bool iselevatordown; // Will default to false (starts at the upper position)

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial position to the upper position
        transform.position = upperpos.position;

        // Ensure the elevator starts up and doesn't move until interacted with
        iselevatordown = false;

        // Set the elevator color to green (indicating it's at rest at an endpoint)
        elevator.color = Color.green;
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
            iselevatordown = !iselevatordown;
        }

        // Move the elevator based on the current direction
        if (iselevatordown)
        {
            transform.position = Vector2.MoveTowards(transform.position, downpos.position, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, upperpos.position, Time.deltaTime * speed);
        }
    }

    void DisplayColor()
    {
        // Change the color based on the elevator's position
        if (transform.position.y <= downpos.position.y || transform.position.y >= upperpos.position.y)
        {
            elevator.color = Color.green; // Resting at either end
        }
        else
        {
            elevator.color = Color.red; // Moving
        }
    }
}