using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovementController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _inputVector;
    private bool _hasMoved = false;
    private Vector3 _forceDirection;

    [SerializeField]
    private float _acceleration;

    [SerializeField]
    private float _topSpeed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // Makes sure that the velocity won't pass the topSpeed if there's a button pressed
        // else stops the character
        _forceDirection = (Vector3)_inputVector;
        if (_forceDirection != Vector3.zero)
        {
            _rb.AddForce(_forceDirection.normalized * _acceleration);
            if (_rb.linearVelocity.magnitude > _topSpeed)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _topSpeed;
            }
        }
        else
        {
            _rb.linearVelocity = Vector2.MoveTowards(
                _rb.linearVelocity,
                Vector2.zero,
                _acceleration
            );
        }
    }
}
