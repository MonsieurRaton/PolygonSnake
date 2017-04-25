using UnityEngine;

[CreateAssetMenu()]
public class SnakeVisual : ScriptableObject {

    /// <summary> Le type de serpent. </summary>
    public SnakeVisuals.Visuels type;

    /// <summary> Le mesh de la tête. </summary>
    public Mesh meshHead;
    /// <summary> Le mesh du corps. </summary>
    public Mesh meshBody;
    /// <summary> Le mesh de la queue. </summary>
    public Mesh meshQueue;

    /// <summary> Le material de ce type de serpent. </summary>
    public Material material;
}