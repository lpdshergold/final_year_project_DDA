using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rulebook : MonoBehaviour
{
    private PlayerManager pm;
    private EnemyManager em;
    private Multiplier multiplier;
    private WeightSystem weightSystem;

    private bool isDDAEnabled = false;
    private bool levelup = false;
    private bool doOnce = false;
    private bool passOncePerLevel = false;

    private string playerWeightLvl = "";
    private string enemyWeightLvl = "";

    private int enemiesKilled = 0;
    private int enemyDamageHits = 0;
    private int enemySpawnRate = 0;
    private int playerDamageHits = 0;
    private int playerHealth = 0;
    private int playerMaxHealth = 0;
    private int playerDeaths = 0;

    private int rulebookEnemiesKilled = 0;
    private int rulebookEnemyDamageHits = 0;
    private int rulebookPlayerDamageHits = 0;
    private int rulebookIsPlayerNotHitEnough = 0;
    private int rulebookPlayerDeath = 0;

    public bool updateRulebook = false;

    void Start()
    {
        multiplier = gameObject.GetComponent<Multiplier>();
        weightSystem = gameObject.GetComponent<WeightSystem>();
    }

    void Update()
    {
        if(GameObject.Find("PlayerManager") && GameObject.Find("EnemyManager") &&  !doOnce) {
            pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            doOnce = true;
        }

        if(isDDAEnabled && updateRulebook) {
            // death checks
            PlayerTooManyDeaths();
            PlayerKillsDeathsCheck();
            // timer checks
            PlayerHitCheck();
            TooManyPlayerHits();
            NotEnoughEnemyDamage();
        }

        if(levelup && isDDAEnabled && passOncePerLevel) {
            passOncePerLevel = false;

            playerHealth = pm.getPlayerHealth();
            playerMaxHealth = pm.getPlayerMaxHealth();
            enemySpawnRate = em.getEnemySpawnRate();

            weightSystem.passWeightDetails(playerHealth, playerMaxHealth, playerDeaths, playerDamageHits, enemyDamageHits, enemySpawnRate);
            ResetVaraibles();
        }

        if(levelup && isDDAEnabled && (playerWeightLvl != "" && enemyWeightLvl != "")) {
            levelup = false;
            multiplier.ddaLevelup(playerWeightLvl, enemyWeightLvl);
            playerWeightLvl = "";

        } else if (levelup && !isDDAEnabled) {
            levelup = false;
            multiplier.basicLevelUp();
        }
    }

    private void ResetVaraibles() {
        playerHealth = 0;
        playerMaxHealth = 0;
        playerDeaths = 0;
        playerDamageHits = 0;
        enemyDamageHits = 0;
    }

    // Rulebook scenarios ===================================================================================

    // scenario 1
    // check if the player kills too many enemies before dying - update enemies damage + health + ememy amount
    private int playerDeathCheck = 0;
    private int playerKillsDeathCounter = 0;
    private void PlayerKillsDeathsCheck() {
        if (playerDeathCheck == playerDeaths) {
            if (playerKillsDeathCounter >= 20) {
                playerKillsDeathCounter = 0;
                rulebookEnemiesKilled = 0;

                multiplier.UpdateEnemyDamageHealthAmount();
                Debug.Log("RB S1: Updating enemy damage and health");

            } else {
                playerKillsDeathCounter = rulebookEnemiesKilled;
            }
        } else {
            playerDeathCheck = playerDeaths;

            playerKillsDeathCounter = 0;
            rulebookEnemiesKilled = 0;
        }
    }

    // scenario 2
    // if the player dies too much, reduce the enemy damage + move speed
    private float playerDeathTimer = 0.0f;
    private void PlayerTooManyDeaths() {
        playerDeathTimer += Time.deltaTime;

        if (playerDeathTimer >= 30.0 || rulebookPlayerDeath > 4) {
            if (rulebookPlayerDeath >= 4) {
                Debug.Log("RB S2: Reducing enemy damage: High");
                multiplier.UpdateEnemyDamage(-15);
            } else if (rulebookPlayerDeath == 3) {
                Debug.Log("RB S2: Reducing enemy damage: Medium");
                multiplier.UpdateEnemyDamage(-10);
            } else if (rulebookPlayerDeath == 2) {
                Debug.Log("RB S2: Reducing enemy damage: Low");
                multiplier.UpdateEnemyDamage(-5);
            }

            rulebookPlayerDeath = 0;
            playerDeathTimer = 0.0f;
        }
    }

    // scenario 3
    // check to see if the enemy hasn't hit the player over a period of time - increase enemy speed + enemy amount
    private float playerHitTimer = 0.0f;
    private int enemyHitPlayer = 0;
    private void PlayerHitCheck() {
        playerHitTimer += Time.deltaTime;

        if(enemyHitPlayer == enemyDamageHits) {
            if(playerHitTimer >= 15.0f) {
                playerHitTimer = 0.0f;
                Debug.Log("RB S3: Increase enemy amount and speed");
                multiplier.TimerUpdateEnemySpeed(0.1f);
            }
        } else {
            playerHitTimer = 0.0f;
            enemyHitPlayer = enemyDamageHits;
        }
    }

    // scenario 4
    // check to see if the player is hit a specific amount in under 5 seconds and reduce the enemy speed
    private float tooManyPlayerHitsTimer = 0.0f;
    private int isPlayerHitTooMuch = 0;
    private void TooManyPlayerHits() {
        tooManyPlayerHitsTimer += Time.deltaTime;

        if (isPlayerHitTooMuch >= 4) {
            tooManyPlayerHitsTimer = 0.0f;
            isPlayerHitTooMuch = 0;
            rulebookEnemyDamageHits = 0;

            Debug.Log("RB S4: Reduce enemy speed");
            multiplier.TimerUpdateEnemySpeed(-0.25f);
        } else {
            isPlayerHitTooMuch = rulebookEnemyDamageHits;

            if (tooManyPlayerHitsTimer >= 10.0f) {
                tooManyPlayerHitsTimer = 0.0f;
                isPlayerHitTooMuch = 0;
                rulebookEnemyDamageHits = 0;
            }
        }
    }

    // scenario 5
    // if the enemies don't do a certain amount of damage over a period of time - increase enemy amount
    private float notEnoughEnemyDamageTimer = 0.0f;
    private void NotEnoughEnemyDamage() {
        // do a while loop around this when enemy sight is activated for any enemy
        notEnoughEnemyDamageTimer += Time.deltaTime;

        if(notEnoughEnemyDamageTimer >= 30.0f) {

            if(rulebookIsPlayerNotHitEnough <= 1) {
                Debug.Log("RB S5: Increase enemy amount: High");
                multiplier.UpdateEnemySpawnAmount(3);
            } else if (rulebookIsPlayerNotHitEnough <= 3) {
                Debug.Log("RB S5: Increase enemy amount: Medium");
                multiplier.UpdateEnemySpawnAmount(2);
            } else if (rulebookIsPlayerNotHitEnough <= 5) {
                Debug.Log("RB S5: Increase enemy amount: Low");
                multiplier.UpdateEnemySpawnAmount(1);
            }

            notEnoughEnemyDamageTimer = 0.0f;
            rulebookIsPlayerNotHitEnough = 0;
        }
    }

    // End of Rulebook scenarios ============================================================================

    public void setDDA(bool enable) { isDDAEnabled = enable; }

    public void setPassOncePerLvl(bool isPassed) { passOncePerLevel = isPassed; }

    public void setLevelup(bool plevel) { levelup = plevel; }

    public void setWeightLvl(string playerWeighting, string enemyWeighting) { playerWeightLvl = playerWeighting; enemyWeightLvl = enemyWeighting; }

    public void updateEnemiesKilled() { enemiesKilled++; rulebookEnemiesKilled++; }

    public void updatePlayerDeaths() { playerDeaths++; rulebookPlayerDeath++; }

    public void updatePlayerDamageHits() { playerDamageHits++; rulebookPlayerDamageHits++; }

    public void updateEnemyDamageHits() { enemyDamageHits++; rulebookEnemyDamageHits++; rulebookIsPlayerNotHitEnough++; }
}
