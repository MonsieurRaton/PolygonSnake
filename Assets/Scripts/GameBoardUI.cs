using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameBoardUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;
    
    public float time;
    public int score;

    void Update() { // Exécutée que si est activé
        if (time > 0) {
            time -= Time.deltaTime;
            if (time < 0) time = 0;
        }

        ShowScore(score);
        ShowTime(time);
    }

    public void Run() {
        score = 0;
        time = 150;
        Show(true);
        enabled = true;
    }

    public void Show(bool show) {
        gameObject.SetActive(show);
    }

    public void ShowScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowTime(float time)
    {
        timeText.text = Mathf.Floor(time / 60).ToString("00") + ":" + Mathf.Floor(time % 60).ToString("00");
    }
}
