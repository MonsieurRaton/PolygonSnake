using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;


    [Header("Cameras")]
    public Camera mainCamera;
    CameraFollow mainCameraFollow;
    public Camera uiCamera;

    [Header("Canvas")]
    public Canvas mainCanvas;

    [Header("Game")]
    public SceneController sceneController;
    public GameBoard gameBoard;
    public StartingPosition[] startingPositions;
    public ItemSpawner[] itemSpawners;
    public Combo[] combos;


    [Header("VisualDebug")]
    public List<HexaEntity> entities;
    public List<GameObject> sceneObjects;


    void Awake()
    {
        instance = this;
        mainCameraFollow = mainCamera.GetComponent<CameraFollow>();
    }


    void Start()
    {
        sceneController.Load();
    }


    void Update()
    {
        //if (Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape))
        //{
        //    SceneLoader.LoadScene(SceneLoader.SceneIndexes.Quit);
        //    return;
        //}
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneLoader.LoadScene(SceneLoader.SceneIndexes.StartingScene);
            return;
        }

        if (sceneController.CanBeStarted && Input.GetKeyDown(KeyCode.O))
        {
            sceneController.Run();
        }
    }

	void OnGUI(){
		if (sceneController.CanBeStarted){
			if(GUI.Button(new Rect(0,0,150,100),"START")){
				sceneController.Run();
			}
		}
	}

    public void SetMainCameraEnable(bool enable)
    {
        mainCamera.enabled = enabled;
    }

    public void AddToCameraAndFollow(Transform target)
    {
        mainCameraFollow.AddAndFollow(target);
    }

    public void SetUICameraEnable(bool enable)
    {
        uiCamera.gameObject.SetActive(enable);// enable doesn't work on this ne for some reason...
    }


    public void AttachToMainCanvas(Transform ui, bool resizeToFit)
    {
        ui.SetParent(mainCanvas.transform, false);

        if (resizeToFit) ui.GetComponent<RectTransform>().sizeDelta = MainCanvasSize;
    }

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
