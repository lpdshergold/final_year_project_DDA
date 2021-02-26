using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance = null;

    private PlayerManager pm;
    private DifficultyManager dm;
    private Rulebook rulebook;

    private int enemyHealth = 100;
    private int enemyDamage = 15;
    private float enemyMoveSpeed = 2.0f;

    [SerializeField] private GameObject enemyPrefab;
    public int enemyAmount = 0;
    private int maxEnemyAmount = 10;
    private int maxMaxEnemyAmount = 25;

    private float spawnTime = 0.0f;
    private float enemySpawnDelay = 3f; 

    public GameObject[] enemySpawns;

    public bool resetEnemyAmount = false;

    [HideInInspector] public int playerAtkDamage;

    [HideInInspector] public bool updateDetails = false;

    private void Awake() {
        if(enemyManagerInstance == null) {
            enemyManagerInstance = this;
        } else if (enemyManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        dm = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();
        rulebook = GameObject.Find("DifficultyManager").GetComponent<Rulebook>();

        Init();
    }

    private void Start() {
        SpawnEnemies();
    }

    private void Update() {
        spawnTime += Time.deltaTime;

        SpawnEnemies();

        playerAtkDamage = pm.getPlayerDamage();

        if(updateDetails) {
            updateDetails = false;
            updatePlayerDetails();
        }

        if(enemySpawns[0] == null) {
            GetEnemySpawnLocations();
            if (resetEnemyAmount == true) {
                enemyAmount = 0;
                resetEnemyAmount = false;
            }
        }
    }

    private void Init() {
        GetEnemySpawnLocations();

        enemyHealth = dm.getEnemyHealth();
        enemyDamage = dm.getEnemyDamage();
        enemyMoveSpeed = dm.getEnemyMoveSpeed();
        maxEnemyAmount = dm.getEnemySpawnAmount();


        playerAtkDamage = pm.getPlayerDamage();
    }

    private void GetEnemySpawnLocations() {
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    // spawn enemy function
    private int saveSpawnPos = 0;
    private void SpawnEnemies() {
        // if max enemies are in the game, exit out
        if (enemyAmount >= maxEnemyAmount) { return; }

        if(spawnTime >= enemySpawnDelay) {
            spawnTime = 0.0f;
            if (maxEnemyAmount != maxMaxEnemyAmount && maxEnemyAmount < maxMaxEnemyAmount) {
                while(enemyAmount < maxEnemyAmount) {
                    for (int i = saveSpawnPos; i <= enemySpawns.Length; i++) {
                        if (saveSpawnPos == enemySpawns.Length) {
                            saveSpawnPos = 0;
                            i = 0;
                        } else { saveSpawnPos++; }

                        if (enemyAmount >= maxEnemyAmount) { return; }

                        enemyAmount++;

                        // get the spawn location
                        GameObject spawn = enemySpawns[i];

                        // create a new enemy gameobject
                        _ = Instantiate(enemyPrefab, spawn.transform.position, spawn.transform.rotation);
                    }
                }
            } else {
                maxEnemyAmount = maxMaxEnemyAmount;
            }
        }
    }

    private void updatePlayerDetails() {
        enemyHealth = dm.getEnemyHealth();
        enemyDamage = dm.getEnemyDamage();
        enemyMoveSpeed = dm.getEnemyMoveSpeed();
    }

    public void updateRuleBookEnemiesKilled() {
        rulebook.updateEnemiesKilled();
    }

    public void updatePlayerHits() {
        rulebook.updatePlayerDamageHits();
    }

    // getter and setter functions ====================================================
    public int getEnemyHealth() { return enemyHealth; }

    public void setEnemyHealth(int health) { enemyHealth = health; }

    public int getEnemyDamage() { return enemyDamage; }

    public void setEnemyDamage(int damage) { enemyDamage = damage; }

    public float getEnemyMoveSpeed() { return enemyMoveSpeed; }

    public void setEnemyMoveSpeed(float moveSpeed) { enemyMoveSpeed = moveSpeed; }    
    
    public int getEnemySpawnRate() { return maxEnemyAmount; }

    public void setEnemySpawnRate(int newMaxEnemyAmount) { maxEnemyAmount += newMaxEnemyAmount; }

    public int getEnemyMaxMaxAmount() { return maxMaxEnemyAmount; }
    // ================================================================================
}