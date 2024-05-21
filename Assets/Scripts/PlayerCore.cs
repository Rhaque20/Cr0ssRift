using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : CombatCore,ISwitchCharacter
{
    protected PlayerInput _playerInput;
    [SerializeField] protected PlayerSkill[] _normalAttacks = new PlayerSkill[1];

    protected int _currentChain = 0;

    protected bool _hasBuffer = false;

    protected InputAction _chargedAttackAction,_normalAttackAction;
    protected PlayerControls _playerControls;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _playerInput = GetComponent<PlayerInput>();
        _playerControls = GetComponent<PlayerVariables>().playerControls;


        // _playerControls.Combat.NormalAttack.performed += ctx => Attack();

        _animOverrideController = GetComponent<PlayerVariables>().animOverrideController;
    }

    public void AttackAction(InputAction.CallbackContext ctx)
    {
        Attack();
    }

    public override void Attack()
    {
        if(!_isAttacking && _canAttack)
        {
            _isAttacking = true;
            _animOverrideController["Attack"] = _normalAttacks[_currentChain].ReturnAttackAnimation(0);
            _animOverrideController["Recover"] = _normalAttacks[_currentChain].ReturnAttackAnimation(1);
            _anim.Play("Attack");
            GetComponent<PlayerVariables>().setMove?.Invoke(false);
        }
        else if (!_hasBuffer && _canAttack)
        {
            _hasBuffer = true;
        }
    }

    public override void HitScan()
    {
        _hurtBox.gameObject.SetActive(true);
        Collider[] entitiesHit = Physics.OverlapBox(_hurtBox.transform.position, _hurtBox.transform.localScale,_hurtBox.transform.localRotation,_hitLayers);
        _hurtBox.gameObject.SetActive(false);

        if (entitiesHit.Length > 0)
        {
            foreach(Collider entity in entitiesHit)
            {
                Debug.Log("Hit "+entity.name);
                entity.GetComponent<StaggerSystem>().KnockBack(transform.position);
                entity.GetComponent<Stats>().DamageProcess(_normalAttacks[_currentChain]);
            }
        }
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

        GetComponent<PlayerVariables>().setMove?.Invoke(true);
    }

    public virtual void SwitchOut()
    {
        _hasBuffer = false;
        _canAttack = true;
        _playerControls.Combat.NormalAttack.performed -= AttackAction;
        Debug.Log("Unsubscribing "+this.name+"'s Attack function");
    }

    public virtual void SwitchIn()
    {
        // Deal with fact subscription breaks on using normal attack with switch character
        if (_playerControls == null)
        {
            Start();
        }
        Debug.Log("Subscribing "+this.name+"'s Attack function");
        _playerControls.Combat.NormalAttack.performed += AttackAction;
    }
}
