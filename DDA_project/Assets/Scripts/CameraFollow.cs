
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Update() {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
