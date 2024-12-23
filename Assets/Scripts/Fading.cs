using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public Color fadeColor = Color.black;                         // Color of the fade
    public float fadeDuration = 1f;                                 // Duration of the fade

    private Texture2D fadeTexture;
    private float fadeAlpha = 0f;                                   // Current alpha value
    private int fadeDirection = 0;                                  // -1 for fade in, 1 for fade out

    private void Awake()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, fadeColor);
        fadeTexture.Apply();
    }

    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;                  //detects when scenes are loaded in order to do the fade logic shheeeesh
                                                                    //why ondestroy -> chatgepetee: The OnDestroy method unsubscribes from the sceneLoaded event to prevent memory leaks.
    }
    
    
    private void OnGUI()
    {
        if (fadeAlpha > 0f)
        {
            Color guiColor = GUI.color;
            GUI.color = new Color(guiColor.r, guiColor.g, guiColor.b, fadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
            GUI.color = guiColor;
        }
    }

    private void Update()
    {
        if (fadeDirection != 0)
        {
            fadeAlpha += fadeDirection * Time.deltaTime / fadeDuration;
            fadeAlpha = Mathf.Clamp01(fadeAlpha);

            if (fadeAlpha == 0f || fadeAlpha == 1f)
            {
                fadeDirection = 0;                                // Stop fading when fully faded in or out
            }
        }
    }

    public void StartFadeIn()
    {
        fadeDirection = -1;
    }

    public void StartFadeOut()
    {
        fadeDirection = 1;
    }
    
    public void FadeOutAndChangeScene(int sceneIndex)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneIndex));
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex)     //sheesh this is cuul
    {
        StartFadeOut();                                         // Trigger fade-out
        yield return new WaitForSeconds(fadeDuration);          // Wait for fade-out to complete
        SceneManager.LoadSceneAsync(sceneIndex);                // Load the new scene
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        fadeAlpha = 1f;                                         // Ensure the screen is black before fading in theeeen do fade
        StartFadeIn();
    }
}