
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (player) {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        } else {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
