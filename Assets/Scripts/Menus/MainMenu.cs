using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Fading fading; // Reference to the Fading script

    public void PlayGame()
    {
        fading.FadeOutAndChangeScene(1); // Trigger fade-out and load scene
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}