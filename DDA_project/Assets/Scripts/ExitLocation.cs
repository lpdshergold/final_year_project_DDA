using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ExitLocation : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            EnemyManager enemy = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

            enemy.resetEnemyAmount = true;
            SceneManager.LoadScene("ScenarioOne");
        }
    }
}
