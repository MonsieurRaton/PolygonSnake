using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameBoard : MonoBehaviour
{
    [Header("Prefab")]
    public GameBoardUI ui;

    [Header("VisualDebug")]
    public float time;
    public int score;
    public bool active;
    
    void Start()
    {
        ui.GetComponent<RectTransform>().sizeDelta = GameController.instance.MainCanvasSize;
    }

    void Update()
    {
        if (!active) return;

        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time < 0) time = 0;
        }

        ui.ShowScore(score);
        ui.ShowTime(time);
    }

    public void Show() { ui.gameObject.SetActive(true); }
    public void Hide() { ui.gameObject.SetActive(false); }
}
