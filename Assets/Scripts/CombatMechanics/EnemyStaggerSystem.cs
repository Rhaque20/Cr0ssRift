
using System.Collections;
using UnityEngine;

public class EnemyStaggerSystem : StaggerSystem
{
    private EnemyForces _enemyForces;

    void Start()
    {
        base.Start();
        _enemyForces = GetComponent<EnemyForces>();
    }

    public override void KnockBack(Vector3 attackerPos)
    {
        if (!_isArmored)
        {
            _enemyForces.Knockback(_knockBackPower,attackerPos,_staggerDuration);
        }
            // _rigid.AddForce((attackerPos - transform.position) * -_knockBackPower);

        if(_staggerTimer != null)
        {
            StopCoroutine(_staggerTimer);
        }

        _staggerTimer = StartCoroutine(StaggerTimer());

    }

}