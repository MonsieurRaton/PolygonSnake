using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;


    /*[Header("Cameras")]
    public Camera mainCamera;
    CameraFollow mainCameraFollow;*/
    //public Camera uiCamera;

    [Header("Canvas")]
    public Canvas mainCanvas;

    [Header("Game")]
    public SceneLevel sceneLevel;
    public GameBoardUI gameBoard;
    public StartingPosition[] startingPositions;
    public ItemSpawner[] itemSpawners;
    public Combo[] combos;


    [Header("VisualDebug")]
    public List<HexaEntity> entities;
    public List<GameObject> sceneObjects;

    [SerializeField] private GameObject startButton;

    void Awake()
    {
        instance = this;
        //mainCameraFollow = mainCamera.GetComponent<CameraFollow>();
    }


    void Start()
    {
        sceneLevel.Load();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            SceneLoader.QuitApplication();
            //SceneLoader.LoadScene(SceneLoader.SceneIndexes.StartingScene);
            return;
        } else if (sceneLevel.CanBeStarted && Input.GetKeyDown(KeyCode.O)) {
            sceneLevel.Run();
        }

        if (sceneLevel.CanBeStarted) {
            startButton.SetActive(true);
        }
    }

    /*public void SetMainCameraEnable(bool enable)
    {
        mainCamera.enabled = enabled;
    }

    public void AddToCameraAndFollow(Transform target)
    {
        mainCameraFollow.AddAndFollow(target);
    }*/

    /*public void SetUICameraEnable(bool enable)
    {
        uiCamera.gameObject.SetActive(enable);// enable doesn't work on this ne for some reason...
    }*/

    public Vector2 MainCanvasSize { get { return mainCanvas.pixelRect.size; } }

    public int RegisterEntity(HexaEntity entity)
    {
        entities.Add(entity);
        return entities.Count - 1;
    }

    public void UnRegisterEntity(HexaEntity entity)
    {
        entities.Remove(entity);
    }

    public void DestroyAllEntities()
    {
        for (int i = 0; i < entities.Count; i++) entities[i].Destroy();
    }


    public void RegisterGameObject(GameObject caller)
    {
        sceneObjects.Add(caller);
    }

    public void UnRegisterGameObject(GameObject caller)
    {
        sceneObjects.Remove(caller);
    }

    public void DestroyAllSceneObjects()
    {
        for (int i = 0; i < sceneObjects.Count; i++) Destroy(sceneObjects[i]);
    }

    public StartingPosition GetStartingPosition(int index)
    {
        return startingPositions[index];
    }
}
