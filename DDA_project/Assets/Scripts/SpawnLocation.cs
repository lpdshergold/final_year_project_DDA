using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SpawnLocation : MonoBehaviour
{
    private GameObject[] exitSpawnLocation;
    public GameObject exitPrefab;
    
    void Start()
    {
        exitSpawnLocation = GameObject.FindGameObjectsWithTag("Exit");

        int ran = Random.Range(0, 5);
        _ = Instantiate(exitPrefab, exitSpawnLocation[ran].transform.position, exitSpawnLocation[ran].transform.rotation);
    }
}
