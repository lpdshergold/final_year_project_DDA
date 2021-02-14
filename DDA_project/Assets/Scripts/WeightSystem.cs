﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : MonoBehaviour {
    private PlayerManager pm;
    private Rulebook rulebook;

    private IEnumerator weightingCoroutine;

    private bool doOnce = false;

    private int playerLvl = 1;
    
    private int playerWeighting = 0;

    private int playerHealth, playerMaxHealth, playerDeaths, playerDamageHits, enemyDamageHits, enemySpawnAmount;

    private List<int> allPlayerHealth = new List<int>();
    private List<int> allPlayerDeaths = new List<int>();
    private List<int> allPlayerDamageHits = new List<int>();
    private List<int> allEnemyDamageHits = new List<int>();

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
        playerLvl++;
        Debug.Log("playerLvl: " + playerLvl);
        playerHealth = pHealth; 
        playerMaxHealth = pMaxHealth;
        playerDeaths = pDeaths;
        playerDamageHits = pDamageHit; // player hits on enemy
        enemyDamageHits = eDamageHit; // enemy hits on player
        enemySpawnAmount = eSpawnAmount;

        allPlayerHealth.Add(playerHealth);
        allPlayerDeaths.Add(playerDeaths);
        allPlayerDamageHits.Add(playerDamageHits);
        allEnemyDamageHits.Add(enemyDamageHits);

        weightingCoroutine = startWeightChecking(2.0f);
        StartCoroutine(weightingCoroutine);
    }

    private IEnumerator startWeightChecking(float waitTime) {
        playerHealthWeighting();
        playerDeathWeighting();
        playerDamageHitsWeighting();

        yield return new WaitForSeconds(waitTime);
        
        Debug.Log("Coroutine ended!");
        weighting();
    }

    private void weighting() {
        if(playerWeighting == 0) {
            Debug.Log("No weighting increase sent to multiplier");
        } else if(playerWeighting > 0 && playerWeighting <= 3) {
            rulebook.setWeightLvl("small");
        } else if(playerWeighting > 3 && playerWeighting <= 6) {
            rulebook.setWeightLvl("medium");
        } else if(playerWeighting > 6 && playerWeighting <= 9) {
            rulebook.setWeightLvl("high");
        }

        playerWeighting = 0;
    }

    private void playerHealthWeighting() { // player weighting 
        if(playerLvl <= 3) {

            if(playerHealth == playerMaxHealth || playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 71)) {
                return;
            } else if(playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 50) && playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 70)) {
                playerWeighting += 1;
            } else if(playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 30) && playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 50)) {
                playerWeighting += 2;
            } else if(playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 30)) {
                playerWeighting += 3;
            }

        } else {

            int averagePlayerHealth = averageOfLastThreeLevels(allPlayerHealth);

            if(averagePlayerHealth > (Convert.ToDouble(playerMaxHealth) / 100.0) * 70.0) {
                Debug.Log("Average Player Health: " + averagePlayerHealth + " No PlayerHealthWeight added");
                return;
            } else if (averagePlayerHealth > (Convert.ToDouble(playerMaxHealth / 100.0) * 50.0) && Convert.ToDouble(averagePlayerHealth) <= (Convert.ToDouble(playerMaxHealth / 100.0) * 70.0)) {
                Debug.Log("Average Player Health: " + averagePlayerHealth + " PlayerHealthWeight + 1");
                playerWeighting += 1;
            } else if (averagePlayerHealth > (Convert.ToDouble(playerMaxHealth / 100.0) * 30.0) && Convert.ToDouble(averagePlayerHealth) <= (Convert.ToDouble(playerMaxHealth / 100.0) * 50.0)) {
                Debug.Log("Average Player Health: " + averagePlayerHealth + " PlayerHealthWeight + 2");
                playerWeighting += 2;
            } else if (averagePlayerHealth <= Convert.ToDouble(playerMaxHealth / 100.0) * 30.0) {
                Debug.Log("Average Player Health: " + averagePlayerHealth + " PlayerHealthWeight + 3");
                playerWeighting += 3;
            }
        }
    }

    private void playerDeathWeighting() { // player weighting
        if(playerLvl <= 3) {
            if(playerDeaths == 0) {
                return;
            } else if(playerDeaths == 1) {
                playerWeighting += 1;
            } else if(playerDeaths == 2) {
                playerWeighting += 2;
            } else if(playerDeaths > 2) {
                playerWeighting += 3;
            }
        } else {

            int averagePlayerDeaths = averageOfLastThreeLevels(allPlayerDeaths);

            if(averagePlayerDeaths == 0) {
                Debug.Log("Average Player Deaths: " + averagePlayerDeaths + " No PlayerDeathWeight added");
                return;
            } else if(averagePlayerDeaths == 1) {
                Debug.Log("Average Player Deaths: " + averagePlayerDeaths + " PlayerDeathWeight added + 1");
                playerWeighting += 1;
            } else if(averagePlayerDeaths == 2) {
                Debug.Log("Average Player Deaths: " + averagePlayerDeaths + " PlayerDeathWeight added + 2");
                playerWeighting += 2;
            } else if(averagePlayerDeaths > 2) {
                Debug.Log("Average Player Deaths: " + averagePlayerDeaths + " PlayerDeathWeight added + 3");
                playerWeighting += 3;
            }
        }
    }

    private void playerDamageHitsWeighting() { // player weighting
        if (playerLvl <= 3) {

            double seventyFiveDamageCheck = Convert.ToDouble(playerDamageHits / 100.0) * 75.0;
            double FiftyDamageCheck = Convert.ToDouble(playerDamageHits / 100.0) * 50.0;
            double twentyFiveDamageCheck = Convert.ToDouble(playerDamageHits / 100.0) * 25.0;

            /*
                Debug.Log("playerDamageHits: " + playerDamageHits);
                Debug.Log("enemyDamageHits:" + enemyDamageHits + " :" + Convert.ToDouble(enemyDamageHits));
                Debug.Log("eighty: " + seventyFiveDamageCheck);
                Debug.Log("sixty: " + FiftyDamageCheck);
                Debug.Log("forty: " + twentyFiveDamageCheck);
            */

            if (Convert.ToDouble(enemyDamageHits) > seventyFiveDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: + 3 weight given");
                playerWeighting += 3;
            } else if (Convert.ToDouble(enemyDamageHits) <= seventyFiveDamageCheck && Convert.ToDouble(enemyDamageHits) > FiftyDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: + 2 weight given");
                playerWeighting += 2;
            } else if (Convert.ToDouble(enemyDamageHits) <= FiftyDamageCheck && Convert.ToDouble(enemyDamageHits) > twentyFiveDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: + 1 weight given");
                playerWeighting += 1;
            } else if (Convert.ToDouble(enemyDamageHits) <= twentyFiveDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: no weight given");
                return;
            }

        } else {

            double averagePlayerDamageHits = averageOfLastThreeLevels(allPlayerDamageHits);
            double averageEnemyDamageHits = averageOfLastThreeLevels(allEnemyDamageHits);

            double averageThirtyFiveFiveDamageCheck = Convert.ToDouble(averagePlayerDamageHits / 100.0) * 35.0;
            double averageTwentyFiveDamageCheck = Convert.ToDouble(averagePlayerDamageHits / 100.0) * 25.0;
            double averageFifteenDamageCheck = Convert.ToDouble(averagePlayerDamageHits / 100.0) * 15.0;

            /*
                Debug.Log("playerDamageHits: " + averagePlayerDamageHits);
                Debug.Log("enemyDamageHits:" + averageEnemyDamageHits);
                Debug.Log("thirtyfive: " + averageThirtyFiveFiveDamageCheck);
                Debug.Log("twentyfive: " + averageTwentyFiveDamageCheck);
                Debug.Log("fifteen: " + averageFifteenDamageCheck);
            */

            if (averageEnemyDamageHits < averageFifteenDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: no weight given");
                return;
            } else if (averageEnemyDamageHits >= averageFifteenDamageCheck && averageEnemyDamageHits < averageTwentyFiveDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: + 1 weight given");
                playerWeighting += 1;
            } else if(averageEnemyDamageHits >= averageTwentyFiveDamageCheck && averageEnemyDamageHits < averageThirtyFiveFiveDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: + 2 weight given");
                playerWeighting += 2;
            } else if(averageEnemyDamageHits >= averageThirtyFiveFiveDamageCheck) {
                Debug.Log("playerDamageHitsWeighting: + 3 weight given");
                playerWeighting += 3;
            }
        }
    }

    private int average(List<int> averageList) {
        List<int> tempList = averageList;
        int tempAverage = 0;

        foreach(int aHealth in tempList) {
            tempAverage += aHealth;
        }

        tempAverage /= tempList.Count;

        return tempAverage;
    }

    private int averageOfLastThreeLevels(List<int> averageList) {
        List<int> tempList = averageList;
        int tempAverage = 0;

        // reverse list to get average of last 3 levels
        tempList.Reverse();
        int i = 0;
        while(i <= 2) {
            tempAverage += tempList[i];
            i++;
        }

        tempAverage /= 3;

        return tempAverage;
    }
}
