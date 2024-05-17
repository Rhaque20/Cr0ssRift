using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCore : CombatCore
{
    [SerializeField]protected EnemySkill[] _moveSet = new EnemySkill[1];
    protected Transform _targetPos;

    protected EnemySkill _activeSkill = null;
    protected Coroutine _idleTimer = null;

    protected EnemyMovement _enemyMove;
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
            
    }

    protected IEnumerator IdleTimer(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);
        _canAttack = true;
        _targetPos = GetComponent<EnemyMovement>().targetPos;
    }

    public override void Recover()
    {
        GetComponent<EnemyVariables>().setMove?.Invoke(true);
        if (_idleTimer != null)
            StopCoroutine(_idleTimer);
        _isAttacking = false;
        if (_activeSkill == null)
            _idleTimer = StartCoroutine(IdleTimer(2f));
        else
            _idleTimer = StartCoroutine(IdleTimer(_activeSkill.idleTime));
        
        _activeSkill = null;
    }

    public override void Attack()
    {
        _animOverrideController["Attack"] = _activeSkill.ReturnAttackAnimation(0);
        _animOverrideController["Recover"] = _activeSkill.ReturnAttackAnimation(1);
        _anim.Play("Attack");
        GetComponent<EnemyVariables>().setMove?.Invoke(false);
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