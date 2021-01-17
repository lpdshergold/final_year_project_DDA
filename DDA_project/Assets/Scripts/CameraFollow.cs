
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (player) {
            Debug.Log("here");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        } else {
            Debug.Log("here now");
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
