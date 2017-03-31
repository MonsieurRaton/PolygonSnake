using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameBoardUI : MonoBehaviour
{
    public Text scoreText;
    public Text timeMinText;
    public Text timeSecText;


    public void ShowScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowTime(float time)
    {
        timeMinText.text = Mathf.Floor(time / 60).ToString("00");
        timeSecText.text = Mathf.Floor(time % 60).ToString("00");
    }
}
