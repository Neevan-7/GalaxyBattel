
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        transform.position = new Vector3(
            cameraTransform.position.x,
            cameraTransform.position.y,
            5f
        );
    }
}
