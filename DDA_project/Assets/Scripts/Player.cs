﻿using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Sprite choosenGun;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;

    [SerializeField] private float playerSpeed = 0f;
    [SerializeField] private float delayShoot = .25f;
    [SerializeField] private float destroyBulletTime = 5f;
    [SerializeField] private float bulletSpeed = 8f;

    [SerializeField] private float delayEnemyAtk = .5f;

    private Transform weaponAim;
    private Rigidbody2D rb;
    private SpriteRenderer playerFlip, gun;

    private PlayerManager pm;

    private bool fX = false;
    private bool isFiring = false;
    private bool isPlayerHit = false;

    private int enemyDamage;

    public int pHealth;

    private void Awake() {
        // get the GameManager script component
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerFlip = GetComponent<SpriteRenderer>();
        gun = weapon.GetComponent<SpriteRenderer>();

        weaponAim = weapon.transform;
        
        gun.sprite = choosenGun;
        
        // Get info from GameManager
        pHealth = pm.getPlayerHealth();
        playerSpeed = pm.getPlayerMoveSpeed();
        enemyDamage = pm.enemyDamage;
    }

    private void Update() {
        RotateGun();
        KillPlayer();
    }

    private void FixedUpdate() {
        PlayerMovement();
        ShootGun();
    }

    // Moves the player character
    private void PlayerMovement() {
        if(Input.GetKey(KeyCode.D)) {
            rb.velocity = new Vector2(playerSpeed, 0);
            fX = false;
        } else if (Input.GetKey(KeyCode.A)) {
            rb.velocity = new Vector2(-playerSpeed, 0);
            fX = true;
        } else if(Input.GetKey(KeyCode.W)) {
            rb.velocity = new Vector2(0, playerSpeed);
        } else if(Input.GetKey(KeyCode.S)) {
            rb.velocity = new Vector2(0, -playerSpeed);
        } else {
            rb.velocity = new Vector2(0, 0);
        }

        playerFlip.flipX = fX;
    }

    private void RotateGun() {
        // get the mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get the aim direction for the gun
        Vector3 aimDir = (mousePos - weaponAim.transform.position);

        // rotation of the mouse
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        // Used to flip the weapon scale
        Vector3 tempScaleVector = weaponAim.transform.localScale;

        if(fX) {
            angle = Mathf.Atan2(-aimDir.y, -aimDir.x) * Mathf.Rad2Deg;
            
            // flip the weapon
            tempScaleVector.x = -1;
            weaponAim.transform.localScale = tempScaleVector;
        } else {
            angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

            // flip the weapon
            tempScaleVector.x = 1;
            weaponAim.transform.localScale = tempScaleVector;
        }

        weaponAim.eulerAngles = new Vector3(0, 0, angle);
    }

    private void ShootGun() {
        // check if left mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0)) {
            // if already firing, exit out
            if(isFiring) { return; }

            isFiring = true;
            // create the bullet gameObject
            GameObject shotBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

            // Ignore the player box colliders
            Physics2D.IgnoreCollision(shotBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            
            // check flip direction 
            if (fX) {
                // if flipped, reverse velocity
                shotBullet.GetComponent<Rigidbody2D>().velocity = bulletSpawn.right * -bulletSpeed; 
            } else {
                // if not flipped, don't reverse velocity
                shotBullet.GetComponent<Rigidbody2D>().velocity = bulletSpawn.right * bulletSpeed;
            }

            // destroy bullet after timer runs out
            Destroy(shotBullet, destroyBulletTime);
            // invoke ResetShoot() after delay
            Invoke("ResetShoot", delayShoot);
        }
    }

    private void ResetShoot() {
        isFiring = false;
    }

    public void updateDetails() {
        pHealth = pm.getPlayerHealth();
        enemyDamage = pm.enemyDamage;
    }

    // collision function
    private void OnCollisionStay2D(Collision2D collision) {
        if(isPlayerHit) { return; }

        isPlayerHit = true;

        // check for collision with enemy
        if(collision.gameObject.tag == "Enemy") {
            pHealth -= enemyDamage;
            pm.updateEnemyHits();
        }
        // update the player health in GameManger
        pm.setPlayerHealth(pHealth);

        Invoke("ResetPlayerHit", delayEnemyAtk);
    }

    private void ResetPlayerHit() {
        isPlayerHit = false;
    }

    // function to check if the player has 0 health
    private void KillPlayer() {
        if(pHealth <= 0) {
            pm.setIsPlayerDead(true);
            Destroy(gameObject);
        }
    }
}