using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour{
    
    public Color fadeColor = Color.black;  // Color of the fade
    public float fadeDuration = 1f;       // Default fade duration
    public float scene1FadeInDuration = 3f; // Custom fade-in duration for Scene 1
    public float winningScreenFadeOutDuration = 0.5f; // Custom fade-out duration for winning screens
    public float winningScreenFadeInDuration = 1f; 
    public float blackScreenDuration = 2f; // Time to keep the screen fully black before fading in

    private Texture2D fadeTexture;
    private float fadeAlpha = 0f;         // Current alpha value
    private int fadeDirection = 0;       // -1 for fade in, 1 for fade out
    private float currentFadeDuration;   // Tracks the fade duration dynamically

    private void Awake() {
        
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, fadeColor);
        fadeTexture.Apply();

        // Manually trigger fade-in for Scene 1
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            
            fadeAlpha = 1f;  
            currentFadeDuration = winningScreenFadeInDuration;
            StartFadeIn();                                              // Standard fade-in for winning screen
        } else if (SceneManager.GetActiveScene().buildIndex == 1){              // Check if the current scene is Scene 1
            
            fadeAlpha = 1f;                                             // Start with a black screen
            currentFadeDuration = scene1FadeInDuration;                 // Use the specified fade-in duration for Scene 1
            StartCoroutine(DelayedFadeIn());
            
        } else if (SceneManager.GetActiveScene().buildIndex == 2) {
            
            fadeAlpha = 1f;  
            currentFadeDuration = winningScreenFadeInDuration;
            StartFadeIn();                                              // Standard fade-in for winning screen
        } else if (SceneManager.GetActiveScene().buildIndex == 3) {
            
            fadeAlpha = 1f;  
            currentFadeDuration = winningScreenFadeInDuration;
            StartFadeIn();                                              // Standard fade-in for winning screen
        }else{
            
            currentFadeDuration = fadeDuration;                         // Default fade duration for other scenes
        }
    }

    private void OnGUI() {
        
        if (fadeAlpha > 0f) {
            
            Color guiColor = GUI.color;
            GUI.color = new Color(guiColor.r, guiColor.g, guiColor.b, fadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
            GUI.color = guiColor;
        }
    }

    private void Update() {
        
        if (fadeDirection != 0) {
            
            fadeAlpha += fadeDirection * Time.deltaTime / currentFadeDuration;
            fadeAlpha = Mathf.Clamp01(fadeAlpha);

            if (fadeAlpha == 0f || fadeAlpha == 1f) {
                
                fadeDirection = 0;                                  // Stop fading when fully faded in or out
            }
        }
    }

    public void StartFadeIn() {
        
        fadeDirection = -1;                                        // Trigger fade-in
    }

    public void StartFadeOut() {
        
        fadeDirection = 1;                                          // Trigger fade-out
        currentFadeDuration = fadeDuration;                         // Reset to default fade duration for other scenes
    }

    public void FadeOutAndChangeScene(int sceneIndex) {
        
        StartCoroutine(FadeOutAndLoadScene(sceneIndex));
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex) {
        
        yield return new WaitForSeconds(blackScreenDuration);       // Wait for black screen duration
        StartFadeOut();                                             // Trigger fade-out
        yield return new WaitForSeconds(fadeDuration);              // Wait for fade-out to complete
        SceneManager.LoadSceneAsync(sceneIndex);                    // Load the new scene
    }

    private IEnumerator DelayedFadeIn() {
        
        yield return new WaitForSeconds(blackScreenDuration);      // Wait for black screen duration
        StartFadeIn();                                             // Start fading in after the delay
    }
}