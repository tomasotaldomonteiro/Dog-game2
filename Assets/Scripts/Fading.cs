using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public Color fadeColor = Color.black; // Color of the fade
    public float fadeDuration = 1f;      // Duration of the fade

    private Texture2D fadeTexture;
    private float fadeAlpha = 0f;        // Current alpha value
    private int fadeDirection = 0;      // -1 for fade in, 1 for fade out

    private void Awake()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, fadeColor);
        fadeTexture.Apply();
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
                fadeDirection = 0; // Stop fading when fully faded in or out
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
}