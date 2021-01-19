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
        spawnTime += Time.deltaTime;

        SpawnEnemies();

        playerAtkDamage = pm.getPlayerDamage();
    }

    private void Init() {
        GetEnemySpawnLocations();

        enemyHealth = dm.getEnemyHealth();
        enemyDamage = dm.getEnemyDamage();


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