using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private Vector2 playerDirection;

    private EnemyManager em;
    private int playerAtkDamage;

    private float moveSpeed;
    private int eHealth;
    private int damage;

    private void Awake() {
        // get the GameManager script component
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();

        moveSpeed = em.getEnemyMoveSpeed();
        eHealth = em.getEnemyHealth();
        damage = em.getEnemyDamage();

        playerAtkDamage = em.playerAtkDamage;
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(playerDirection);
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            eHealth -= playerAtkDamage;
        }
    }

    private void DestroyEnemy() {
        if (eHealth <= 0) {
            em.enemyAmount--;
            Destroy(gameObject);
        }
    }
}