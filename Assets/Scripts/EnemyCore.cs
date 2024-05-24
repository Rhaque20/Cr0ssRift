using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCore : CombatCore
{
    [SerializeField]protected EnemySkill[] _moveSet = new EnemySkill[1];
    protected Transform _targetPos;

    protected EnemySkill _activeSkill = null;
    protected Coroutine _idleTimer = null, _attackDelay = null;

    protected Coroutine[] _cooldowns;

    protected EnemyMovement _enemyMove;

    protected EnemyVariables _enemyVariables;

    protected EnemyStats _enemyStats;

    protected int _usedMoveIndex = -1;

    protected const int NORMALATTACK = 0;
    
    void Start()
    {
        base.Start();
        if (_animOverrideController != null)
        {
            _animOverrideController = Instantiate(_animOverrideController);
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _animOverrideController;
        }

        _canAttack = false;

        _idleTimer = StartCoroutine(IdleTimer(Random.Range(1f,3f)));

        _enemyVariables = GetComponent<EnemyVariables>();

        PlayerPartyManager.instance.onPlayerSwitched += UpdateTarget;

        GetComponent<EnemyStats>().onDeath += OnDeath;

        _enemyStats = GetComponent<EnemyStats>();

        _cooldowns = new Coroutine[_moveSet.Length];
        
        for(int i = 0; i < _moveSet.Length; i++)
        {
            _cooldowns[i] = null;
        }
            
    }

    public override void DealDamage(Collider entity)
    {
        PlayerDefenseCore _playerDefenseCore = entity.GetComponent<PlayerDefenseCore>();

        Debug.Log("Hit "+entity.name);

        if(_activeSkill == null)
            Debug.Log("Active skill is null!");

        if(_playerDefenseCore.isParrying && IsFacingEachOther(entity.transform) && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnParryable))
        {
            Debug.Log("Parry!");
            return;
        }
        else if(_playerDefenseCore.isDodging && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnDodgeable))
        {
            Debug.Log("Evaded");
            return;
        }

        Stats stat = entity.GetComponent<Stats>();
        entity.GetComponent<Stats>().DamageProcess(_activeSkill,_enemyStats);

        if(!stat.isDead)
            entity.GetComponent<StaggerSystem>().KnockBack(transform.position);
    }

    public override void HitScan()
    {
        Debug.Log(this.name+"'s Hurtbox has size "+_hurtBox.size);
        _hurtBox.gameObject.SetActive(true);
        List<Collider> entitiesHit = Physics.OverlapBox(_hurtBox.transform.position, _hurtBox.size,_hurtBox.transform.localRotation,_hitLayers).ToList();
        _hurtBox.gameObject.SetActive(false);

        if (entitiesHit.Count > 0)
        {   
            foreach(Collider entity in entitiesHit)
            {
                DealDamage(entity);
                
            }
        }
    }

    public override void OnDeath()
    {
        PlayerPartyManager.instance.onPlayerSwitched -= UpdateTarget;
        if (_idleTimer != null)
            StopCoroutine(_idleTimer);

        foreach(Coroutine skillCoolDown in _cooldowns)
        {
            if(skillCoolDown != null)
            {
                StopCoroutine(skillCoolDown);
            }
        }
    }

    protected IEnumerator IdleTimer(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);
        _canAttack = true;
        _targetPos = GetComponent<EnemyMovement>().targetPos;
    }

    public override void Recover()
    {
        _enemyVariables.setMove?.Invoke(true);
        if (_idleTimer != null)
            StopCoroutine(_idleTimer);
        _isAttacking = false;
        if (_activeSkill == null)
            _idleTimer = StartCoroutine(IdleTimer(2f));
        else
        {
            if(_activeSkill.coolDown != 0 && _cooldowns[_usedMoveIndex] == null)
            {
                Debug.Log("Setting skill "+_usedMoveIndex+"cooldown of "+_activeSkill.name+" to "+_activeSkill.coolDown);
                _cooldowns[_usedMoveIndex] = StartCoroutine(Cooldown(_usedMoveIndex,_activeSkill.coolDown));
            }
            _idleTimer = StartCoroutine(IdleTimer(_activeSkill.idleTime));
        }
        
        _activeSkill = null;
        _usedMoveIndex = -1;
    }

    protected IEnumerator Cooldown(int index, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        _cooldowns[index] = null;
        Debug.Log("Ended cooldown of "+_moveSet[index]);

    }

    protected IEnumerator AttackDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _anim.SetBool("Charging",true);
        _attackDelay = null;
    }
    

    public override void Attack()
    {
        if (_activeSkill.ContainsTag(EnumLib.SkillCategory.Charge))
        {
            _animOverrideController["ChargeWindUp"] = _activeSkill.ReturnAttackAnimation(0);
            _animOverrideController["ActiveCharge"] = _activeSkill.ReturnAttackAnimation(1);
            _animOverrideController["Recover"] = _activeSkill.ReturnAttackAnimation(2);

            _anim.SetBool("Charging",false);
            _anim.Play("ChargeWindUp");
            Debug.Log("Setting up attack delay again: "+_attackDelay == null);
            _attackDelay = StartCoroutine(AttackDelay(2f));

        }
        else
        {
            _animOverrideController["Attack"] = _activeSkill.ReturnAttackAnimation(0);
            _animOverrideController["Recover"] = _activeSkill.ReturnAttackAnimation(1);
            _anim.Play("Attack");
        }
        
        _enemyVariables.setMove?.Invoke(false);
    }

    public override void SkillSelect()
    {
        if(Vector3.Distance(transform.position,_targetPos.position) < 2f)
        {
            _isAttacking = true;
            _canAttack = false;

            _activeSkill = _moveSet[0];

        }
    }

    public void UpdateTarget()
    {
        _targetPos = PlayerPartyManager.instance.getActivePlayer.transform;
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

    protected void Update()
    {
        if(_canAttack && !_isAttacking)
        {
            SkillSelect();
            if (_isAttacking)
            {
                Attack();
            }
        }
    }
}