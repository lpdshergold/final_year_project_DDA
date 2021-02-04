using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;

    private PlayerLevel pl;

    private Rigidbody2D rb;
    private Vector2 playerDirection;

    private EnemyManager em;
    private int playerAtkDamage;

    private float moveSpeed;
    private int eHealth;
    private int minExpToGive = 10;
    private int maxExpToGive = 20;

    private void Awake() {
        // get the GameManager script component
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        pl = GameObject.Find("PlayerManager").GetComponent<PlayerLevel>();
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
        MoveEnemy();
    }

    private void MoveEnemy() {
        if (player) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        } else {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bullet") {
            eHealth -= playerAtkDamage;
            em.updatePlayerHits();
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
            Destroy(gameObject);
        }
    }
}