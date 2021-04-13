using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;

public class RulebookTests
{
    [Test]
    public void playerKillsDeathCheck_Test() {
        Rulebook rulebook = new Rulebook();

        Rulebook rb = rulebook;
        rb.playerDeaths = 0;
        rb.playerDeathCheck = 0;
        rb.playerKillsDeathCounter = 20;
        Assert.Throws<NullReferenceException>(() => rb.PlayerKillsDeathsCheck());

        Assert.That(rb.playerKillsDeathCounter, Is.EqualTo(0));
    }

    [Test]
    public void playerTooManyDeaths_Test() {
        Rulebook rulebook = new Rulebook();

        Rulebook rb = rulebook;
        rb.playerDeathTimer = 10.0f;
        rb.rulebookPlayerDeath = 5;
        Assert.Throws<NullReferenceException>(() => rb.PlayerTooManyDeaths());

        Assert.That(rb.rulebookPlayerDeath, Is.EqualTo(0));
    }

    [Test]
    public void playerHitCheck() {
        Rulebook rulebook = new Rulebook();

        Rulebook rb = rulebook;
        rb.playerHitTimer = 16.0f;
        rb.enemyHitPlayer = 5;
        rb.enemyDamageHits = 5;
        Assert.Throws<NullReferenceException>(() => rb.PlayerHitCheck());

        Assert.That(rb.playerHitTimer, Is.EqualTo(0.0f));
    }

    [Test]
    public void tooManyPlayerHits() {
        Rulebook rulebook = new Rulebook();

        Rulebook rb = rulebook;
        rb.tooManyPlayerHitsTimer = 5.0f;
        rb.isPlayerHitTooMuch = 4;
        Assert.Throws<NullReferenceException>(() => rb.TooManyPlayerHits());

        Assert.That(rb.tooManyPlayerHitsTimer, Is.EqualTo(0.0f));
        Assert.That(rb.isPlayerHitTooMuch, Is.EqualTo(0));
    }

    [Test]
    public void notEnoughEnemyDamage() {
        Rulebook rulebook = new Rulebook();

        Rulebook rb = rulebook;
        rb.notEnoughEnemyDamageTimer = 30.0f;
        rb.rulebookIsPlayerNotHitEnough = 1;
        Assert.Throws<NullReferenceException>(() => rb.NotEnoughEnemyDamage());

        Assert.That(rb.notEnoughEnemyDamageTimer, Is.EqualTo(0.0f));
        Assert.That(rb.rulebookIsPlayerNotHitEnough, Is.EqualTo(0));
    }
}
