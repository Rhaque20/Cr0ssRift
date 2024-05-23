using System.Collections;

using UnityEngine;

public class KnightCore : EnemyCore
{
    const int COUNTER = 1, FLAMEBURST = 2, CHARGE = 3;
    public override void SkillSelect()
    {
        if(Vector3.Distance(transform.position,_targetPos.position) < 4f + _capsuleCollider.radius)
        {
            if(_cooldowns[FLAMEBURST] == null)
            {
                _usedMoveIndex = FLAMEBURST;
            }
        }
        else if(Vector3.Distance(transform.position,_targetPos.position) < 2f + _capsuleCollider.radius)
        {
            if(_cooldowns[FLAMEBURST] == null)
            {
                _usedMoveIndex = FLAMEBURST;
            }
            // else if(_cooldowns[COUNTER] == null)
            // {
            //     _usedMoveIndex = COUNTER;
            // }
            else
            {
                _usedMoveIndex = NORMALATTACK;
            }
        }

        if(_usedMoveIndex != -1)
        {
            _isAttacking = true;
            _canAttack = false;

            _activeSkill = _moveSet[_usedMoveIndex];
        }
    }
}