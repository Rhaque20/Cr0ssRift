using System.Collections;
using UnityEngine;

public class OgreCore : EnemyCore
{
    const int SLAM = 0, WAVE = 1, ORB = 2;
    [SerializeField]GameObject _columnObject;
    private int _columnsSpawned = 0, _maxColumnSpawns = 3;
    int part = 1;

    private Coroutine _columnSpawnTimer = null;

    public override void SpecialAction()
    {
        if(_usedMoveIndex == SLAM && !_isCanceled)
        {
            _columnSpawnTimer = StartCoroutine(SpawnMultipleColumns());
        }
        else if (_usedMoveIndex == ORB)
        {
            if (part == 1)
            {

            }
            else
            {
                part = 1;
            }
        }
    }

    private IEnumerator SpawnMultipleColumns()
    {
        Projectile _projectile;
        while(_columnsSpawned < _maxColumnSpawns)
        {
            _projectile = Instantiate(_columnObject).GetComponent<Projectile>();
            _projectile.transform.position = _targetPos.position;
            _projectile.SetUpSpriteProjectileDelayed(_stats,_moveSet[SLAM]);
            _columnsSpawned++;
            yield return new WaitForSeconds(0.5f);
            Destroy(_projectile.gameObject);
        }
        _columnsSpawned = 0;
        _columnSpawnTimer = null;
    }

    private IEnumerator OrbFuryRest()
    {
        yield return new WaitForSeconds(2f);
        Recover();
    }

    public override void SkillSelect()
    {
        if(Vector3.Distance(transform.position,_targetPos.position) < 4f && _columnSpawnTimer == null)
        {
            _usedMoveIndex = SLAM;
        }

        if(_usedMoveIndex != -1)
        {
            _isAttacking = true;
            _canAttack = false;

            _activeSkill = _moveSet[_usedMoveIndex];
        }
    }

    // public override void Recover()
    // {
    //     if()
    //     SpecialAction();
    //     base.Recover();
    // }
}