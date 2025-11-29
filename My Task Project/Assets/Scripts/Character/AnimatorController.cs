using System;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 moveDirection)
    {
        Debug.Log($"moveDirection {moveDirection}");
        if (moveDirection == Vector2.zero)
        {
            // Idle animation
            _animator.SetBool("Moving", false);
        }
        else
        {
            // Checks if the character is moving faster on vertical or horizontal
            _animator.SetBool("Moving", true);
            if (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x))
            {
                _spriteRenderer.flipX = false;
                _animator.SetInteger("MoveX", 0);
                _animator.SetInteger("MoveY", (int)(Mathf.Abs(moveDirection.y) / moveDirection.y));
            }
            else
            {
                // Flips the sprite when moving to the left
                _spriteRenderer.flipX = moveDirection.x < 0;
                _animator.SetInteger("MoveX", 1);
                _animator.SetInteger("MoveY", 0);
            }
        }
    }
}
