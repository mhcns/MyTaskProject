using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;

    public float followUpdateTime = 1 / 10f; // Arbitrary default value

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            _targetTransform.position,
            followUpdateTime
        );
    }
}
