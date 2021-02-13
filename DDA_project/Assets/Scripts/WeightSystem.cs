using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : MonoBehaviour
{
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
        playerDamageHits = pDamageHit;
        enemyDamageHits = eDamageHit;
        enemySpawnAmount = eSpawnAmount;

        allPlayerHealth.Add(playerHealth);
        allPlayerDeaths.Add(playerDeaths);
        allPlayerDamageHits.Add(playerDamageHits);
        allEnemyDamageHits.Add(enemyDamageHits);

        weightingCoroutine = startWeightChecking(2.0f);
        StartCoroutine(weightingCoroutine);

        //weightSteps();
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
        Debug.Log("Send weighting back to rulebook from here");
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

    private void playerDamageHitsWeighting() { // player damage weighting
        if(enemyDamageHits == 0) {
            return;
        } else if(enemyDamageHits >= playerDamageHits) {
            playerWeighting += 3;
        } else if(enemyDamageHits >= playerDamageHits / 2) {
            playerWeighting += 2;
        } else {
            playerWeighting += 1;
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

    private int median(List<int> medianList) {
        List<int> tempList = medianList;
        tempList.Sort();

        int tempMedian;

        if(tempList.Count % 2 == 0) { // Even
            int numOne = tempList.Count / 2 - 1;
            int numTwo = tempList.Count / 2;
            int choice = (numOne + numTwo) / 2;

            tempMedian = tempList[choice];
        } else { // Odd
            int choice = tempList.Count / 2;
            tempMedian = allPlayerHealth[choice];
        }

        return tempMedian;
    }

    private int mode(List<int> modeList) {
        List<int> tempList = modeList;
        tempList.Sort();

        int tempModeData = 0;

        Dictionary<int, int> modeData = new Dictionary<int, int>();
        foreach(int key in tempList) {
            if(modeData.ContainsKey(key)) {
                modeData[key]++;
            } else {
                modeData.Add(key, 1);
            }
        }

        int mostCommonNum = 0;
        foreach(KeyValuePair<int, int> data in modeData) {
            if(data.Value > mostCommonNum) {
                tempModeData = data.Key;
                mostCommonNum = data.Value;
            }
            Debug.Log("Mode Data Key: " + data.Key + " Mode Data Value: " + data.Value);
        }

        return tempModeData;
    }
}
