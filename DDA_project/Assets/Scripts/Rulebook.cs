using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rulebook : MonoBehaviour
{
    private PlayerManager pm;
    private Multiplier multiplier;
    private WeightSystem weightSystem;

    private bool isDDAEnabled = false;
    private bool levelup = false;
    private bool doOnce = false;

    void Start()
    {
        multiplier = gameObject.GetComponent<Multiplier>();
        weightSystem = gameObject.GetComponent<WeightSystem>();
    }

    void Update()
    {
        if(GameObject.Find("PlayerManager") && !doOnce) {
            pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            doOnce = true;
        }

        if(levelup) {
            levelup = false;
            if (isDDAEnabled) {
                multiplier.ddaLevelup();
            } else {
                multiplier.basicLevelUp();
            } 
        }
    }

    public void setDDA(bool enable) { isDDAEnabled = enable; }

    public void setLevelup(bool plevel) { levelup = plevel; }
}
