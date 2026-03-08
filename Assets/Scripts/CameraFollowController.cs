using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float followSmoothness = 0.125f;
    [SerializeField] private Vector3 followOffset = new Vector3(0, 5, -10);

    [Header("Zoom Settings")]
    [SerializeField] private float zoomMultiplier = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found in scene!");
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        FollowTarget();
        AdjustZoomBasedOnTargetSize();
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + followOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness);
    }

    private void AdjustZoomBasedOnTargetSize()
    {
        float targetSize = target.localScale.magnitude;
        float desiredZoom = Mathf.Clamp(minZoom + targetSize * zoomMultiplier, minZoom, maxZoom);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, desiredZoom, Time.deltaTime);
    }
}
