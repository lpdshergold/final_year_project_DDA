using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        } else if(collision.gameObject.CompareTag("Enemy")) {
            Destroy(gameObject);
        }
    }
}
