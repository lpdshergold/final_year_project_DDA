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
    private WeightSystem weightSystem;

    private Rigidbody2D rb;
    private Vector2 playerDirection;

    private EnemyManager em;
    private int playerAtkDamage;

    private float moveSpeed;
    private int eHealth;
    private int damage;
    private int minExpToGive = 10;
    private int maxExpToGive = 20;

    private void Awake() {
        // get the GameManager script component
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        pl = GameObject.Find("PlayerManager").GetComponent<PlayerLevel>();
        rulebook = GameObject.Find("DifficultyManager").GetComponent<Rulebook>();
        weightSystem = GameObject.Find("DifficultyManager").GetComponent<WeightSystem>();
    }

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();

        moveSpeed = em.getEnemyMoveSpeed();
        eHealth = em.getEnemyHealth();
        damage = em.getEnemyDamage();

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
            playerDirection = player.transform.position - transform.position;
            rb.MovePosition((Vector2)transform.position + (playerDirection * moveSpeed * Time.deltaTime));
        } else {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bullet") {
            eHealth -= playerAtkDamage;
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
            Destroy(gameObject);
        }
    }
}