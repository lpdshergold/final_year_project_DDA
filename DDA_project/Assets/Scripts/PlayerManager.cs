using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManagerInstance = null;

    [SerializeField] private GameObject player;
    private Player getPlayer;

    private Transform playerSpawn;

    private DifficultyManager dm;
    private Rulebook rulebook;

    private int startPlayerHealth;
    private int playerHealth = 100;
    private float playerMoveSpeed = 3.0f;
    private int playerMaxHealth;
    private int playerLevel = 1;
    private int playerDamage = 100;
    private int playerExperiencePoints = 0;
    private bool isPlayerDead = false;
    public int enemyDamage;

    private void Awake() {
        if(playerManagerInstance == null) {
            playerManagerInstance = this;
        } else if(playerManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        dm = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();
        rulebook = GameObject.Find("DifficultyManager").GetComponent<Rulebook>();

        Init();
    }

    private void Update() {
        PlayerRespawn();

        if(!getPlayer) {
            getPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    private void Init() {
        playerHealth = dm.getPlayerHealth();
        playerMaxHealth = playerHealth;
        playerDamage = dm.getPlayerDamage();
        enemyDamage = dm.getEnemyDamage();

        startPlayerHealth = playerHealth;

        Debug.Log("P health: " + playerHealth);
        Debug.Log("P damage: " + playerDamage);

        spawnPlayer();
    }

    public void updatePlayerDetails(string whatToUpdate = "") {
        if (whatToUpdate == "player") {
            playerHealth = dm.getPlayerHealth();
            playerMaxHealth = dm.getPlayerHealth();
            playerDamage = dm.getPlayerDamage();
        } else if(whatToUpdate == "enemy") {
            Debug.Log("enemy update");
            enemyDamage = dm.getEnemyDamage();
        } else if (whatToUpdate == "all") {
            playerHealth = dm.getPlayerHealth();
            playerMaxHealth = dm.getPlayerHealth();
            playerDamage = dm.getPlayerDamage();
            enemyDamage = dm.getEnemyDamage();
        } else {
            Debug.Log("Didn't need updating = PlayerManager");
        }

        getPlayer.updateDetails();
    }

    // find playerSpawn location and create player
    private void spawnPlayer() {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        _ = Instantiate(player, playerSpawn.position, playerSpawn.rotation);
    }

    private void PlayerRespawn() {
        if(isPlayerDead) {
            _ = Instantiate(player, playerSpawn.position, playerSpawn.rotation);
            rulebook.updatePlayerDeaths();
            isPlayerDead = false;
            setPlayerHealth(startPlayerHealth);
        }
    }

    public void updateEnemyHits() {
        rulebook.updateEnemyDamageHits();
    }

    // getter and setter functions ====================================================
    public int getPlayerHealth() { return playerHealth; }

    public void setPlayerHealth(int health) { playerHealth = health; }

    public int getPlayerMaxHealth() { return playerMaxHealth; }

    public void setPlayerMaxHealth(int health) { playerMaxHealth = health; }

    public int getPlayerLevel() { return playerLevel; }

    public void setPlayerLevel(int level) { playerLevel = level; }

    public int getPlayerDamage() { return playerDamage; }

    public void setPlayerDamage(int damage) { playerDamage = damage; }

    public int getPlayerExperiencePoints() { return playerExperiencePoints; }

    public void setPlayerExperiencePoints(int exp) { playerExperiencePoints = exp; }

    public bool getIsPlayerDead() { return isPlayerDead; }

    public void setIsPlayerDead(bool dead) { isPlayerDead = dead; }

    public float getPlayerMoveSpeed() { return playerMoveSpeed; }

    // ================================================================================
}
