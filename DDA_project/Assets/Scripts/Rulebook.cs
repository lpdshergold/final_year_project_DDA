using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rulebook : MonoBehaviour
{
    // LOOK AT INTERIM REPORT FOR SCENARIOS FOR RULEBOOK AS WELL AS MIDDLEGROUND BETWEEN WEIGHTSYSTEM AND MULTIPLIER

    private PlayerManager pm;
    private Multiplier multiplier;
    private WeightSystem weightSystem;

    private bool isDDAEnabled = false;
    private bool levelup = false;
    private bool doOnce = false;
    private bool passOncePerLevel = false;

    private string weightLvl = "";

    private int enemiesKilled = 0;
    private int playerHealth = 0;
    private int playerMaxHealth = 0;
    private int playerDeaths = 0;

    void Start()
    {
        multiplier = gameObject.GetComponent<Multiplier>();
        weightSystem = gameObject.GetComponent<WeightSystem>();
    }

    void Update()
    {
        if(GameObject.Find("PlayerManager") && !doOnce) {
            pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            doOnce = true;
        }

        if(levelup && passOncePerLevel) {
            passOncePerLevel = false;

            playerHealth = pm.getPlayerHealth();
            playerMaxHealth = pm.getPlayerMaxHealth();

            weightSystem.passWeightDetails(playerHealth, playerMaxHealth, playerDeaths, enemiesKilled);
        }

        if(levelup && isDDAEnabled && weightLvl != "") {
            levelup = false;
            multiplier.ddaLevelup(weightLvl);
            weightLvl = "";
        } else if (levelup && !isDDAEnabled) {
            levelup = false;
            multiplier.basicLevelUp();
        }
    }

    public void setDDA(bool enable) { isDDAEnabled = enable; }

    public void setPassOncePerLvl(bool isPassed) { passOncePerLevel = isPassed; }

    public void setLevelup(bool plevel) { levelup = plevel; }

    public void setWeightLvl( string weight) { weightLvl = weight; }

    public void updateEnemiesKilled() { enemiesKilled++; }

    public void updatePlayerDeaths() { playerDeaths++; }
}
