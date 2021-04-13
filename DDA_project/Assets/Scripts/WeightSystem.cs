using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Jobs;

public class WeightSystem : MonoBehaviour {
    private EnemyManager em;
    private Rulebook rulebook;

    private IEnumerator weightingCoroutine;

    private bool doOnce = false;

    public int playerLvl = 1;

    public int playerWeighting = 0;
    public int enemyWeighting = 0;

    public string pWeighting = "";
    public string eWeighting = "";

    public int playerHealth, playerMaxHealth, playerDeaths, playerDamageHits, enemyDamageHits, enemySpawnAmount;

    public List<int> allPlayerHealth = new List<int>();
    public List<int> allPlayerDeaths = new List<int>();
    public List<int> allPlayerDamageHits = new List<int>();
    public List<int> allEnemyDamageHits = new List<int>();
    public List<KeyValuePair<int, int>> allIndividualEnemyHitsOnPlayer = new List<KeyValuePair<int, int>>();
    public List<KeyValuePair<int, bool>> checkEnemyMovementStatus = new List<KeyValuePair<int, bool>>();

    public void Start() {
        rulebook = GameObject.Find("DifficultyManager").GetComponent<Rulebook>();
    }

    void Update() {
        if(GameObject.Find("EnemyManager") && !doOnce) {
            em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            doOnce = true;
        }
    }

    public void passWeightDetails(int pHealth, int pMaxHealth, int pDeaths, int pDamageHit, int eDamageHit, int eSpawnAmount) {
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

        enemiesNotSeenPlayer();

        Debug.Log("playerLvl: " + playerLvl);

        weightingCoroutine = startWeightChecking(2.0f);
        StartCoroutine(weightingCoroutine);
    }

    private IEnumerator startWeightChecking(float waitTime) {
        // player weighting
        playerHealthWeighting();
        playerDeathWeighting();
        playerDamageHitsWeighting();

        // enemy weighting
        setAliveEnemyHits();
        enemyHitPlayerWeighting();
        enemySeenPlayerWeighting();

        yield return new WaitForSeconds(waitTime);
        playerLvl++;
        weighting();
    }

    public void weighting() {

        if(playerWeighting == 0) {
            pWeighting = "none";
        } else if(playerWeighting > 0 && playerWeighting <= 3) {
            pWeighting = "small";
        } else if(playerWeighting > 3 && playerWeighting <= 6) {
            pWeighting = "medium";
        } else if(playerWeighting > 6 && (playerWeighting <= 9 || playerWeighting > 9)) {
            pWeighting = "high";
        }

        if(enemyWeighting == 0) {
            eWeighting = "none";
        } else if(enemyWeighting > 0 && enemyWeighting <= 3) {
            eWeighting = "small";
        } else if(enemyWeighting > 3 && enemyWeighting <= 6) {
            eWeighting = "medium";
        } else if(enemyWeighting > 6 && (enemyWeighting <= 9 || enemyWeighting > 9)) {
            eWeighting = "high";
        }

        playerWeighting = 0;
        enemyWeighting = 0;

        rulebook.setWeightLvl(pWeighting, eWeighting);
    }

