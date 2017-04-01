using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject loadingUIText;
    [SerializeField] private Effect rotationEffect;
    [SerializeField] private GameObject[] visuals;
    
    private Effect effectInstance;
    private GameObject visualInstance;


    public void Init()
    {
        effectInstance = Instantiate(rotationEffect, new Vector3(-5, 0, 0), Quaternion.identity) as Effect;
        visualInstance = Instantiate(visuals[Random.Range(0, visuals.Length)]);
        visualInstance.transform.SetParent(effectInstance.transform);
        visualInstance.transform.localPosition = Vector3.zero;        
    }

    public void Destroy()
    {
        Destroy(loadingUIText.gameObject);
        Destroy(effectInstance.gameObject);
        Destroy(this);
    }
}
