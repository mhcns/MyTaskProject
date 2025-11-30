using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform;

    public Transform targetTransform;
    private float _defaultCameraSize = 6f;
    private float _targetCameraSize = 6f;

    public float followUpdateTime = 1 / 10f; // Arbitrary default value

    void Start()
    {
        targetTransform = _playerTransform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetTransform.position,
            followUpdateTime
        );

        Camera.main.orthographicSize = Mathf.Lerp(
            Camera.main.orthographicSize,
            _targetCameraSize,
            followUpdateTime
        );
    }

    public void SetCameraTarget(Transform target, float targetCameraSize)
    {
        targetTransform = target;
        _targetCameraSize = targetCameraSize;
    }
}
