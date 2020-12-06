using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    private GameManager gm;

    private int health;
    private Text healthText;

    private void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthText = GetComponent<Text>();
    }

    private void Update() {
        health = gm.getPlayerHealth();
        healthText.text = "Health: " + health;
    }
}
