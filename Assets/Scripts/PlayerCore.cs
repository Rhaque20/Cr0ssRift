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

    [SerializeField]protected bool _hasFamiliarSummoned = false,_canSummonFamiliar = true;

    protected InputAction _chargedAttackAction,_normalAttackAction;
    protected PlayerControls _playerControls;

    protected PlayerStats _playerStats;

    protected PlayerVariables _playerVariables;

    protected PlayerDefenseCore _playerDefenseCore;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _playerInput = GetComponent<PlayerInput>();
        _playerVariables = GetComponent<PlayerVariables>();
        _playerControls = _playerVariables.playerControls;

        _playerStats = GetComponent<PlayerStats>();

        _animOverrideController = _playerVariables.animOverrideController;
        _playerDefenseCore = GetComponent<PlayerDefenseCore>();
    }

    protected void SetCanSummonFamiliar()
    {
        _canSummonFamiliar = !_canSummonFamiliar;
    }

    public void AttackAction(InputAction.CallbackContext ctx)
    {
        if(!_playerDefenseCore.isParrying)
            Attack();
    }

    public void ActivateFamiliarAction(InputAction.CallbackContext ctx)
    {
        if (_canSummonFamiliar)
            _playerVariables.onSummonFamiliar?.Invoke(!_hasFamiliarSummoned);
        else
            Debug.Log("Wait for familiar to recharge");
    }

    public void ActivateFamiliar(bool value)
    {
        _hasFamiliarSummoned = value;
    }

    public override void Attack()
    {
        if(!_isAttacking && _canAttack)
        {
            _isAttacking = true;
            _animOverrideController["Attack"] = _normalAttacks[_currentChain].ReturnAttackAnimation(0);
            _animOverrideController["Recover"] = _normalAttacks[_currentChain].ReturnAttackAnimation(1);
            _anim.Play("Attack");
            _playerVariables.setMove?.Invoke(false);
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
                // Will need to rescale this once skills roll in
                if (_normalAttacks[_currentChain].canOverrideElement)
                {
                    _normalAttacks[_currentChain].attribute = _playerStats.familiarElement;
                }
                else
                {
                    _normalAttacks[_currentChain].attribute = EnumLib.Element.Physical;
                }
                
                entity.GetComponent<Stats>().DamageProcess(_normalAttacks[_currentChain],_playerStats);
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

        _playerVariables.setMove?.Invoke(true);
    }

    public virtual void SwitchOut()
    {
        _hasBuffer = false;
        _canAttack = true;
        _playerControls.Combat.NormalAttack.performed -= AttackAction;
        _playerControls.Combat.ToggleFamiliar.performed -= ActivateFamiliarAction;

        _hasFamiliarSummoned = false;
        
        _playerVariables.onSummonFamiliar?.Invoke(_hasFamiliarSummoned);

        _playerVariables.onForcedUnSummon -= SetCanSummonFamiliar;

        _playerVariables.onSummonFamiliar -= _playerStats.SetElement;
        _playerVariables.onSummonFamiliar -= ActivateFamiliar;
        _playerVariables.onParryEnd -= Recover;
    }

    public virtual void SwitchIn()
    {
  
        if (_playerControls == null)
        {
            Start();
        }

        _playerControls.Combat.NormalAttack.performed += AttackAction;
        _playerControls.Combat.ToggleFamiliar.performed += ActivateFamiliarAction;

        _playerVariables.onForcedUnSummon += SetCanSummonFamiliar;

        _playerVariables.onParryEnd += Recover;

        _playerVariables.onSummonFamiliar += _playerStats.SetElement;
        _playerVariables.onSummonFamiliar += ActivateFamiliar;
        
    }
}
