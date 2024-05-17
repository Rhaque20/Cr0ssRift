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

    
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _playerInput = GetComponent<PlayerInput>();
        PlayerControls playerInputActions = GetComponent<PlayerVariables>().playerInputActions;


        playerInputActions.Combat.NormalAttack.performed += ctx => Attack();

        _animOverrideController =  GetComponent<PlayerVariables>().animOverrideController;
    }

    public override void Attack()
    {
        if(!_isAttacking && _canAttack)
        {
            _isAttacking = true;
            _animOverrideController["Attack"] = _normalAttacks[_currentChain].ReturnAttackAnimation(0);
            _animOverrideController["Recover"] = _normalAttacks[_currentChain].ReturnAttackAnimation(1);
            _anim.Play("Attack");
        }
        else if (!_hasBuffer && _canAttack)
        {
            _hasBuffer = true;
        }
    }

    // public override void HitScan()
    // {
    //     _hurtBox.gameObject.SetActive(true);
    //     Collider[] entitiesHit = Physics.OverlapBox(_hurtBox.transform.position, _hurtBox.transform.localScale,_hurtBox.transform.localRotation,_hitLayers);
    //     _hurtBox.gameObject.SetActive(false);

    //     if (entitiesHit.Length > 0)
    //     {
    //         foreach(Collider entity in entitiesHit)
    //         {
    //             Debug.Log("Hit "+entity.name);
    //             entity.GetComponent<StaggerSystem>().KnockBack(transform.position);
    //         }
    //     }
    // }

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