    public void playerHealthWeighting() { // player weighting 
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
                return;
            } else if(averagePlayerHealth > (Convert.ToDouble(playerMaxHealth / 100.0) * 50.0) && Convert.ToDouble(averagePlayerHealth) <= (Convert.ToDouble(playerMaxHealth / 100.0) * 70.0)) {
                playerWeighting += 1;
            } else if(averagePlayerHealth > (Convert.ToDouble(playerMaxHealth / 100.0) * 30.0) && Convert.ToDouble(averagePlayerHealth) <= (Convert.ToDouble(playerMaxHealth / 100.0) * 50.0)) {
                playerWeighting += 2;
            } else if(averagePlayerHealth <= Convert.ToDouble(playerMaxHealth / 100.0) * 30.0) {
                playerWeighting += 3;
            }
        }
    }

    public void playerDeathWeighting() { // player weighting
        if(playerLvl <= 3) {
            if(playerDeaths == 0) {
                return;
            } else if(playerDeaths == 1) {
                playerWeighting += 1;
            } else if(playerDeaths == 2) {
                playerWeighting += 2;
            } else if(playerDeaths == 3) {
                playerWeighting += 3;
            } else if(playerDeaths > 3) {
                playerWeighting += 4;
            }
        } else {

            int averagePlayerDeaths = averageOfLastThreeLevels(allPlayerDeaths);

            if(averagePlayerDeaths == 0) {
                enemyWeighting += 3;
                Debug.Log("playerDeathWeighting: High");
            } else if(averagePlayerDeaths == 1) {
                playerWeighting += 1;
                enemyWeighting += 2;
                Debug.Log("playerDeathWeighting: Medium");
            } else if(averagePlayerDeaths == 2) {
                playerWeighting += 2;
                enemyWeighting += 1;
                Debug.Log("playerDeathWeighting: Low");
            } else if(averagePlayerDeaths == 3) {
                playerWeighting += 3;
            } else if(averagePlayerDeaths > 3) {
                playerWeighting += 4;
            }
        }
    }

    public void playerDamageHitsWeighting() { // player weighting
        if(playerLvl <= 3) {

            double seventyFiveDamageCheck = Convert.ToDouble(playerDamageHits / 100.0) * 75.0;
            double FiftyDamageCheck = Convert.ToDouble(playerDamageHits / 100.0) * 50.0;
            double twentyFiveDamageCheck = Convert.ToDouble(playerDamageHits / 100.0) * 25.0;

            if(Convert.ToDouble(enemyDamageHits) > seventyFiveDamageCheck) {
                playerWeighting += 3;
            } else if(Convert.ToDouble(enemyDamageHits) <= seventyFiveDamageCheck && Convert.ToDouble(enemyDamageHits) > FiftyDamageCheck) {
                playerWeighting += 2;
            } else if(Convert.ToDouble(enemyDamageHits) <= FiftyDamageCheck && Convert.ToDouble(enemyDamageHits) > twentyFiveDamageCheck) {
                playerWeighting += 1;
            } else if(Convert.ToDouble(enemyDamageHits) <= twentyFiveDamageCheck) {
                return;
            }

        } else {

            double averagePlayerDamageHits = averageOfLastThreeLevels(allPlayerDamageHits);
            double averageEnemyDamageHits = averageOfLastThreeLevels(allEnemyDamageHits);

            double averageThirtyFiveFiveDamageCheck = Convert.ToDouble(averagePlayerDamageHits / 100.0) * 35.0;
            double averageTwentyFiveDamageCheck = Convert.ToDouble(averagePlayerDamageHits / 100.0) * 25.0;
            double averageFifteenDamageCheck = Convert.ToDouble(averagePlayerDamageHits / 100.0) * 15.0;

            if(averageEnemyDamageHits < averageFifteenDamageCheck) {
                return;
            } else if(averageEnemyDamageHits >= averageFifteenDamageCheck && averageEnemyDamageHits < averageTwentyFiveDamageCheck) {
                playerWeighting += 1;
            } else if(averageEnemyDamageHits >= averageTwentyFiveDamageCheck && averageEnemyDamageHits < averageThirtyFiveFiveDamageCheck) {
                playerWeighting += 2;
            } else if(averageEnemyDamageHits >= averageThirtyFiveFiveDamageCheck) {
                playerWeighting += 3;
            }
        }
    }

    // one to check if the enemies have seen the player - or encountered the player
    // one to check the hits on player from dead enemies - maybe also look at ones that have been triggered and hit the player as well for this
    // using the player death for the third weighting - may change

    public void enemySeenPlayerWeighting() {
        List<KeyValuePair<int, bool>> tempList = checkEnemyMovementStatus;

        int enemySeenPlayer = 0, enemyNotSeenPlayer = 0, tempPlayerLvl = 0, enemyLvlInList = 0;

        if(playerLvl < 4) {
            foreach(KeyValuePair<int, bool> enemy in tempList) {
                if(enemy.Value == false) {
                    enemyNotSeenPlayer++;
                } else {
                    enemySeenPlayer++;
                }
            }
        } else {
            foreach(KeyValuePair<int, bool> enemy in tempList) {
                if(enemyLvlInList != enemy.Key) {
                    enemyLvlInList = enemy.Key;
                    tempPlayerLvl++;
                }

                if(tempPlayerLvl > 3) {
                    break;
                }

                if(enemy.Value == false) {
                    enemyNotSeenPlayer++;
                } else {
                    enemySeenPlayer++;
                }
            }
        }

        int totalEnemies = enemySeenPlayer + enemyNotSeenPlayer;
        double getPercentage = (enemySeenPlayer * 100.0) / totalEnemies;

        if (getPercentage < 40.0) {
            enemyWeighting += 3;
        } else if (getPercentage >= 40.0 && getPercentage < 45.0) {
            enemyWeighting += 2;
        } else if (getPercentage >= 45.0 && getPercentage < 50.0) {
            enemyWeighting += 1;
        }
    }

    public void enemyHitPlayerWeighting() {
        List<KeyValuePair<int, int>> tempList = allIndividualEnemyHitsOnPlayer;

        int enemyCount = 0, enemyDamage = 0, tempPlayerLvl = 0, enemyLvlInList = 0;
        bool readyForAverage = false;
        double tempAverage = 0.0;

        if(playerLvl < 4) {
            foreach(KeyValuePair<int, int> enemy in tempList) {
                enemyCount++;
                enemyDamage += enemy.Value;
            }

            readyForAverage = true;

        } else {
            foreach(KeyValuePair<int, int> enemy in tempList) {
                if(enemyLvlInList != enemy.Key) {
                    enemyLvlInList = enemy.Key;
                    tempPlayerLvl++;
                }

                if (tempPlayerLvl > 3) {
                    break;
                }
                
                enemyCount++;
                enemyDamage += enemy.Value;
            }

            readyForAverage = true;
        
        }

        if(readyForAverage) {
            tempAverage = Convert.ToDouble(enemyDamage) / enemyCount;

            if(tempAverage <= 0.15) {
                enemyWeighting += 3;
            } else if (tempAverage > 0.15 && tempAverage <= 0.3) {
                enemyWeighting += 2;
            } else if (tempAverage > 0.3 && tempAverage <= 0.45) {
                enemyWeighting += 1;
            }
        }
    }

    // check enemies that have not seen the player and check enemies that are alive and have seen the player
    private void enemiesNotSeenPlayer() {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in Enemies) {
            Enemy e = enemy.GetComponent<Enemy>();

            if(!e.isFollowingPlayer) {
                checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(playerLvl, false));
            } else if(e.isFollowingPlayer) {
                checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(playerLvl, true));
            }
        }
    }

    // set dead enemies that have seen the player
    public void setDeadEnemiesSeenPlayer() {
        checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(playerLvl, true));
    }

    // get any alive enemies that have hit the player
    private void setAliveEnemyHits() {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in Enemies) {
            Enemy e = enemy.GetComponent<Enemy>();

            setIndividualEnemyHitList(e.hitPlayerAmount);
        }
    }

    // set individual enemy hits (public for dead enemies to update)
    public void setIndividualEnemyHitList(int hits) {
        allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(playerLvl, hits));
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

    public int averageOfLastThreeLevels(List<int> averageList) {
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