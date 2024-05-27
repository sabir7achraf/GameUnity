using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 newPosition = playerTransform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }

    public void SetPlayer(Transform newPlayerTransform)
    {
        playerTransform = newPlayerTransform;
    }
}
