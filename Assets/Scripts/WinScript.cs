using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    [SerializeField] private GameObject win;

    void Update()
    {
       
    }  

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            win.SetActive(true);
        }
    }  
    
}
