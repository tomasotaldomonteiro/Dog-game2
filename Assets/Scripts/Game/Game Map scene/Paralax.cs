using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Calculate horizontal parallax effect
        float distance = cam.transform.position.x * parallaxEffect; 
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        // Smooth horizontal movement using deltaTime
        float smoothX = Mathf.Lerp(transform.position.x, startPos + distance, Time.deltaTime * 10f); // Adjust 10f for smoothness speed
        transform.position = new Vector3(smoothX, cam.transform.position.y, transform.position.z);

        // Infinite horizontal scrolling logic
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}