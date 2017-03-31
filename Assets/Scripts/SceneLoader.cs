using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public class SceneIndexes
    {
        public const int Quit = -1;
        public const int StartingScene = 0;
        public const int TestPlay = 1;
    }

    public static void LoadScene(int index)
    {
        if (index == SceneIndexes.Quit)
        {
#if UNITY_EDITOR
            if (Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }
}
