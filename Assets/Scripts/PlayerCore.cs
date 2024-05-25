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

    protected Animator _familiar;

    protected Coroutine _summonCooldown = null;

    // protected PlayerSkill _activeSkill;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _playerInput = GetComponent<PlayerInput>();
        _playerVariables = GetComponent<PlayerVariables>();
        _playerControls = _playerVariables.playerControls;

        _playerStats = _stats as PlayerStats;

        _animOverrideController = _playerVariables.animOverrideController;
        _playerDefenseCore = GetComponent<PlayerDefenseCore>();
        _familiar = transform.GetChild(1).GetComponent<Animator>();
        _familiar.gameObject.SetActive(false);
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
        if (_canSummonFamiliar && _summonCooldown == null)
            _playerVariables.onSummonFamiliar?.Invoke(!_hasFamiliarSummoned);
        else
            Debug.Log(" and summon Cooldown is null "+_summonCooldown == null);
    }

    public void ActivateFamiliar(bool value)
    {
        _hasFamiliarSummoned = value;
        if (value)
        {
            _familiar.gameObject.SetActive(value);
            _familiar.Play("Summon");
        }
        else
        {
            _summonCooldown = StartCoroutine(SummonCooldown());
        }
        
        _familiar.SetBool("Summoned",value);
    }

    public override void Attack()
    {
        if(!_isAttacking && _canAttack)
        {
            _isAttacking = true;
            _animOverrideController["Attack"] = _normalAttacks[_currentChain].ReturnAttackAnimation(0);
            _animOverrideController["Recover"] = _normalAttacks[_currentChain].ReturnAttackAnimation(1);
            _activeSkill = _normalAttacks[_currentChain];
            _anim.Play("Attack");
            _playerVariables.setMove?.Invoke(false);
        }
        else if (!_hasBuffer && _canAttack)
        {
            _hasBuffer = true;
        }
    }

    // public override void DealDamage(Collider entity)
    // {
    //     EnemyDefenseCore _enemyDefenseCore = entity.GetComponent<EnemyDefenseCore>();

    //     Debug.Log("Hit "+entity.name);

    //     if(_activeSkill == null)
    //         Debug.Log("Active skill is null!");

    //     if(_enemyDefenseCore.isParrying && IsFacingEachOther(entity.transform) && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnParryable))
    //     {
    //         Debug.Log("Parry!");
    //         return;
    //     }
    //     else if(_enemyDefenseCore.isDodging && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnDodgeable))
    //     {
    //         Debug.Log("Evaded");
    //         return;
    //     }

    //     entity.GetComponent<Stats>().DamageProcess(_activeSkill,_playerStats);
    // }

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
                entity.GetComponent<EnemyStaggerSystem>().KnockBack(transform.position);
                // Will need to rescale this once skills roll in
                if (_normalAttacks[_currentChain].canOverrideElement && _hasFamiliarSummoned)
                {
                    _normalAttacks[_currentChain].attribute = _playerStats.familiarElement;
                }
                else
                {
                    _normalAttacks[_currentChain].attribute = EnumLib.Element.Physical;
                }
                
                DealDamage(entity);
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
        else
            _activeSkill = null;

        _playerVariables.setMove?.Invoke(true);
    }

    public IEnumerator SummonCooldown()
    {
        yield return new WaitForSeconds(3f);
        _summonCooldown = null;
    }

    public virtual void SwitchOut()
    {
        _hasBuffer = false;
        _canAttack = true;

        if( _summonCooldown != null)
        {
            StopCoroutine( _summonCooldown);
            _summonCooldown = null;
        }

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
        if(_summonCooldown != null)
        {
            Debug.Log("Somehow summoncooldown wasn't null on new switch in");
            _summonCooldown = null;
        }
        
    }
}
