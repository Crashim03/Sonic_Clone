using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

/* 
States:
    0: Running
    1: Jumping
    2: Ball
    3: Spindash
    4: Falling
*/

public class Player : MonoBehaviour
{
    public State _state;
    public Rigidbody2D _rb;
    public Animator _animator;
    public float _speed = 0;
    public float _lastDirection = 1;
    public bool _isSpindashing = false;
    public SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _state = new Running() { _player = this };
    }

    private void Update()
    {
        _animator.SetInteger("State", _state.GetState());
        _animator.SetFloat("Speed", Math.Abs(_speed));
        _animator.SetBool("Spindash", _isSpindashing);

        if (_speed > 0)
        {
            _spriteRenderer.flipX = false;
            _lastDirection = 1;
        }
        else if (_speed < 0)
        {
            _spriteRenderer.flipX = true;
            _lastDirection = -1;
        }
    }

    private void FixedUpdate()
    {
        _state.Move();
    }

    public void JumpAction(InputAction.CallbackContext context) { _state.Jump(context); }

    public void Accelerate(InputAction.CallbackContext context) { _state.Accelerate(context); }

    public void Crouch(InputAction.CallbackContext context) { _state.Crouch(context); }

    public void ChangeState(State state) { _state = state; }

    public void Jump(float jump)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jump);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _state.Ground();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _state.Fall();
    }

    public void Move(float direction, float acceleration, float deceleration, float max_speed)
    {
        if (direction > 0)
        {
            if (_speed < 0)
            {
                _speed += deceleration * 2;

                if (_speed > 0) { _speed = 0; }
            }
            else
            {
                if (_speed < max_speed)
                {
                    _speed += acceleration;

                    if (_speed > max_speed)
                    {
                        _speed = max_speed;
                    }
                }
            }
        }
        else if (direction < 0)
        {
            if (_speed > 0)
            {
                _speed -= deceleration * 2;

                if (_speed < 0) { _speed = 0; }
            }
            else
            {
                if (_speed > -max_speed)
                {
                    _speed -= acceleration;

                    if (_speed < -max_speed)
                    {
                        _speed = -max_speed;
                    }
                }
            }
        }
        else
        {
            if (_speed > 0)
            {
                _speed -= deceleration;

                if (_speed < 0) { _speed = 0; }
            }
            else if (_speed < 0)
            {
                _speed += deceleration;

                if (_speed > 0) { _speed = 0; }
            }
        }

        _rb.velocity = new Vector2(_speed, _rb.velocity.y);
    }
}