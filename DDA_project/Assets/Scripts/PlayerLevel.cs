using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    private GameObject playerManager;
    private PlayerManager pm;
    private Rulebook rulebook;

    private int playerlvl = 0;
    private int playerExp = 0;
    private int expToNextLevel = 100;

    void Start()
    {
        playerManager = GameObject.Find("PlayerManager");
        pm = playerManager.GetComponent<PlayerManager>();
        rulebook = playerManager.GetComponent<Rulebook>();

        playerlvl = pm.getPlayerLevel();
        playerExp = pm.getPlayerExperiencePoints();
    }

    void Update()
    {
        levelUp();
    }

    private void levelUp() {
        if (playerExp >= expToNextLevel) {
            playerExp = 0;
            playerlvl++;
            updateXp();

            pm.setPlayerLevel(playerlvl);
        }
    }

    private void updateXp() {
        float nextXpMax = (expToNextLevel / 100) * 5;
        expToNextLevel += Convert.ToUInt16(nextXpMax);
    }

    public int addXp(int xp) {
        return playerExp += xp;
    }
}
