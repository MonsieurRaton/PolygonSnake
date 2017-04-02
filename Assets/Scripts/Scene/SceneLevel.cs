using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneLevel : MonoBehaviour
{
    public LoadingScreen loadingScreen;
    public Level level;

    [Space(5)]
    public HexaEntity player;

    [Header("Test/Misc")]
    public HexaEntity snakePrefab;
    public float debugWait = 2;
    public float timeElapsed;
    public bool[] debugTestEvents = new bool[3] { false, false, false };
    
    public bool IsRunning { get; set; }
    public bool HasLoaded { get; set; }
    public bool GameOver { get; set; }

    public bool CanBeStarted { get { return HasLoaded && GameOver; } }

    void Update()
    {
        if (!HasLoaded || !IsRunning || GameOver) return;

        if (player == null)
        {
            IsRunning = false;
            GameOver = true;
            return;
        }
        else
        {
            if (GameController.instance.gameBoard.time == 0)
            {
                Debug.Log("Time out");
                player.TimeOut();
                return;
            }
        }

        timeElapsed += Time.deltaTime;

        if (!debugTestEvents[0] && timeElapsed >= 3)
        {
            GameController.instance.itemSpawners[0].Spawn = true;
            debugTestEvents[0] = true;
        }

        /*if (!debugTestEvents[1] && timeElapsed >= 10)
        {
            GameController.instance.combos[0].StartCombo();
            debugTestEvents[1] = true;
        }*/
    }

    public void Load()
    {
        GameOver = true;

        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        loadingScreen.Init();

        level.Load();

        while (!level.hasLoaded)
        {
            yield return wait;
        }

        yield return new WaitForSeconds(debugWait);

        loadingScreen.Destroy();
        
        HasLoaded = true;
    }


    public void Run()
    {
        StartCoroutine(RunCoroutine());
    }

    IEnumerator RunCoroutine()
    {
        GameOver = false;
        timeElapsed = 0;
        
        GameController.instance.gameBoard.Run();

        player = Instantiate(snakePrefab);

        yield return new WaitForEndOfFrame();

        CameraFollow.main.SetFollow(player.snake.HeadGameObject.transform);

        IsRunning = true;
    }
}
