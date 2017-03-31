using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    const string AnimName_Spawn = "Spawn";
    const string AnimName_Idle = "Idle";
    const string AnimName_Picked = "Picked";
    const string AnimName_Die = "Die";

    [Header("Setup")]
    public Animation visual;

    [Header("Values")]
    public bool snakeSizeChange;
    [Range(1, 9)]
    public int snakeSizeValue = 1;

    [Space(5)]
    public bool scoreChange;
    public int scoreValue = 100;

    [Space(5)]
    public bool timeChange;
    public float timeValue = 5;

    [Space(5)]
    public bool speedChange = false;
    public Speed.Type speedType = Speed.Type.Normal;
    public int speedTileDuration;


    bool isAvailable;
    bool canBePicked;
    public IItemSimpleEvent ItemSimpleEvent { get; set; }


    public void Spawn()
    {
        gameObject.SetActive(true);
        isAvailable = true;
        canBePicked = true;
        StartCoroutine(SpawnCoroutine());
    }

    public void Pick()
    {
        if (!isAvailable || !canBePicked) return;

        isAvailable = false;
        canBePicked = false;

        StartCoroutine(PickCoroutine());
    }

    public void Kill()
    {
        if (!isAvailable) return;

        isAvailable = false;

        StartCoroutine(KillCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        if (ItemSimpleEvent != null) ItemSimpleEvent.OnItemSpawn(this);

        visual.Play(AnimName_Spawn);
        yield return new WaitForSeconds(visual[AnimName_Spawn].length);

        if (isAvailable) visual.Play(AnimName_Idle);
    }

    IEnumerator PickCoroutine()
    {
        if (ItemSimpleEvent != null) ItemSimpleEvent.OnItemPicked(this);

        visual.Play(AnimName_Picked);
        yield return new WaitForSeconds(visual[AnimName_Picked].length);

        if (ItemSimpleEvent != null) ItemSimpleEvent.OnItemDisable(this);
        gameObject.SetActive(false);
    }

    IEnumerator KillCoroutine()
    {
        if (ItemSimpleEvent != null) ItemSimpleEvent.OnItemDie(this);

        visual.Play(AnimName_Die);
        yield return new WaitForSeconds(visual[AnimName_Die].length);

        if (ItemSimpleEvent != null) ItemSimpleEvent.OnItemDisable(this);
        gameObject.SetActive(false);
    }
}
