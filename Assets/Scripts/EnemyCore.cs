using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCore : CombatCore
{
    [SerializeField]protected EnemySkill[] _moveSet = new EnemySkill[1];
    protected Transform _targetPos;
    protected Coroutine _idleTimer = null, _attackDelay = null;

    protected Coroutine[] _cooldowns;

    protected EnemyMovement _enemyMove;

    protected EnemyVariables _enemyVariables;

    protected EnemyStats _enemyStats;

    protected int _usedMoveIndex = -1;

    protected const int NORMALATTACK = 0;

    public EnemySkill activeSkill
    {
        get { return _activeSkill as EnemySkill; }
    }
    
    protected override void Start()
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
        _enemyStats = _stats as EnemyStats;

        _enemyStats.onDeath += OnDeath;

        _cooldowns = new Coroutine[_moveSet.Length];
        
        for(int i = 0; i < _moveSet.Length; i++)
        {
            _cooldowns[i] = null;
        }
            
    }

    public float ActualDistance(float originalRange)
    {
        return _capsuleCollider.radius + originalRange;
    }

    public override void HitScan()
    {
        Debug.Log(this.name+"'s Hurtbox has position "+_hurtBox.transform.position);
        _hurtBox.gameObject.SetActive(true);
        List<Collider> entitiesHit = Physics.OverlapBox(_hurtBox.transform.position, _hurtBox.size * transform.GetChild(0).localScale.y,_hurtBox.transform.localRotation,_hitLayers).ToList();
        _hurtBox.gameObject.SetActive(false);

        if (entitiesHit.Count > 0)
        {   
            foreach(Collider entity in entitiesHit)
            {
                DealDamage(entity);
                
            }
        }
        else
        {
            Debug.Log("Hit nothing");
        }
    }

    public override void OnDeath()
    {
        PlayerPartyManager.instance.onPlayerSwitched -= UpdateTarget;
        if (_idleTimer != null)
            StopCoroutine(_idleTimer);

        _enemyVariables.onBeingCountered -= CancelAction;

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
        yield return new WaitForSeconds(idleTime * 0.9f);
        _enemyVariables.readyToAttack(true);
        yield return new WaitForSeconds(idleTime * 0.1f);
        _canAttack = true;
        _targetPos = GetComponent<EnemyMovement>().targetPos;
        Debug.Log("Ready to fight");
    }

    public override void Recover()
    {
        _enemyVariables.setMove?.Invoke(true);
        _enemyVariables.readyToAttack(false);
        if (_idleTimer != null)
            StopCoroutine(_idleTimer);
        _isAttacking = false;
        if (_activeSkill == null)
            _idleTimer = StartCoroutine(IdleTimer(2f));
        else
        {
            if(activeSkill.coolDown != 0 && _cooldowns[_usedMoveIndex] == null)
            {
                Debug.Log("Setting skill "+_usedMoveIndex+"cooldown of "+_activeSkill.name+" to "+activeSkill.coolDown);
                _cooldowns[_usedMoveIndex] = StartCoroutine(Cooldown(_usedMoveIndex,activeSkill.coolDown));
            }
            _idleTimer = StartCoroutine(IdleTimer(activeSkill.idleTime));
        }
        
        _activeSkill = null;
        _usedMoveIndex = -1;
        _isCanceled = false;
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
        if(Vector3.Distance(transform.position,_targetPos.position) < ActualDistance(2f))
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