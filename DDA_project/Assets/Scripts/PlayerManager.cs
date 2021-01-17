using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManagerInstance = null;

    [SerializeField] private GameObject player;

    private Transform playerSpawn;

    private DifficultyManager difficultyManager;

    // Start of new game info - health and damage
    private const int easyHealth = 150;
    private const int mediumHealth = 100;
    private const int hardHealth = 75;
    private const int easyDamage = 50;
    private const int mediumDamage = 25;
    private const int hardDamage = 15;

    private int startPlayerHealth;
    private int playerHealth = 100;
    private int playerLevel = 1;
    private int playerDamage = 100;
    private int playerExperiencePoints = 0;
    private bool isPlayerDead = false;

    private void Awake() {
        if(playerManagerInstance == null) {
            playerManagerInstance = this;
        } else if(playerManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Update() {
        PlayerRespawn();
    }

    private void Init() {
        // get the difficulty manager object and component
        difficultyManager = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();

        if(difficultyManager.gEasy) {
            setPlayerHealth(easyHealth);
            setPlayerDamage(easyDamage);

            startPlayerHealth = easyHealth;
        } else if(difficultyManager.gMedium) {
            setPlayerHealth(mediumHealth);
            setPlayerDamage(mediumDamage);

            startPlayerHealth = mediumHealth;
        } else if(difficultyManager.gHard) {
            setPlayerHealth(hardHealth);
            setPlayerDamage(hardDamage);

            startPlayerHealth = hardHealth;

        }

        Debug.Log("P health: " + playerHealth);
        Debug.Log("P damage: " + playerDamage);

        spawnPlayer();
    }

    // find playerSpawn location and create player
    private void spawnPlayer() {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        _ = Instantiate(player, playerSpawn.position, playerSpawn.rotation);
    }

    private void PlayerRespawn() {
        if(isPlayerDead) {
            _ = Instantiate(player, playerSpawn.position, playerSpawn.rotation);
            isPlayerDead = false;
            setPlayerHealth(startPlayerHealth);
        }
    }

    // getter and setter functions ====================================================
    public int getPlayerHealth() { return playerHealth; }

    public void setPlayerHealth(int health) { playerHealth = health; }

    public int getPlayerLevel() { return playerLevel; }

    public void setPlayerLevel(int level) { playerLevel = level; }

    public int getPlayerDamage() { return playerDamage; }

    public void setPlayerDamage(int damage) { playerDamage = damage; }

    public int getPlayerExperiencePoints() { return playerExperiencePoints; }

    public void setPlayerExperiencePoints(int exp) { playerExperiencePoints = exp; }

    // this getter may not be needed
    public bool getIsPlayerDead() { return isPlayerDead; }

    public void setIsPlayerDead(bool dead) { isPlayerDead = dead; }

    // ================================================================================
}
