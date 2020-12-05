using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance = null;

    [SerializeField] private GameObject player;

    private Transform playerSpawn;

    private int playerHealth = 100;
    private int playerLevel = 1;
    private int playerDamager = 15;

    private void Awake() {
        if(gameManagerInstance == null) {
            gameManagerInstance = this;
        } else if (gameManagerInstance != this){
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init() {
        spawnPlayer();
    }

    // create a playerSpawn function
    private void spawnPlayer() {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        _ = Instantiate(player, playerSpawn.position, playerSpawn.rotation);
    }

    public int getPlayerHealth() {
        return playerHealth;
    }

    public void setPlayerHealth(int health) {
        playerHealth = health;
    }
}
