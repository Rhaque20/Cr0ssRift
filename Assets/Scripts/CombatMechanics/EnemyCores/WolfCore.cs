using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfCore : EnemyCore
{
    const int WARPATTACK = 0, HOWL = 1, EVADE = 2;

    protected override void Start()
    {
        base.Start();
        _enemyVariables.onEvading += EvasionEffect;
        _enemyVariables.endIframes += Recover;
    }
    public override void SpecialAction()
    {
        Debug.Log("Calling special action");
        if(_usedMoveIndex == WARPATTACK)
        {
            Debug.Log("Warping behind them!");
            float xScale = _targetPos.localScale.x;
            transform.position = new Vector3(_targetPos.position.x - (0.5f * xScale),_targetPos.position.y, _targetPos.position.z);

            transform.localScale = new Vector3(xScale,transform.localScale.y,transform.localScale.z);
        }
    }

    public override void EvasionEffect(Stats attackerStats)
    {
        Debug.Log("Countered "+attackerStats.name);
        PlayerStats _playerStats = attackerStats as PlayerStats;
        
        _playerStats.DealStatusDamage(10,EnumLib.Status.Frozen);
    }

    public override void Attack()
    {
        if(_usedMoveIndex == EVADE)
        {
            _defenseCore.Dodge();
        }
        else
            base.Attack();
    }

    public override void SkillSelect()
    {
        if(Vector3.Distance(transform.position,_targetPos.position) < 2f)
        {
            if(_cooldowns[EVADE] == null)
            {
                Debug.Log("Choosing Evade");
                _usedMoveIndex = EVADE;
            }
            else
            {
                _usedMoveIndex = HOWL;
                Debug.Log("Choosing Howl");
            }

        }
        else
        {
            Debug.Log("Choosing warp");
            _usedMoveIndex = WARPATTACK;
        }

        if(_usedMoveIndex != -1)
        {
            _isAttacking = true;
            _canAttack = false;

            _activeSkill = _moveSet[_usedMoveIndex];
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();

        _enemyVariables.onEvading -= EvasionEffect;
    }

}