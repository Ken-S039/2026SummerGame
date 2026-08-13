using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float offsetX = 0f;
    public float offsetY = 0f;

    void LateUpdate()
{
    transform.position = new Vector3(
        player.position.x + offsetX,
        transform.position.y,
        transform.position.z
    );
}
}