using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    private DifficultyManager dm;
    private PlayerManager pm;
    private EnemyManager em;

    private double basicMultiplier = 1.05;
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

    public void ddaLevelup() {
        Debug.Log("Getting to DDA multiplier");
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
}
