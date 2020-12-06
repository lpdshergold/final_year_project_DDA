using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance = null;

    private GameManager gm;

    private int enemyHealth = 100;
    private int enemyDamage = 15;
    private float enemyMoveSpeed = 1;

    [SerializeField] private GameObject enemyPrefab;
    public int enemyAmount = 0;
    private int maxEnemyAmount = 10;

    private float enemySpawnDelay = 1f; 

    private GameObject[] enemySpawns;

    [HideInInspector] public int playerAtkDamage;

    private void Awake() {
        if(enemyManagerInstance == null) {
            enemyManagerInstance = this;
        } else if (enemyManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // THIS WILL NEED TO BE ADDED TO UPDATE FUNCTION FOR WHEN THE DAMAGE IS INCREASED
        // get player attack damager
        playerAtkDamage = gm.getPlayerDamage();

        Init();
    }

    private void Start() {
        SpawnEnemies();
    }

    private void Update() {
        SpawnEnemies();
    }

    private void Init() {
        GetEnemySpawnLocations();
    }

    private void GetEnemySpawnLocations() {
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    // spawn enemy function
    private void SpawnEnemies() {
        // if max enemies are in the game, exit out
        if (enemyAmount >= maxEnemyAmount) { return; }

        while (enemyAmount < maxEnemyAmount) {
            enemyAmount++;

            // get a random enemy spawn with random.range
            GameObject spawn = enemySpawns[Random.Range(0, 4)];
            // create a new enemy gameobject
            _ = Instantiate(enemyPrefab, spawn.transform.position, spawn.transform.rotation);
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