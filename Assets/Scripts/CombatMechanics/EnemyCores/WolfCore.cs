using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfCore : EnemyCore
{
    const int WARPATTACK = 0, HOWL = 1, EVADE = 2;
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

    public override void SkillSelect()
    {
        if(Vector3.Distance(transform.position,_targetPos.position) < 2f)
        {
            _usedMoveIndex = HOWL;

        }
        else
        {
            _usedMoveIndex = WARPATTACK;
        }

        if(_usedMoveIndex != -1)
        {
            _isAttacking = true;
            _canAttack = false;

            _activeSkill = _moveSet[_usedMoveIndex];
        }
    }

}