using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager difficultyManagerInstance = null;

    private void Awake() {
        if(difficultyManagerInstance == null) {
            difficultyManagerInstance = this;
        } else if (difficultyManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
