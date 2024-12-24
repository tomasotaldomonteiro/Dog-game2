using UnityEngine;
using UnityEditor;

public class FindMissingScripts : MonoBehaviour
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void FindAllMissingScripts()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            Component[] components = obj.GetComponents<Component>();

            foreach (Component c in components)
            {
                if (c == null)
                {
                    Debug.LogError($"Missing script on GameObject: {obj.name}", obj);
                    count++;
                }
            }
        }

        Debug.Log($"Finished! Found {count} GameObjects with missing scripts.");
    }
}