using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : Movement, ISwitchCharacter
{
    PlayerControls _playerControls;
    // Start is called before the first frame update
    void Start()
    {
        _playerControls = GetComponent<PlayerVariables>().playerControls;
        base.Start();
        
    }

    void GetDirectionFromInput(InputAction.CallbackContext ctx)
    {
        Vector2 inputVector = ctx.ReadValue<Vector2>();

        _direction = new Vector3(inputVector.x, 0, inputVector.y);
    }

    void CancelDirection(InputAction.CallbackContext ctx)
    {
        _direction = Vector3.zero;
    }

    protected override void Move()
    {
        if (_direction.x != 0)
            FaceDirection(_direction.x);
        _rigid.MovePosition(transform.position + (_direction.normalized * Time.deltaTime * _moveSpeed * _stats.speedModifier));
        _anim.SetFloat("MoveIntensity",_direction.magnitude);
    }

    void FixedUpdate()
    {
        if(_canMove)
            Move();
    }

    public void SwitchOut()
    {
        GetComponent<PlayerVariables>().setMove -= SetMove;
        _playerControls.Combat.Movement.performed -= GetDirectionFromInput;
        _playerControls.Combat.Movement.canceled -= ctx => _direction = Vector3.zero;
    }

    public void SwitchIn()
    {
        _canMove = true;
        GetComponent<PlayerVariables>().setMove += SetMove;
        if (_playerControls == null)
        {
            Start();
        }
        _playerControls.Combat.Movement.performed += GetDirectionFromInput;
        _playerControls.Combat.Movement.canceled += CancelDirection;
    }
}
