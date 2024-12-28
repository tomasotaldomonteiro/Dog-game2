using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishToHome : MonoBehaviour {
    
    [SerializeField] private Fading fading;
    [SerializeField] GameObject FinishMenu;
    
    public void Home()
    {
        fading.FadeOutAndChangeScene(0); 
        
    }
}
