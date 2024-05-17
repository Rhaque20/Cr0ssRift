using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Movement
{
    PlayerInput _playerInput;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _playerInput = GetComponent<PlayerInput>();
        PlayerControls playerInputActions = GetComponent<PlayerVariables>().playerInputActions;
        playerInputActions.Combat.Enable();
        playerInputActions.Combat.Movement.performed += GetDirectionFromInput;
        playerInputActions.Combat.Movement.canceled += ctx => _direction = Vector3.zero;
        
    }

    void GetDirectionFromInput(InputAction.CallbackContext ctx)
    {
        Vector2 inputVector = ctx.ReadValue<Vector2>();

        _direction = new Vector3(inputVector.x, 0, inputVector.y);
    }

    protected override void Move()
    {
        if (_direction.x != 0)
            FaceDirection(_direction.x);
        _rigid.MovePosition(transform.position + (_direction.normalized * Time.deltaTime * _moveSpeed));
        _anim.SetFloat("MoveIntensity",_direction.magnitude);
    }

    void FixedUpdate()
    {
        Move();
    }
}
