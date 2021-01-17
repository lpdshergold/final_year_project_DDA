using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    private PlayerManager pm;

    private int health;
    private Text healthText;

    private void Start() {
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        healthText = GetComponent<Text>();
    }

    private void Update() {
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay() {
        health = pm.getPlayerHealth();
        healthText.text = "Health: " + health;
    }
}
