using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private DifficultyManager dm;
    private InitGame initGame;
    private void Start() {
        dm = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();
        initGame = GameObject.Find("DifficultyManager").GetComponent<InitGame>();
    }

    public TextMeshProUGUI dda;

    public void StartGame() {
        if(easyDiff || mediumDiff || hardDiff) {
            initGame.setDDA(isDDAEnabled);
            initGame.setGameDifficulty(easyDiff, mediumDiff, hardDiff);

            SceneManager.LoadScene("ScenarioOne");
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    private bool isDDAEnabled = false;
    public void EnableDisableDDA() {
        if (!isDDAEnabled) {
            isDDAEnabled = true;
            dda.text = "Disable DDA";
        } else {
            isDDAEnabled = false;
            dda.text = "Enable DDA";
        }
    }

    private bool easyDiff = false, mediumDiff = false, hardDiff = false;
    public void EasyDifficulty() {
        easyDiff = true;
        mediumDiff = false;
        hardDiff = false;
    }

    public void MediumDifficulty() {
        easyDiff = false;
        mediumDiff = true;
        hardDiff = false;
    }

    public void HardDifficulty() {
        easyDiff = false;
        mediumDiff = false;
        hardDiff = true;
    }
}
