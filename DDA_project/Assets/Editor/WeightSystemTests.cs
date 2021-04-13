using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;

public class WeightSystemTests
{
    [Test]
    public void weighting_player_test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.Start();
        weight.pWeighting = "";
        weight.playerWeighting = 8;
        weight.weighting();

        Assert.That(weight.pWeighting, Is.EqualTo("high"));
    }

    [Test]
    public void weighting_enemy_test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.Start();
        weight.eWeighting = "";
        weight.enemyWeighting = 1;
        weight.weighting();

        Assert.That(weight.eWeighting, Is.EqualTo("small"));
    }

    [Test]
    public void playerHealthWeighting_No_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem pHealth = weightSystem;
        pHealth.playerLvl = 2;
        pHealth.playerHealth = 35;
        pHealth.playerMaxHealth = 100;
        pHealth.playerHealthWeighting();
        
        Assert.That(pHealth.playerWeighting, Is.EqualTo(2));
    }

    [Test]
    public void playerHealthWeighting_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem pHealth = weightSystem;
        pHealth.playerLvl = 4;
        pHealth.allPlayerHealth.Add(35);
        pHealth.allPlayerHealth.Add(35);
        pHealth.allPlayerHealth.Add(35);
        pHealth.playerMaxHealth = 100;
        pHealth.playerHealthWeighting();

        Assert.That(pHealth.playerWeighting, Is.EqualTo(2));
    }

    [Test]
    public void playerDeathWeighing_No_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 2;
        weight.playerDeaths = 1;

        weight.playerDeathWeighting();

        Assert.That(weight.playerWeighting, Is.EqualTo(1));
    }

    [Test]
    public void playerDeathWeighing_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 4;
        weight.allPlayerDeaths.Add(1);
        weight.allPlayerDeaths.Add(1);
        weight.allPlayerDeaths.Add(1);

        weight.playerDeathWeighting();

        Assert.That(weight.playerWeighting, Is.EqualTo(1));
        Assert.That(weight.enemyWeighting, Is.EqualTo(2));
    }

    [Test]
    public void playerDamageHitsWeighing_No_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 2;
        weight.playerDamageHits = 100;
        weight.enemyDamageHits = 50;
        weight.playerDamageHitsWeighting();

        Assert.That(weight.playerWeighting, Is.EqualTo(1));
    }

    [Test]
    public void playerDamageHitsWeighing_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 4;
        weight.allPlayerDamageHits.Add(80);
        weight.allPlayerDamageHits.Add(80);
        weight.allPlayerDamageHits.Add(80);
        weight.allEnemyDamageHits.Add(45);
        weight.allEnemyDamageHits.Add(45);
        weight.allEnemyDamageHits.Add(45);
        weight.playerDamageHits = 100;
        weight.enemyDamageHits = 50;
        weight.playerDamageHitsWeighting();

        Assert.That(weight.playerWeighting, Is.EqualTo(3));
    }

    [Test]
    public void enemySeenPlayerWeighting_No_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 2;
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(2, true));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(2, false));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(2, false));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(2, false));
        weight.enemySeenPlayerWeighting();

        Assert.That(weight.enemyWeighting, Is.EqualTo(3));
    }

    [Test]
    public void enemySeenPlayerWeighting_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 4;
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(0, true));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(0, false));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(1, false));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(1, false));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(2, true));
        weight.checkEnemyMovementStatus.Insert(0, new KeyValuePair<int, bool>(2, true));
        weight.enemySeenPlayerWeighting();

        Assert.That(weight.enemyWeighting, Is.EqualTo(0));
    }

    [Test]
    public void enemyHitPlayerWeighting_No_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 2;
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(0, 2));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(0, 1));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(0, 0));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(0, 0));
        weight.enemyHitPlayerWeighting();

        Assert.That(weight.enemyWeighting, Is.EqualTo(0));
    }

    [Test]
    public void enemyHitPlayerWeighting_Average_Test() {
        WeightSystem weightSystem = new WeightSystem();

        WeightSystem weight = weightSystem;
        weight.playerLvl = 4;
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(0, 0));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(0, 2));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(1, 0));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(1, 0));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(2, 0));
        weight.allIndividualEnemyHitsOnPlayer.Insert(0, new KeyValuePair<int, int>(2, 0));
        weight.enemyHitPlayerWeighting();

        Assert.That(weight.enemyWeighting, Is.EqualTo(1));
    }

    [Test]
    public void averageOfLastThreeLevels_Test() {
        var average = new WeightSystem();
        List<int> averageList = new List<int>();
        averageList.Add(1);
        averageList.Add(2);
        averageList.Add(3);
        var expected = 2;

        var averageMethod = average.averageOfLastThreeLevels(averageList);

        Assert.That(averageMethod, Is.EqualTo(expected));
    }
}
