using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManagerInstance = null;

    [SerializeField] private GameObject player;

    private Transform playerSpawn;

    private DifficultyManager dm;
    private Rulebook rulebook;

    private int startPlayerHealth;
    private int playerHealth = 100;
    private int playerMaxHealth;
    private int playerLevel = 1;
    private int playerDamage = 100;
    private int playerExperiencePoints = 0;
    private bool isPlayerDead = false;
    public int enemyDamage;

    [HideInInspector] public bool updateDetails = false;

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
        
        if (updateDetails) {
            updateDetails = false;
            updatePlayerDetails();
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

    private void updatePlayerDetails() {
        playerHealth = dm.getPlayerHealth();
        playerMaxHealth = dm.getPlayerHealth();
        playerDamage = dm.getPlayerDamage();
        enemyDamage = dm.getEnemyDamage();

        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        p.updateDetails();
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

    // ================================================================================
}
