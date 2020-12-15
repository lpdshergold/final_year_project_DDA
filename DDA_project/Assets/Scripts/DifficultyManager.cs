using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager difficultyManagerInstance = null;

    // Use an Enum or something similar when putting this in the menu
    public bool gEasy = false;
    public bool gMedium = false;
    public bool gHard = true;

    private void Awake() {
        if(difficultyManagerInstance == null) {
            difficultyManagerInstance = this;
        } else if (difficultyManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {

    }
}