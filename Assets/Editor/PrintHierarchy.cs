using UnityEditor;
using UnityEngine;
public class PrintHierarchy
{
    [MenuItem("Tools/Print Hierarchy")]
    public static void Print()
    {
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Debug.Log("Root: " + obj.name);
        }
    }
}
