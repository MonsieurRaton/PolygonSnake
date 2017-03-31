using UnityEngine;
using System.Collections;

public class Speed : MonoBehaviour
{
    public enum Type
    {
        Extreme,
        VeryFast,
        Fast,
        Normal,
        Slow,
        VerySlow
    }
    [Space(10)]
    public Type type = Type.Normal;

    [Header("Definitions")]
    public int Extreme = 4;
    public int VeryFast = 6;
    public int Fast = 8;
    public int Normal = 12;
    public int Slow = 20;
    public int VerySlow = 30;

    [Header("Debug (Visual)")]
    public int step;

    public int GetValue(Type type)
    {
        if (type == Type.Extreme) return Extreme;
        if (type == Type.VeryFast) return VeryFast;
        if (type == Type.Fast) return Fast;
        if (type == Type.Normal) return Normal;
        if (type == Type.Slow) return Slow;
        if (type == Type.VerySlow) return VerySlow;
        return 0;
    }

    public void IncreaseStep()
    {
        step++;
        if (step > GetValue(type)) StepToMax();
    }
    public void ResetStep() { step = 0; }
    public void StepToMax() { step = GetValue(type); }

    public float NormalizedRatio { get { return (GetValue(Type.Normal) * 1f) / (GetValue(type) * 1f); } }
    public float StepSpeedRatio { get { return (step * 1f) / (GetValue(type) * 1f); } }
    public bool StepIsAtMax { get { return step >= GetValue(type); } }
}
