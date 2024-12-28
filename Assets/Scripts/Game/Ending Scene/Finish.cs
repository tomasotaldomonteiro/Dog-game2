using UnityEngine;

public class Finish : MonoBehaviour
{
    private Fading fading;

    private const int EndingOneSceneIndex = 2; 
    private const int EndingTwoSceneIndex = 3; 
    private void Start()
    {
        fading = FindObjectOfType<Fading>();

        if (fading == null)
        {
            Debug.LogError("Fading script not found in the scene. Ensure it is attached to an object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            if (PickupCollectible.Endingtwo)
            {
                fading.FadeOutAndChangeScene(EndingTwoSceneIndex);
            }
            else
            {
                fading.FadeOutAndChangeScene(EndingOneSceneIndex);
            }
        }
    }
}