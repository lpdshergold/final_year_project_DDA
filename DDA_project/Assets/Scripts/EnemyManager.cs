using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance = null;

    private GameManager gm;
    
    public int playerAtkDamage;

    private void Awake() {
        if(enemyManagerInstance == null) {
            enemyManagerInstance = this;
        } else if (enemyManagerInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // THIS WILL NEED TO BE ADDED TO UPDATE FUNCTION FOR WHEN THE DAMAGE IS INCREASED
        // get player attack damager
        playerAtkDamage = gm.getPlayerDamage();
    }

    private void Start() {

    }
}