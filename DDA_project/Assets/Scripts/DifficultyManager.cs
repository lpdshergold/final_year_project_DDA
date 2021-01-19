using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager difficultyManagerInstance = null;

    private int playerHealth, playerDamage, enemyHealth, enemyDamage;
    private float enemyMoveSpeed;

    private bool isDDAEnabled = false;
    private string gameDifficulty = "";

    private void Awake() {
        if(difficultyManagerInstance == null) {
            difficultyManagerInstance = this;
        } else if (difficultyManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Getter and Setters
    public int getPlayerHealth() { return playerHealth; }

    public void setPlayerHealth(int health) { playerHealth = health; }

    public int getPlayerDamage() { return playerDamage; }

    public void setPlayerDamage(int damage) { playerDamage = damage; }

    public int getEnemyHealth() { return enemyHealth; }

    public void setEnemyHealth(int health) { enemyHealth = health; }

    public int getEnemyDamage() { return enemyDamage; }

    public void setEnemyDamage(int damage) { enemyDamage = damage; }

    public float getEnemyMoveSpeed() { return enemyMoveSpeed; }

    public void setEnemyMoveSpeed(float moveSpeed) { enemyMoveSpeed = moveSpeed; }

    public bool getDDAEnabled() { return isDDAEnabled; }

    public void setDDAEnabled(bool enable) { isDDAEnabled = enable; }

    public string getGameDifficulty() { return gameDifficulty; }

    public void setGameDifficulty(string difficulty) { gameDifficulty = difficulty; }
}