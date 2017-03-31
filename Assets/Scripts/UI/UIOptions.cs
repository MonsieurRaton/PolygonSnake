using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIOptions : MonoBehaviour
{
    public Text musicText;
    public Text soundText;
    public Text languageText;

    _PersistentData PersitentData { get; set; }

    void Start()
    {
        PersitentData = _PersistentData.instance;
    }


    public void ToggleMusic(UIButton button)
    {
        if (button.externalSpecialState) return; button.externalSpecialState = true;

        PersitentData.music = !PersitentData.music;
        musicText.text = PersitentData.music ? "On" : "Off";
    }


    public void ToggleSound(UIButton button)
    {
        if (button.externalSpecialState) return; button.externalSpecialState = true;

        PersitentData.sound = !PersitentData.sound;
        soundText.text = PersitentData.sound ? "On" : "Off";
    }


    public void ToggleLanguage(UIButton button)
    {//More things to do later
        if (button.externalSpecialState) return; button.externalSpecialState = true;

        PersitentData.languageSelected++;
        if (PersitentData.languageSelected >= PersitentData.languages.Length) PersitentData.languageSelected = 0;
        languageText.text = PersitentData.languages[PersitentData.languageSelected];
    }
}
