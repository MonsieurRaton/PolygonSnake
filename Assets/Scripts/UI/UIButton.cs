using UnityEngine;
using System.Collections;

public class UIButton : MonoBehaviour
{
    public Animator animator;

    public AnimationClip showButtonClip;
    public AnimationClip disableButtonClip;
    public AnimationClip clickClip;

    public UIMenu parentMenu;
    public bool isBusy;
    public bool isDisabled;


    public void SelectMenu(UIMenu menu)
    {
        if (parentMenu.isLocked || !parentMenu.isActive || isBusy || isDisabled) return;
        if (!parentMenu.NoBusyButton) return;

        isBusy = true;
        StartCoroutine(SelectMenuAnimation(menu));
    }
    IEnumerator SelectMenuAnimation(UIMenu menu)
    {
        yield return StartCoroutine(PlayClickAnimation());

        parentMenu.TransitionToMenu(menu);
    }

    public void PreviousMenu()
    {
        if (parentMenu.isLocked || !parentMenu.isActive || isBusy || isDisabled) return;
        if (!parentMenu.NoBusyButton) return;

        isBusy = true;
        StartCoroutine(PreviousMenuAnimation());
    }
    IEnumerator PreviousMenuAnimation()
    {
        yield return StartCoroutine(PlayClickAnimation());

        parentMenu.TransitionToMenu(null);
    }


    public void LoadScene(int sceneIndex)
    {
        if (parentMenu.isLocked || !parentMenu.isActive || isBusy || isDisabled) return;
        if (!parentMenu.NoBusyButton) return;

        isBusy = true;
        StartCoroutine(LoadSceneAnimation(sceneIndex));
    }
    IEnumerator LoadSceneAnimation(int sceneIndex)
    {
        yield return StartCoroutine(PlayClickAnimation());

        parentMenu.TransitionToScene(sceneIndex);
    }

    IEnumerator PlayClickAnimation()
    {
        animator.Play(clickClip.name);
        yield return new WaitForSeconds(clickClip.length);
    }

    public bool externalSpecialState;
    public void SimpleClick()
    {
        if (parentMenu.isLocked || !parentMenu.isActive || isBusy || isDisabled) return;
        if (!parentMenu.NoBusyButton) return;

        isBusy = true;
        StartCoroutine(PlaySimpleClickAnimation());
    }
    IEnumerator PlaySimpleClickAnimation()
    {
        animator.Play(clickClip.name, 0, 0);
        yield return new WaitForSeconds(clickClip.length);
        isBusy = false;
        externalSpecialState = false;
    }


    public void ShowButton() { animator.Play(showButtonClip.name); isBusy = false; }
    public void HideButton() { animator.Play(showButtonClip.name + "(R)"); isBusy = false; }

    public void DisableButton() { animator.Play(disableButtonClip.name); isBusy = false; }
    public void EnableButton() { animator.Play(disableButtonClip.name + "(R)"); isBusy = false; }
}
