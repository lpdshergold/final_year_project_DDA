using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance = null;

    private PlayerManager pm;
    private DifficultyManager dm;

    // Start of new game info - health and damage
    private const int easyEnemyHealth = 50;
    private const int mediumEnemyHealth = 75;
    private const int hardEnemyHealth = 100;
    private const int easyEnemyDamage = 10;
    private const int mediumEnemyDamage = 15;
    private const int hardEnemyDamage = 20;

    private int enemyHealth = 100;
    private int enemyDamage = 15;
    private float enemyMoveSpeed = 1;

    [SerializeField] private GameObject enemyPrefab;
    public int enemyAmount = 0;
    private int maxEnemyAmount = 10;

    private float spawnTime = 0.0f;
    private float enemySpawnDelay = 3f; 

    private GameObject[] enemySpawns;

    [HideInInspector] public int playerAtkDamage;

    private void Awake() {
        if(enemyManagerInstance == null) {
            enemyManagerInstance = this;
        } else if (enemyManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        dm = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();

        Init();
    }

    private void Start() {
        SpawnEnemies();
    }

    private void Update() {
        Debug.Log(enemyAmount);
        spawnTime += Time.deltaTime;
        SpawnEnemies();
        playerAtkDamage = pm.getPlayerDamage();
    }

    private void Init() {
        GetEnemySpawnLocations();

        if(dm.gEasy) {
            setEnemyHealth(easyEnemyHealth);
            setEnemyDamage(easyEnemyDamage);
        } else if (dm.gMedium) {
            setEnemyHealth(mediumEnemyHealth);
            setEnemyDamage(mediumEnemyDamage);
        } else if (dm.gHard) {
            setEnemyHealth(hardEnemyHealth);
            setEnemyDamage(hardEnemyDamage);
        }

        Debug.Log("E health: " + enemyHealth);
        Debug.Log("E damage: " + enemyDamage);

        playerAtkDamage = pm.getPlayerDamage();
    }

    private void GetEnemySpawnLocations() {
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    // spawn enemy function
    private void SpawnEnemies() {
        // if max enemies are in the game, exit out
        if (enemyAmount >= maxEnemyAmount) { return; }

        if(spawnTime >= enemySpawnDelay) {
            spawnTime = 0.0f;
            while(enemyAmount < maxEnemyAmount) {
                enemyAmount++;

                // get a random enemy spawn with random.range
                GameObject spawn = enemySpawns[Random.Range(0, 4)];
                // create a new enemy gameobject
                _ = Instantiate(enemyPrefab, spawn.transform.position, spawn.transform.rotation);
            }
        }
    }

    // getter and setter functions ====================================================
    public int getEnemyHealth() { return enemyHealth; }

    public void setEnemyHealth(int health) { enemyHealth = health; }

    public int getEnemyDamage() { return enemyDamage; }

    public void setEnemyDamage(int damage) { enemyDamage = damage; }

    public float getEnemyMoveSpeed() { return enemyMoveSpeed; }

    public void setEnemyMoveSpeed(float moveSpeed) { enemyMoveSpeed = moveSpeed; }

    // ================================================================================
}