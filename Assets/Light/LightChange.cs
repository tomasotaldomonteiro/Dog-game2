using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LightChange : MonoBehaviour
{
    public Scrollbar scrollbar;

    public Light2D gLight2D;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gLight2D);
        scrollbar.value = gLight2D.intensity;        
        scrollbar.onValueChanged.AddListener(UpdateBrightness);

    }

    // Update is called once per frame
    void UpdateBrightness(float value)
    {
        gLight2D.intensity = value;
    }
}
