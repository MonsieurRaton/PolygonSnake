using UnityEngine;
using System.Collections;

public class _PersistentData : MonoBehaviour
{
    public static _PersistentData instance;

    [Header("Game Options")]
    public bool music = true;
    public float musicValue = 1;

    public bool sound = true;
    public float soundVolume = 1;

    public int languageSelected = 0;
    public string[] languages = new string[2] { "English", "Français" };

    [Header("Player session")]
    public string todo = "TODO";


    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

}
