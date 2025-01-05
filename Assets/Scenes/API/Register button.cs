using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Registerbutton : MonoBehaviour
{
    [SerializeField] public Button registerbutton;
    [SerializeField] public TMP_InputField name;
    [SerializeField] public TMP_InputField password;
    private RestAPI restAPI;
    // Start is called before the first frame update
    void Start()
    {
        GameObject APIManager = GameObject.Find("APIManager");
        if (APIManager != null)
        {
            restAPI = APIManager.GetComponent<RestAPI>();
        }
        else
        {
            Debug.LogError("No API Manager");
        }
        
        registerbutton.onClick.AddListener(Register);
    }

    private void Register()
    {
        if (restAPI != null)
        {
            StartCoroutine(restAPI.Register(name.text, password.text));
            
        }
    }
}

