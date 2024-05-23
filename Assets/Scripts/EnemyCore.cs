using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyCore : CombatCore
{
    [SerializeField]protected EnemySkill[] _moveSet = new EnemySkill[1];
    protected Transform _targetPos;

    protected EnemySkill _activeSkill = null;
    protected Coroutine _idleTimer = null;

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

    public override void HitScan()
    {
        Debug.Log(this.name+"'s Hurtbox has size "+_hurtBox.transform.localScale);
        _hurtBox.gameObject.SetActive(true);
        Collider[] entitiesHit = Physics.OverlapBox(_hurtBox.transform.position, _hurtBox.transform.localScale,_hurtBox.transform.localRotation,_hitLayers);
        _hurtBox.gameObject.SetActive(false);

        bool _scannedBoxFirst = false, _scannedPlayer = false;

        if (entitiesHit.Length > 0)
        {
            foreach(Collider entity in entitiesHit)
            {
                if(entity.CompareTag("Block"))
                {
                    Debug.Log("Scanned counterbox");
                    if (!_scannedPlayer)
                        _scannedBoxFirst = true;
                }
                else
                {
                    Stats stat = entity.GetComponent<Stats>();
                    Debug.Log("Hit "+entity.name);
                    stat.DamageProcess(_activeSkill,_enemyStats);

                    if(!stat.isDead)
                        entity.GetComponent<StaggerSystem>().KnockBack(transform.position);
                    
                    _scannedPlayer = true;
                }
            }

            if(_scannedBoxFirst)
            {
                Debug.Log("hit the counter box first");
            }
            else
            {
                Debug.Log("Hit the player first");
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
            if(_activeSkill.coolDown != 0)
            {
                _cooldowns[_usedMoveIndex] = StartCoroutine(Cooldown(_usedMoveIndex,_activeSkill.coolDown));
            }
            _idleTimer = StartCoroutine(IdleTimer(_activeSkill.idleTime));
        }
        
        _activeSkill = null;
    }

    protected IEnumerator Cooldown(int index, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        _cooldowns[_usedMoveIndex] = null;

    }

    public override void Attack()
    {
        _animOverrideController["Attack"] = _activeSkill.ReturnAttackAnimation(0);
        _animOverrideController["Recover"] = _activeSkill.ReturnAttackAnimation(1);
        _anim.Play("Attack");
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