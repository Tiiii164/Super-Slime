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

    private Camera mainCamera;
    public static CameraFollowController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found in scene!");
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + followOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness);
    }

    public void AdjustZoomBasedOnTargetSize()
    {
        followOffset = followOffset * zoomMultiplier;
    }

}
