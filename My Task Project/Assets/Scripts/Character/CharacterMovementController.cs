using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovementController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private AnimatorController _animatorController;
    private Vector2 _inputVector;
    private Vector3 _forceDirection;

    private float _zAxisOffset = 0.3f; // Offset to fix the depth effect

    [SerializeField]
    private float _acceleration;

    [SerializeField]
    private float _topSpeed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animatorController = GetComponent<AnimatorController>();
    }

    public void OnMove(InputValue value)
    {
        if (DialogueUIController.IsActive())
            return;

        _inputVector = value.Get<Vector2>();
        _animatorController.Move(_inputVector);
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

        // Adjusts the position so it gives the impression of depth when walking by
        // elements in the same layer order;
        // Implemented on items and NPCs aswell
        transform.position = new(
            transform.position.x,
            transform.position.y,
            transform.position.y + _zAxisOffset
        );
    }
}
