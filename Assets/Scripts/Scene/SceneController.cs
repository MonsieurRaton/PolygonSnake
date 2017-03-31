using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public virtual void Load() { }
    public virtual void Run() { }
    
    public bool IsRunning { get; set; }
    public bool HasLoaded { get; set; }
    public bool GameOver { get; set; }

    public bool CanBeStarted { get { return HasLoaded && GameOver; } }
}
