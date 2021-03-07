using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    private DifficultyManager dm;
    private PlayerManager pm;
    private EnemyManager em;

    private double basicMultiplier = 1.05;

    private double smallWeightMultiplier = 1.03;
    private double mediumWeightMultiplier = 1.05;
    private double highWeightMultiplier = 1.07;

    private bool doOnce = false;

    void Start()
    {
        dm = gameObject.GetComponent<DifficultyManager>();
    }

    void Update()
    {
        if(GameObject.Find("PlayerManager") && GameObject.Find("EnemyManager") && !doOnce) {
            pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            doOnce = true;
        }

    }

    public void basicLevelUp() {
        Debug.Log("Static Difficulty: running static mutliplier increase");
        basicMultiplierCal();
    }

    public void ddaLevelup(string pWeight, string eWeight) {
        if (pWeight != "" || pWeight != "none") {
            Debug.Log("Player multiplier increase: " + pWeight);
            playerMultiplierCal(pWeight);
        } else {
            Debug.Log("No player multiplier increase");
        }

        if(eWeight != "" || eWeight != "none") {
            Debug.Log("Enemy multiplier increase: " + eWeight);
            enemyMultiplayerCal(eWeight);
        } else {
            Debug.Log("No enemy multiplier increase");
        }
    }

    private void basicMultiplierCal() {
        int pHealth = dm.getPlayerHealth();
        int pDamage = dm.getPlayerDamage();
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();
        int eSpawn = dm.getEnemySpawnAmount();

        int tempHealth = (int)(pHealth * basicMultiplier);
        dm.setPlayerHealth(tempHealth);

        int tempDamage = (int)(pDamage * basicMultiplier);
        dm.setPlayerDamage(tempDamage);

        int tempEHealth = (int)(eHealth * basicMultiplier);
        dm.setEnemyHealth(tempEHealth);

        int tempEDamage = (int)(eDamage * basicMultiplier);
        dm.setEnemyDamage(tempEDamage);

        dm.setEnemySpawnAmount(eSpawn + 1);

        Debug.Log("Previous Player Health: " + pHealth + ", New Player Health: " + tempHealth);
        Debug.Log("Previous Player Damage: " + pDamage + ", New Player Damage: " + tempDamage);
        Debug.Log("Previous Enemy Health: " + eHealth + ", New Enemy Health: " + tempEHealth);
        Debug.Log("Previous Enemy Damage: " + eDamage + ", New Enemy Damage: " + tempEDamage);
        Debug.Log("Previous Enemy Spawn Amount: " + eSpawn + ", New Enemy Spawn Amount: " + (eSpawn + 1));

        updateManagers("all"); 
    }

    private void playerMultiplierCal(string mulWeight = "") {
        int pHealth = dm.getPlayerHealth();
        int pDamage = dm.getPlayerDamage();
        int tempHealth = 0;
        int tempDamage = 0;

        if (mulWeight == "") {
            return;

        } else if (mulWeight == "small") {

            tempHealth = (int)(pHealth * smallWeightMultiplier);
            dm.setPlayerHealth(tempHealth);
            pm.setStartPlayerHealth(tempHealth);

            tempDamage = (int)(pDamage * smallWeightMultiplier);
            dm.setPlayerDamage(tempDamage);

        } else if (mulWeight == "medium") {

            tempHealth = (int)(pHealth * mediumWeightMultiplier);
            dm.setPlayerHealth(tempHealth);
            pm.setStartPlayerHealth(tempHealth);

            tempDamage = (int)(pDamage * mediumWeightMultiplier);
            dm.setPlayerDamage(tempDamage);

        } else if(mulWeight == "high") {

            tempHealth = (int)(pHealth * highWeightMultiplier);
            dm.setPlayerHealth(tempHealth);
            pm.setStartPlayerHealth(tempHealth);

            tempDamage = (int)(pDamage * highWeightMultiplier);
            dm.setPlayerDamage(tempDamage);

        }
        
        if (tempHealth < pHealth && tempDamage < pDamage) {
            tempHealth = pHealth;
            tempDamage = pDamage;
        } else if (tempHealth < pHealth) {
            tempHealth = pHealth;
        } else if (tempDamage < pDamage) {
            tempDamage = pDamage;
        }

        Debug.Log("Previous Player Health: " + pHealth + ", New Player Health: " + tempHealth);
        Debug.Log("Previous Player Damage: " + pDamage + ", New Player Damage: " + tempDamage);

        updateManagers("all");
    }

    private void enemyMultiplayerCal(string mulWeight = "") {
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();
        int eSpawn = dm.getEnemySpawnAmount();
        int tempHealth = 0;
        int tempDamage = 0;
        int tempSpawnAmount = 0;

        if(mulWeight == "") {
            return;

        } else if(mulWeight == "small") {

            tempHealth = (int)(eHealth * smallWeightMultiplier);
            dm.setEnemyHealth(tempHealth);

            tempDamage = (int)(eDamage * smallWeightMultiplier);

            if(tempDamage == eDamage) {
                tempDamage++;
            }

            tempSpawnAmount = eSpawn + 1;
            dm.setEnemySpawnAmount(tempSpawnAmount);

            dm.setEnemyDamage(tempDamage);

        } else if(mulWeight == "medium") {

            tempHealth = (int)(eHealth * mediumWeightMultiplier);
            dm.setEnemyHealth(tempHealth);

            tempDamage = (int)(eDamage * mediumWeightMultiplier);

            if(tempDamage == eDamage) {
                tempDamage++;
            }

            tempSpawnAmount = eSpawn + 2;
            dm.setEnemySpawnAmount(tempSpawnAmount);

            dm.setEnemyDamage(tempDamage);

        } else if(mulWeight == "high") {

            tempHealth = (int)(eHealth * highWeightMultiplier);
            dm.setEnemyHealth(tempHealth);

            tempDamage = (int)(eDamage * highWeightMultiplier);

            if(tempDamage == eDamage) {
                tempDamage++;
            }

            tempSpawnAmount = eSpawn + 3;
            dm.setEnemySpawnAmount(tempSpawnAmount);

            dm.setEnemyDamage(tempDamage);

        }

        if(tempHealth < eHealth && tempDamage < eDamage) {
            tempHealth = eHealth;
            tempDamage = eDamage;
        } else if(tempHealth < eHealth) {
            tempHealth = eHealth;
        } else if(tempDamage < eDamage) {
            tempDamage = eDamage;
        }
        
        if(tempSpawnAmount < eSpawn) {
            tempSpawnAmount = eSpawn;
        }

        Debug.Log("Previous Enemy Health: " + eHealth + ", New Enemy Health: " + tempHealth);
        Debug.Log("Previous Enemy Damage: " + eDamage + ", New Enemy Damage: " + tempDamage);
        Debug.Log("Previous Enemy Spawn Amount: " + eSpawn + ", New Enemy Spawn Amount: " + (tempSpawnAmount));

        updateManagers("all");
    }

    public void UpdateEnemyDamageHealthAmount() {
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();

        int tempEHealth = (int)(eHealth * smallWeightMultiplier);

        dm.setEnemyHealth(tempEHealth);

        int tempEDamage = (int)(eDamage * smallWeightMultiplier);

        // check to see if multiplier increased eDamage - if not add 1
        if (tempEDamage == eDamage) {
            tempEDamage++;
        }

        // update enemy movespeed - check it's not more than player move speed
        if (dm.getEnemyMoveSpeed() < pm.getPlayerMoveSpeed()) {
            dm.setEnemyMoveSpeed(dm.getEnemyMoveSpeed() + 0.1f);

            // if enemy move speed is more than the player, set eMoveSpeed to pMoveSpeed
            if(em.getEnemyMoveSpeed() > pm.getPlayerMoveSpeed()) {
                dm.setEnemyMoveSpeed(pm.getPlayerMoveSpeed());
            }
        }

        dm.setEnemyDamage(tempEDamage);

        UpdateEnemySpawnAmount(1);

        updateManagers("enemy");
    }
    
    public void TimerUpdateEnemySpeed(float updateSpeed) {
        // update enemy movespeed - check it's not more than player move speed
        if(dm.getEnemyMoveSpeed() < pm.getPlayerMoveSpeed()) {
            dm.setEnemyMoveSpeed(dm.getEnemyMoveSpeed() + updateSpeed);

            // if enemy move speed is more than the player, set eMoveSpeed to pMoveSpeed
            if(dm.getEnemyMoveSpeed() > pm.getPlayerMoveSpeed()) {
                dm.setEnemyMoveSpeed(pm.getPlayerMoveSpeed());
            } else if (dm.getEnemyMoveSpeed() < 2.0f) {
                dm.setEnemyMoveSpeed(2.0f);
            }
        }

        updateManagers("");
    }

    public void UpdateEnemyDamage(int updateDamage) {
        int tempCheck = dm.getEnemyDamage();
        tempCheck += updateDamage;
        if (tempCheck >= 10) {
            dm.hardSetEnemyDamage(updateDamage);
        } else {
            Debug.Log("Damage would be too low");
        }

        updateManagers("enemy");
    }

    public void UpdateEnemySpawnAmount(int amount) {
        int tempSpawnAmount = em.getEnemySpawnRate();
        int tempMaxSpawnAmount = em.getEnemyMaxMaxAmount();
        int tempAmount = tempSpawnAmount + amount;

        if(tempAmount <= tempMaxSpawnAmount) {
            em.setEnemySpawnRate(amount);

            updateManagers("");

        } else if (tempSpawnAmount < tempMaxSpawnAmount && tempAmount > tempMaxSpawnAmount) {
            em.setEnemySpawnRate(em.getEnemyMaxMaxAmount());
            
            updateManagers("");

        } else { return; }
    }

    private void updateManagers(string updatePlayer = "") {
        pm.updatePlayerDetails(updatePlayer);
        em.updateDetails = true;
    }
}
