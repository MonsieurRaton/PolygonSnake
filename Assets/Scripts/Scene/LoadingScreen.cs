using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject loadingUI;
    public Effect rotationEffect;
    public GameObject[] visuals;

    GameObject uiInstance;
    Effect effectInstance;
    GameObject visualInstance;


    public void Init()
    {
        uiInstance = Instantiate(loadingUI);
        effectInstance = (Effect)Instantiate(rotationEffect, Vector3.zero, Quaternion.identity);
        visualInstance = Instantiate(visuals[Random.Range(0, visuals.Length)]);

        GameController.instance.AttachToMainCanvas(uiInstance.transform,true);
        visualInstance.layer = LayerMask.NameToLayer("Effects");
        visualInstance.transform.parent = effectInstance.transform;
    }

    public void Destroy()
    {
        Destroy(uiInstance);
        Destroy(effectInstance.gameObject);
    }
}
