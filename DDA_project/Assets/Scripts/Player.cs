using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerSpeed = 2;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Sprite choosenGun;

    private Transform weaponAim;
    private Rigidbody2D rb;
    private SpriteRenderer ren, gun;

    private bool fX = false;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        gun = weapon.GetComponent<SpriteRenderer>();
        weaponAim = weapon.transform;
        gun.sprite = choosenGun;
    }

    private void Update() {
        RotateGun();
        
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

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

        ren.flipX = fX;
    }

    private void RotateGun() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDir = (mousePos - weaponAim.transform.position);

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        Vector3 tempVector = weaponAim.transform.localScale;

        if(fX) {
            angle = Mathf.Atan2(-aimDir.y, -aimDir.x) * Mathf.Rad2Deg;
            tempVector.x = -1;
            weaponAim.transform.localScale = tempVector;
        } else {
            angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            tempVector.x = 1;
            weaponAim.transform.localScale = tempVector;
        }

        weaponAim.eulerAngles = new Vector3(0, 0, angle);
    }
}
