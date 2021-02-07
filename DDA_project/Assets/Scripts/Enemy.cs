using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;

    private PlayerLevel pl;
    private Rulebook rulebook;

    private Rigidbody2D rb;
    private Vector2 playerDirection;

    private EnemyManager em;
    private int playerAtkDamage;

    private float moveSpeed;
    private int eHealth;
    private int minExpToGive = 10;
    private int maxExpToGive = 20;

    private bool isCautious = false;
    private bool isFollowingPlayer = false;
    private bool stopFollowingPlayer = true;

    private int playerTriggeredEnemy = 0;
    private float cautionTimer = 0.0f;
    private float stopFollowingPlayerTimer = 0.0f;

    private void Awake() {
        // get the GameManager script component
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        pl = GameObject.Find("PlayerManager").GetComponent<PlayerLevel>();
        rulebook = GameObject.Find("DifficultyManager").GetComponent<Rulebook>();
    }

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Init();
    }

    private void Init() {
        moveSpeed = em.getEnemyMoveSpeed();
        eHealth = em.getEnemyHealth();

        playerAtkDamage = em.playerAtkDamage;
    }

    // Update is called once per frame
    void Update() {
        DestroyEnemy();
    }

    private void FixedUpdate() {
        //if (isFollowingPlayer) {
        //    Debug.Log("Is Following");
        //    StopFollowingPlayerTimer();
        //    rulebook.updateRulebook = true;
            MoveEnemy();
       // } else if (isCautious) {
       //     Debug.Log("Is cautious");
       //     FollowPlayerTimer();
       // }
    }

    private void MoveEnemy() {
        if (player) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        } else {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            playerTriggeredEnemy++;
            if (playerTriggeredEnemy > 1) {
                isFollowingPlayer = true;
                stopFollowingPlayer = false;
            } else {
                isCautious = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            isCautious = false;
            stopFollowingPlayer = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Bullet") {
            isFollowingPlayer = true;
            eHealth -= playerAtkDamage;
            em.updatePlayerHits();
        }
    }

    private void FollowPlayerTimer() {
        if(!isFollowingPlayer) {
            cautionTimer += Time.deltaTime;

            if(cautionTimer >= 1.5f) {
                isFollowingPlayer = true;
            }
        }
    }

    private void StopFollowingPlayerTimer() {

        if(stopFollowingPlayer) {
            stopFollowingPlayerTimer += Time.deltaTime;

            if(stopFollowingPlayerTimer >= 5.0f) {
                isFollowingPlayer = false;
            }
        } else {
            stopFollowingPlayerTimer = 0.0f;
        }
    }

    // UPDATE TO CHANGE EXPERIENCE GIVEN AFTER LEVELING
    private void EnemyExperience() {
        int exp = Random.Range(minExpToGive, maxExpToGive);
        pl.addXp(exp);
    }

    private void DestroyEnemy() {
        if (eHealth <= 0) {
            em.enemyAmount--;
            EnemyExperience();
            em.updateRuleBookEnemiesKilled();
            rulebook.updateRulebook = false;
            Destroy(gameObject);
        }
    }
}