using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    DifficultyManager dm;

    public bool gEasy = false;
    public bool gMedium = false;
    public bool gHard = false;

    // player information
    private const int easyHealth = 150;
    private const int mediumHealth = 100;
    private const int hardHealth = 75;
    private const int easyDamage = 50;
    private const int mediumDamage = 25;
    private const int hardDamage = 15;

    // enemy information 
    private const int easyEnemyHealth = 50;
    private const int mediumEnemyHealth = 75;
    private const int hardEnemyHealth = 100;
    private const int easyEnemyDamage = 10;
    private const int mediumEnemyDamage = 15;
    private const int hardEnemyDamage = 20;

    private bool isDDAEnabled = false;
    private bool sendOnce = false;

    private void Awake() {
        dm = GetComponent<DifficultyManager>();
    }

    private void Start() {

    }

    private void Update() {
        sendData();
    }

    private void sendData() {
        if(gEasy && !sendOnce) {
            dm.setPlayerHealth(easyHealth);
            dm.setPlayerDamage(easyDamage);
            dm.setEnemyHealth(easyEnemyHealth);
            dm.setEnemyDamage(easyEnemyDamage);
            dm.setDDAEnabled(isDDAEnabled);
            dm.updateDDAElsewhere(isDDAEnabled);
            dm.setGameDifficulty("easy");
            sendOnce = true;
        } else if(gMedium && !sendOnce) {
            dm.setPlayerHealth(mediumHealth);
            dm.setPlayerDamage(mediumDamage);
            dm.setEnemyHealth(mediumEnemyHealth);
            dm.setEnemyDamage(mediumEnemyDamage);
            dm.setDDAEnabled(isDDAEnabled);
            dm.updateDDAElsewhere(isDDAEnabled);
            dm.setGameDifficulty("medium");
            sendOnce = true;
        } else if(gHard && !sendOnce) {
            dm.setPlayerHealth(hardHealth);
            dm.setPlayerDamage(hardDamage);
            dm.setEnemyHealth(hardEnemyHealth);
            dm.setEnemyDamage(hardEnemyDamage);
            dm.setDDAEnabled(isDDAEnabled);
            dm.updateDDAElsewhere(isDDAEnabled);
            dm.setGameDifficulty("hard");
            sendOnce = true;
        }
    }

    // Getters and Setters
    public void setGameDifficulty(bool easy, bool medium, bool hard) {
        if(easy) {
            gEasy = true;
        } else if(medium) {
            gMedium = true;
        } else if(hard) {
            gHard = true;
        }
    }

    public void setDDA(bool checkDDA) {
        isDDAEnabled = checkDDA;
    }
}
