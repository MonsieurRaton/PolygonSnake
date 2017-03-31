using UnityEngine;
using System.Collections.Generic;

public class SnakeVisual : MonoBehaviour
{
    new Animation animation;
    float speed = 1;
    int selected = 0;
    List<string> names = new List<string>();


    void Awake()
    {
        animation = GetComponent<Animation>();

        foreach (AnimationState a in animation)
        {
            names.Add(a.name);
            if (animation.clip.name == names[names.Count - 1]) selected = names.Count - 1;
        }
    }


    public void SetSpeed(float speed)
    {
        this.speed = speed;
        animation[names[selected]].speed = this.speed;
    }
}
