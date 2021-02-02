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
            smallPlayerMultiplierCal();
            smallEnemyMultiplierCal();
        } else if (pWeight == "medium") {
            mediumPlayerMultiplierCal();
            mediumEnemyMultiplierCal();
        } else if (pWeight == "high") {
            highPlayerMultiplierCal();
            highEnemyMultiplierCal();
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

        pm.updateDetails = true;
        em.updateDetails = true;
    }

    private void smallPlayerMultiplierCal() {
        Debug.Log("Small player multiplier");
        int pHealth = dm.getPlayerHealth();
        int pDamage = dm.getPlayerDamage();

        int tempHealth = (int)(pHealth * smallWeightMultiplier);
        dm.setPlayerHealth(tempHealth);

        int tempDamage = (int)(pDamage * smallWeightMultiplier);
        dm.setPlayerDamage(tempDamage);

        pm.updateDetails = true;
    }

    private void mediumPlayerMultiplierCal() {
        Debug.Log("Medium player multiplier");
        int pHealth = dm.getPlayerHealth();
        int pDamage = dm.getPlayerDamage();

        int tempHealth = (int)(pHealth * mediumWeightMultiplier);
        dm.setPlayerHealth(tempHealth);

        int tempDamage = (int)(pDamage * mediumWeightMultiplier);
        dm.setPlayerDamage(tempDamage);

        pm.updateDetails = true;
    }

    private void highPlayerMultiplierCal() {
        Debug.Log("High player multiplier");
        int pHealth = dm.getPlayerHealth();
        int pDamage = dm.getPlayerDamage();

        int tempHealth = (int)(pHealth * highWeightMultiplier);
        dm.setPlayerHealth(tempHealth);

        int tempDamage = (int)(pDamage * highWeightMultiplier);
        dm.setPlayerDamage(tempDamage);

        pm.updateDetails = true;
    }

    private void smallEnemyMultiplierCal() {
        Debug.Log("Small enemy multiplier");
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();

        int tempEHealth = (int)(eHealth * smallWeightMultiplier);
        dm.setEnemyHealth(tempEHealth);

        int tempEDamage = (int)(eDamage * smallWeightMultiplier);
        dm.setEnemyDamage(tempEDamage);

        em.updateDetails = true;
    }

    private void mediumEnemyMultiplierCal() {
        Debug.Log("Medium enemy multiplier");
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();

        int tempEHealth = (int)(eHealth * mediumWeightMultiplier);
        dm.setEnemyHealth(tempEHealth);

        int tempEDamage = (int)(eDamage * mediumWeightMultiplier);
        dm.setEnemyDamage(tempEDamage);

        em.updateDetails = true;
    }

    private void highEnemyMultiplierCal() {
        Debug.Log("High enemy multiplier");
        int eHealth = dm.getEnemyHealth();
        int eDamage = dm.getEnemyDamage();

        int tempEHealth = (int)(eHealth * highWeightMultiplier);
        dm.setEnemyHealth(tempEHealth);

        int tempEDamage = (int)(eDamage * highWeightMultiplier);
        dm.setEnemyDamage(tempEDamage);

        em.updateDetails = true;
    }
}
