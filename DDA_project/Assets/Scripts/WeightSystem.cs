using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : MonoBehaviour
{
    private PlayerManager pm;
    private Rulebook rulebook;

    private bool doOnce = false;

    private int playerHealthWeight = 0;
    private int playerDamageWeight = 0;
    private int enemyHealthWeight = 0;
    private int enemyDamageWeight = 0;

    private int playerHealth, playerMaxHealth, playerDeaths, playerDamageHits, enemyDamageHits, enemySpawnAmount;

    void Start()
    {
        rulebook = GameObject.Find("DifficultyManager").GetComponent<Rulebook>();
    }

    void Update()
    {
        if (GameObject.Find("PlayerManager") && !doOnce) {
            pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            doOnce = true;
        }
    }

    public void passWeightDetails(int pHealth, int pMaxHealth, int pDeaths, int pDamageHit, int eDamageHit, int eSpawnAmount) {
        playerHealth = pHealth; 
        playerMaxHealth = pMaxHealth;
        playerDeaths = pDeaths;
        playerDamageHits = pDamageHit;
        enemyDamageHits = eDamageHit;
        enemySpawnAmount = eSpawnAmount;

        weightSteps();
    }

    private void weightSteps() { // Change this to playerWeightSteps and add an enemy function once basics are done
        playerHealthWeighting();
        playerDeathWeighting();
        playerDamageHitsWeighting();
    }

    private void playerHealthWeighting() { // player health weighting 
        if(playerHealth == playerMaxHealth) {
            Debug.Log("no change in player health weighting");
            return;
        } else if(playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 75)) {
            Debug.Log("player health weighting up by 1");
            playerHealthWeight += 1;
        } else if (playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 35) && playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 75)) {
            Debug.Log("player health weighting up by 2");
            playerHealthWeight += 2;
        } else if (playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 35)) {
            Debug.Log("player health weighting up by 3");
            playerHealthWeight += 3;
        }
    }

    private void playerDeathWeighting() { // player health weighting
        if(playerDeaths == 0) {
            Debug.Log("no change in player deaths weighting");
        } else if ( playerDeaths == 1 ) {
            Debug.Log("player death weighting up by 1");
            playerHealthWeight += 1;
        } else if (playerDeaths == 2) {
            Debug.Log("player death weighting up by 2");
            playerHealthWeight += 2;
        } else if (playerDeaths > 2) {
            Debug.Log("player death weighting up by 3");
            playerHealthWeight += 3;
        }
    }

    private void playerDamageHitsWeighting() { // player damage weighting
        if(enemyDamageHits == 0) {
            return;
        } else if(enemyDamageHits >= playerDamageHits) {
            Debug.Log("player damage weighting up by 3");
            playerDamageWeight += 3;
        } else if(enemyDamageHits >= playerDamageHits / 2) {
            Debug.Log("player damage weighting up by 2");
            playerDamageWeight += 2;
        } else {
            Debug.Log("player damage weighting up by 1");
            playerDamageWeight += 1;
        }
    }
}
