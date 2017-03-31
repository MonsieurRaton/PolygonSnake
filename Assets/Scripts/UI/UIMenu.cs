using UnityEngine;
using System.Collections;

public class UIMenu : MonoBehaviour
{
    public Animator animator;
    public AnimationClip showMenuClip;
    public AnimationClip disableMenuClip;

    [Space(5)]
    public UIMenu escapeKeyMenu;

    [Space(5)]
    public UIButton[] buttons;
    
    [Header("Debug Visual")]
    public UIMenu previousMenuCache;

    public bool isLocked;
    public bool isBusy;
    public bool isActive;
    bool IsAvailable
    {
        get
        {
            if (isBusy) return false;
            isBusy = true;
            return true;
        }
    }

    void Update()
    {
        if (isLocked || isBusy || !isActive) return;

        if (Input.GetKeyDown(KeyCode.Escape)) TransitionToMenu(escapeKeyMenu);
    }


    public void ShowMenu()
    {
        if (!IsAvailable) return;
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        StartCoroutine(ShowMenuAnimation());
    }
    IEnumerator ShowMenuAnimation()
    {
        animator.speed = 1;
        animator.Play(showMenuClip.name);
        yield return new WaitForSeconds(showMenuClip.length);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].ShowButton();
            yield return new WaitForSeconds(buttons[i].showButtonClip.length);
        }

        isActive = true;
        isBusy = false;
    }

    public void HideMenu()
    {
        if (!IsAvailable) return;
        isActive = false;
        StartCoroutine(HideMenuAnimation());
    }
    IEnumerator HideMenuAnimation()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].HideButton();
            yield return new WaitForSeconds(buttons[i].showButtonClip.length);
            buttons[i].gameObject.SetActive(false);
        }

        animator.Play(showMenuClip.name + "(R)");
        yield return new WaitForSeconds(showMenuClip.length);
        
        isBusy = false;
    }

    public void DisableMenu()
    {
        if (!IsAvailable) return;
        isActive = false;
        StartCoroutine(DisableMenuAnimation());
    }
    IEnumerator DisableMenuAnimation()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].DisableButton();
            buttons[i].isDisabled = true;
        }

        animator.Play(disableMenuClip.name);
        yield return new WaitForSeconds(disableMenuClip.length);

        isActive = false;
        isBusy = false;
    }

    public void EnableMenu()
    {
        if (!IsAvailable) return;
        StartCoroutine(EnableMenuAnimation());
    }
    IEnumerator EnableMenuAnimation()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].EnableButton();
            buttons[i].isDisabled = false;
        }

        animator.Play(disableMenuClip.name + "(R)");
        yield return new WaitForSeconds(disableMenuClip.length);

        isActive = true;
        isBusy = false;
    }


    public void TransitionToMenu(UIMenu menu)
    {
        if (!IsAvailable) return;

        if (menu == null && previousMenuCache == null)
        {
            TransitionToScene(-1);
            return;
        }

        if (menu) StartCoroutine(ToNextMenu(menu)); else StartCoroutine(ToPreviousMenu());
    }
    IEnumerator ToNextMenu(UIMenu menu)
    {
        StartCoroutine(DisableMenuAnimation());

        menu.previousMenuCache = this;
        menu.ShowMenu();

        yield return new WaitForEndOfFrame();
    }
    IEnumerator ToPreviousMenu()
    {
        isActive = false;

        yield return StartCoroutine(HideMenuAnimation());

        previousMenuCache.EnableMenu();
        previousMenuCache = null;
    }


    public void TransitionToScene(int index)
    {
        if (isLocked) return;
        isLocked = true;
        StartCoroutine(NextScene(index));
    }
    IEnumerator NextScene(int sceneIndex)
    {
        yield return StartCoroutine(HideMenuAnimation());

        if (previousMenuCache != null)
        {
            previousMenuCache.TransitionToScene(sceneIndex);
            yield break;
        }

        SceneLoader.LoadScene(sceneIndex);
    }


    public bool NoBusyButton
    {
        get
        {
            for (int i = 0; i < buttons.Length; i++) if (buttons[i].isBusy) return false;
            return true;
        }
    }
}
