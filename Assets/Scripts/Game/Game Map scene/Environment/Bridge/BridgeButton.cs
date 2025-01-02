using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    public GameObject bridge; 
    public Transform bridgeTargetPosition; 
    public Transform player; 
    public float interactionDistance = 1.5f; // Distance within which the player can press the button
    public float bridgeSpeed = 2f; 

    private bool bridgeMoving = false;

    private void Start()
    {
        // Hide the bridge at the start
        bridge.SetActive(true);
    }

    private void Update()
    {
       
        if (!bridgeMoving && Vector2.Distance(player.position, transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Show the bridge and start moving it
                bridge.SetActive(true);
                bridgeMoving = true;
            }
        }

        if (bridgeMoving)
        {
            MoveBridge();
        }
    }

    void MoveBridge()
    {
        // Preserve the Z position while moving the bridge
        Vector3 currentPosition = bridge.transform.position;
        Vector3 targetPosition = new Vector3(bridgeTargetPosition.position.x, bridgeTargetPosition.position.y, currentPosition.z);

        bridge.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, bridgeSpeed * Time.deltaTime);

        // Stop moving if the bridge reaches the target position
        if (Vector3.Distance(bridge.transform.position, targetPosition) < 0.1f)
        {
            bridgeMoving = false;
        }
    }
}