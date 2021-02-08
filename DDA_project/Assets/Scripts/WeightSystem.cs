using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : MonoBehaviour
{
    private PlayerManager pm;
    private Rulebook rulebook;

    private bool doOnce = false;

    private int playerLvl = 1;

    private int playerHealthWeight = 0;
    private int playerDamageWeight = 0;
    private int enemyHealthWeight = 0;
    private int enemyDamageWeight = 0;

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
        playerDamageHits = pDamageHit;
        enemyDamageHits = eDamageHit;
        enemySpawnAmount = eSpawnAmount;

        allPlayerHealth.Add(playerHealth);
        allPlayerDeaths.Add(playerDeaths);
        allPlayerDamageHits.Add(playerDamageHits);
        allEnemyDamageHits.Add(enemyDamageHits);

        weightSteps(); // Turn this function into a coroutine to run the functions before calculating the weight
    }

    private void weightSteps() { // Change this to playerWeightSteps and add an enemy function once basics are done
        playerHealthWeighting();
        playerDeathWeighting();
        playerDamageHitsWeighting();
    }

    private void playerHealthWeighting() { // player health weighting 
        if(playerLvl <= 4) {
            if(playerHealth == playerMaxHealth || playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 71)) {
                return;
            } else if(playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 50) && playerHealth < Convert.ToDouble((playerMaxHealth / 100) * 70)) {
                playerHealthWeight += 1;
            } else if(playerHealth >= Convert.ToDouble((playerMaxHealth / 100) * 30) && playerHealth < Convert.ToDouble((playerMaxHealth / 100) * 50)) {
                playerHealthWeight += 2;
            } else if(playerHealth <= Convert.ToDouble((playerMaxHealth / 100) * 30)) {
                playerHealthWeight += 3;
            }
        } else {

            int averagePlayerHealth = 0, medianPlayerHealth, modePlayerHealth = 0;

            allPlayerHealth.Sort();

            foreach (int aHealth in allPlayerHealth) {
                averagePlayerHealth += aHealth;
            }

            if (allPlayerHealth.Count % 2 == 0) { // Even
                int numOne = allPlayerHealth.Count / 2 - 1;
                int numTwo = allPlayerHealth.Count / 2;
                int choice = (numOne + numTwo) / 2;

                medianPlayerHealth = allPlayerHealth[choice];
            } else { // Odd
                int choice = allPlayerHealth.Count / 2;
                medianPlayerHealth = allPlayerHealth[choice];
            }

            Dictionary<int, int> modeData = new Dictionary<int, int>();
            foreach(int key in allPlayerHealth) {
                if(modeData.ContainsKey(key)) {
                    modeData[key]++;
                } else {
                    modeData.Add(key, 1);
                }
            }

            int mostCommonNum = 0;
            foreach(KeyValuePair<int, int> data in modeData) {
                if(data.Value > mostCommonNum) {
                    modePlayerHealth = data.Key;
                    mostCommonNum = data.Value;
                }
                Debug.Log("Mode Data Key: " + data.Key + " Mode Data Value: " + data.Value);
            }

            averagePlayerHealth = averagePlayerHealth / allPlayerHealth.Count;

            Debug.Log("Average Player Health: " + averagePlayerHealth);
            Debug.Log("Median Player Health: " + medianPlayerHealth);
            Debug.Log("Mode Player Health: " + modePlayerHealth);
        }
    }

    private void playerDeathWeighting() { // player health weighting
        if(playerLvl <= 4) {
            if(playerDeaths == 0) {
                return;
            } else if(playerDeaths == 1) {
                playerHealthWeight += 1;
            } else if(playerDeaths == 2) {
                playerHealthWeight += 2;
            } else if(playerDeaths > 2) {
                playerHealthWeight += 3;
            }
        } else {
            Debug.Log("Do stuff with the average of deaths stored beforehand");
        }
    }

    private void playerDamageHitsWeighting() { // player damage weighting
        if(enemyDamageHits == 0) {
            return;
        } else if(enemyDamageHits >= playerDamageHits) {
            playerDamageWeight += 3;
        } else if(enemyDamageHits >= playerDamageHits / 2) {
            playerDamageWeight += 2;
        } else {
            playerDamageWeight += 1;
        }
    }
}
