using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : CombatCore
{
    protected PlayerInput _playerInput;
    [SerializeField] protected PlayerSkill[] _normalAttacks = new PlayerSkill[1];

    protected int _currentChain = 0;

    protected bool _hasBuffer = false;

    [SerializeField]protected BoxCollider _hurtBox;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _anim = transform.GetChild(0).GetComponent<Animator>();

        PlayerControls playerInputActions = GetComponent<PlayerVariables>().playerInputActions;


        playerInputActions.Combat.NormalAttack.performed += ctx => Attack();

        _animOverrideController =  GetComponent<PlayerVariables>().animOverrideController;
    }

    public override void Attack()
    {
        if(!_isAttacking)
        {
            _isAttacking = true;
            _animOverrideController["Attack"] = _normalAttacks[_currentChain].ReturnAttackAnimation(0);
            _animOverrideController["Recover"] = _normalAttacks[_currentChain].ReturnAttackAnimation(1);
            _anim.Play("Attack");
        }
        else if (!_hasBuffer)
        {
            _hasBuffer = true;
        }
    }

    public override void HitScan()
    {
        Debug.Log("Scanning with box size of "+_hurtBox.transform.localScale);
    }

    public override void Recover()
    {
        _currentChain = (_currentChain + 1) % _normalAttacks.Length;
        _isAttacking = false;

        if(_hasBuffer)
        {
            _hasBuffer = false;
            Attack();
        }
    }
}
