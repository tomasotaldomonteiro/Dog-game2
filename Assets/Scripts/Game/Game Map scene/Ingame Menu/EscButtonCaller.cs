using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscButtonCaller : MonoBehaviour
{
    public Button settings; // Reference the UI button in inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Simulate a button click
            ExecuteEvents.Execute(settings.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }
}