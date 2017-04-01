using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour
{

    public static void LoadScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public static void QuitApplication() {
#if UNITY_EDITOR
        if (Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
