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
        Debug.Log("Getting to basic multiplier");
        basicMultiplierCal();
    }

    public void ddaLevelup(string pWeight) {
        if (pWeight == "small") {
            basicMultiplierCal();
        } else if (pWeight == "medium") {
            basicMultiplierCal();
        } else if (pWeight == "high") {
            basicMultiplierCal();
        } else {
            Debug.Log("Error with weight lvl: Multiplier script"); 
        }
    }

    private void basicMultiplierCal() {
        int pHealth = dm.getPlayerHealth();
        int pDamage = dm.getPlayerDamage();
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();

        int tempHealth = (int)(pHealth * basicMultiplier);
        dm.setPlayerHealth(tempHealth);

        int tempDamage = (int)(pDamage * basicMultiplier);
        dm.setPlayerDamage(tempDamage);

        int tempEHealth = (int)(eHealth * basicMultiplier);
        dm.setEnemyHealth(tempEHealth);

        int tempEDamage = (int)(eDamage * basicMultiplier);
        dm.setEnemyDamage(tempEDamage);

        updateManagers("all"); 
    }

    public void UpdateEnemyDamageHealthAmount() {
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();

        int tempEHealth = (int)(eHealth * mediumWeightMultiplier);

        dm.setEnemyHealth(tempEHealth);

        int tempEDamage = (int)(eDamage * mediumWeightMultiplier);

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

        em.setEnemySpawnRate(1);

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

        Debug.Log("enemy move speed: " + dm.getEnemyMoveSpeed());

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
        em.setEnemySpawnRate(amount);

        updateManagers("");
    }

    private void updateManagers(string updatePlayer = "") {
        pm.updatePlayerDetails(updatePlayer);
        em.updateDetails = true;
    }
}
