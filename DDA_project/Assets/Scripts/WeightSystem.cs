using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : MonoBehaviour
{
    private PlayerManager pm;
    private Rulebook rulebook;

    private bool doOnce = false;

    private int weight = 0;
    private int playerWeight = 0;
    private int enemyWeight = 0;

    private int playerHealth, playerMaxHealth, playerDeaths, enemiesKilled;

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

    public void passWeightDetails(int pHealth, int pMaxHealth, int pDeaths, int eKilled) {
        playerHealth = pHealth; 
        playerMaxHealth = pMaxHealth;
        enemiesKilled = eKilled;
        playerDeaths = pDeaths;

        weightSteps();
    }

    private void weightSteps() { // Change this to playerWeightSteps and add an enemy function once basics are done
        playerHealthWeighting();
        playerDealthWeighting();
    }

    private void playerHealthWeighting() {
        if(playerHealth == playerMaxHealth) {
            Debug.Log("no change in player health weighting");
            return;
        } else if(playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 75)) {
            Debug.Log("player health weighting up by 1");
            weight += 1;
        } else if (playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 35) && playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 75)) {
            Debug.Log("player health weighting up by 2");
            weight += 2;
        } else if (playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 35)) {
            Debug.Log("player health weighting up by 3");
            weight += 3;
        }
    }

    private void playerDealthWeighting() {
        if(playerDeaths == 0) {
            Debug.Log("no change in player deaths weighting");
        } else if ( playerDeaths == 1 ) {
            Debug.Log("player death weighting up by 1");
            weight += 1;
        } else if (playerDeaths == 2) {
            Debug.Log("player death weighting up by 2");
            weight += 2;
        } else if (playerDeaths > 2) {
            Debug.Log("player death weighting up by 3");
            weight += 3;
        }
    }

    private void enemyWeightSteps() {

    }
}
