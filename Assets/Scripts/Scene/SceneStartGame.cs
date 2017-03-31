using UnityEngine;
using System.Collections;

public class SceneStartGame : MonoBehaviour
{
    public Canvas canvas;
    public UIMenu mainMenu;

    void Start()
    {
        StartCoroutine(StartMenu());
    }

    IEnumerator StartMenu()
    {
        foreach(Transform t in canvas.transform)
        {
            UIMenu menu = t.GetComponent<UIMenu>();
            if (menu == null) continue;

            for (int i = 0; i < menu.buttons.Length; i++) menu.buttons[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        mainMenu.ShowMenu();
    }
}
