using System.Collections;
using System.Collections.Generic;
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

    private string weightLvl = "";

    private int enemiesKilled = 0;
    private int enemyDamageHits = 0;
    private int enemySpawnRate = 0;
    private int playerDamageHits = 0;
    private int playerHealth = 0;
    private int playerMaxHealth = 0;
    private int playerDeaths = 0;

    private int rulebookEnemiesKilled = 0;
    private int rulebookEnemyDamageHits = 0;
    private int rulebookEnemySpawnRate = 0;
    private int rulebookPlayerDamageHits = 0;
    private int rulebookPlayerHealth = 0;
    private int rulebookPlayerMaxHealth = 0;

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

        if(isDDAEnabled) {
            RunScenarios();
        }

/*        if(levelup && passOncePerLevel) {
            passOncePerLevel = false;

            playerHealth = pm.getPlayerHealth();
            playerMaxHealth = pm.getPlayerMaxHealth();
            enemySpawnRate = em.getEnemySpawnRate();

            weightSystem.passWeightDetails(playerHealth, playerMaxHealth, playerDeaths, playerDamageHits, enemyDamageHits, enemySpawnRate);
            ResetVaraibles();
        }

        if(levelup && isDDAEnabled && weightLvl != "") {
            levelup = false;
            multiplier.ddaLevelup(weightLvl);
            weightLvl = "";
        } else if (levelup && !isDDAEnabled) {
            levelup = false;
            multiplier.basicLevelUp();
        }*/
    }

    private void ResetVaraibles() {
        playerHealth = 0;
        playerMaxHealth = 0;
        playerDeaths = 0;
        playerDamageHits = 0;
        enemyDamageHits = 0;
    }

    public void setDDA(bool enable) { isDDAEnabled = enable; }

    public void setPassOncePerLvl(bool isPassed) { passOncePerLevel = isPassed; }

    public void setLevelup(bool plevel) { levelup = plevel; }

    public void setWeightLvl( string weight) { weightLvl = weight; }

    public void updateEnemiesKilled() { enemiesKilled++; rulebookEnemiesKilled++; }

    public void updatePlayerDeaths() { playerDeaths++; }

    public void updatePlayerDamageHits() { playerDamageHits++; rulebookPlayerDamageHits++; }

    public void updateEnemyDamageHits() { enemyDamageHits++; rulebookEnemyDamageHits++; }

    // Rulebook scenarios
    /*
     * Things to add to rulebook - scenarios
     * if the player kills a certain amount of enemies without dying increase enemy damage
     * if player either levels up or doesn't get hit over a period of time - increase enemy speed
     * if the player is consistenly dying reduce enemy difficulty
     * if a player dies a certain amount of times before levelling up do something - this is probably a weight system thing
     * if the enemies don't do a certain amount of damage over a period of time - increae enemy amount
     */

    private void RunScenarios() {
        PlayerKillsDeathsCheck();
        PlayerHitCheck();
        PlayerTooManyDeaths();
        PlayerLowDamage();
    }

    // check if the player kills too many enemies before dying - update enemies damage + health + ememy amount
    private int playerDeathCheck = 0;
    private int playerKillsDeathCounter = 0;
    private void PlayerKillsDeathsCheck() {
        if (playerDeathCheck == playerDeaths) {
            if (playerKillsDeathCounter >= 10) {
                playerKillsDeathCounter = 0;
                rulebookEnemiesKilled = 0;

                multiplier.UpdateEnemyDamageHealthAmount();

            } else {
                playerKillsDeathCounter = rulebookEnemiesKilled;
            }
        } else {
            playerDeathCheck = playerDeaths;

            playerKillsDeathCounter = 0;
            rulebookEnemiesKilled = 0;
        }
    }

    // check to see if the enemy hasn't hit the player over a period of time - increase enemy speed + emeny amount
    private void PlayerHitCheck() {

    }

    // if the player dies too much, reduce the enemy damage + move speed
    private void PlayerTooManyDeaths() {

    }

    // if the enemies have not hit the player in a specific amount of time - increase enemy amount
    private void PlayerLowDamage() {

    }
}
