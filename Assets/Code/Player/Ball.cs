using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : State
{
    public Player _player;

    private float _deceleration = 0.1f;

    public int GetState()
    {
        return 2;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            _player.Jump();
            _player.ChangeState(new Jumping()
            {
                _player = _player,
            });
        }
    }

    public void Move()
    {
        _player.Move(0, 0, _deceleration, 0, false);

        if (_player._rb.velocity.x == 0)
        {
            _player.IdleColliders();
            _player.ChangeState(new Running()
            {
                _player = _player,
            });
        }
    }

    public void Fall()
    {
        if (_player._rb.velocity.y < 0)
        {
            _player.ChangeState(new Jumping()
            {
                _player = _player,
            });
        }
    }

    public void Crouch(InputAction.CallbackContext context) { }
    public void Ground(Collider2D other) { }
    public void LookUp(InputAction.CallbackContext context) { }
}
