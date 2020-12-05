using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private Vector2 playerDirection;

    public int moveSpeed = 1;
    public int eHealth = 100;
    public int damage = 15; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyEnemy();
    }

    private void FixedUpdate() {
        moveEnemy();
    }

    private void moveEnemy() {
        playerDirection = player.transform.position - transform.position;
        rb.MovePosition((Vector2)transform.position + (playerDirection * moveSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            eHealth -= 15;
        }
    }

    private void destroyEnemy() {
        if (eHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
