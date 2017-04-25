using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SnakeVisuals : ScriptableObject {
    
    /// <summary> Liste des différents aspets visuels disponible dans le jeu. </summary>
    public enum Visuels { Basic, Cactus, Squeletton, TGV, ToyTrain}
    

    public SnakeVisual[] allSnakeParts;


    public SnakeVisual GetVisualFromType(Visuels type) {
        for (int i = 0; i < allSnakeParts.Length; i++) {
            if (allSnakeParts[i].type == type) {
                return allSnakeParts[i];
            }
        }
        return null; // Erreur

    }


}


