using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneLevel : SceneController
{
    public LoadingScreen loadingScreen;
    public Level level;
    public LayerMask effectLayer;

    [Space(5)]
    public HexaEntity player;

    [Header("Test/Misc")]
    public HexaEntity snakePrefab;
    public float debugWait = 2;
    public float timeElapsed;
    public bool[] debugTestEvents = new bool[3] { false, false, false };


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
                Debug.Log("Time out\n");
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

        if (!debugTestEvents[1] && timeElapsed >= 10)
        {
            GameController.instance.combos[0].StartCombo();
            debugTestEvents[1] = true;
        }
    }

    public override void Load()
    {
        GameOver = true;

        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        loadingScreen.Init();

        level.Load();

        while (!level.HasLoaded)
        {
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(debugWait);

        loadingScreen.Destroy();

        GameController.instance.SetMainCameraEnable(true);
        GameController.instance.SetUICameraEnable(false);

        HasLoaded = true;
    }


    public override void Run()
    {
        StartCoroutine(RunCoroutine());
    }

    IEnumerator RunCoroutine()
    {
        GameOver = false;
        timeElapsed = 0;

        //test-
        GameController.instance.gameBoard.Show();
        GameController.instance.gameBoard.score = 0;
        GameController.instance.gameBoard.time = 150;
        GameController.instance.gameBoard.active = true;
        //----

        player = Instantiate(snakePrefab);

        yield return new WaitForEndOfFrame();

        GameController.instance.AddToCameraAndFollow(player.snake.HeadGameObject.transform);

        IsRunning = true;
    }
}
