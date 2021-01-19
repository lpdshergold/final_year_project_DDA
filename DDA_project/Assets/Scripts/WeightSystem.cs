using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : MonoBehaviour
{
    private PlayerManager pm;
    private Rulebook rulebook;

    private bool doOnce = false;

    void Start()
    {
        rulebook = GameObject.Find("DifficultyManager").GetComponent<Rulebook>();
    }

    void Update()
    {
        if (GameObject.Find("PlayerManager") && !doOnce) {
            pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            doOnce = true;
        }
        
    }
}
