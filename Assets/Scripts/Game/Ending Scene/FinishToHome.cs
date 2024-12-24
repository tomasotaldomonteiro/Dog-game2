using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishToHome : MonoBehaviour
{
    [SerializeField] GameObject FinishMenu;   
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        
    }
}
