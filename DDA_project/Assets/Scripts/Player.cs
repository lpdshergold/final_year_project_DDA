using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerSpeed = 2;
    [SerializeField] private Sprite choosenGun;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;

    private Transform weaponAim;
    private Rigidbody2D rb;
    private SpriteRenderer playerFlip, gun;

    private bool fX = false;
    private bool isFiring = false;

    [SerializeField] float delayShoot = .25f;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFlip = GetComponent<SpriteRenderer>();
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


    // MOVE SHOOTGUN AND RESETSHOOT TO THEIR OWN SCRIPT THAT WILL BE PUT ON THE BULLET PREFAB
    private void ShootGun() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            if(isFiring) { return; }

            isFiring = true;
            GameObject b = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            if (fX) {
                b.GetComponent<Rigidbody2D>().velocity = bulletSpawn.right * -5f; 
            } else {
                b.GetComponent<Rigidbody2D>().velocity = bulletSpawn.right * 5f;
            }
            Destroy(b, 5f);
            Invoke("ResetShoot", delayShoot);
        }
    }

    private void ResetShoot() {
        isFiring = false;
    }
}
