using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OgreCore : EnemyCore
{
    const int SLAM = 0, WAVE = 1, ORB = 2;
    [SerializeField]GameObject _columnObject, _lightningBullet;
    private int _maxColumnSpawns = 3;
    int part = 1;

    private Coroutine _columnSpawnTimer = null, _orbFuryRest = null;
    Projectile[] _bulletSet = new Projectile[10];
    Projectile[] _columnSet;

    [SerializeField]SphereCollider _hurtSphere;

    private Vector3[] _bulletPositions = new Vector3[10];

    protected override void Start()
    {
        base.Start();

        _columnSet = new Projectile[_maxColumnSpawns];

        ProjectileManager.instance.AddProjectile(_lightningBullet,10);
        ProjectileManager.instance.AddProjectile(_columnObject,_maxColumnSpawns);
        for(int i = 0; i < 10; i++)
        {
            if(i < _maxColumnSpawns)
            {
                _columnSet[i] = ProjectileManager.instance.SummonProjectile(_columnObject).GetComponent<Projectile>();
                _columnSet[i].gameObject.SetActive(false);
            }
            _bulletSet[i] = ProjectileManager.instance.SummonProjectile(_lightningBullet).GetComponent<Projectile>();
            _bulletSet[i].gameObject.SetActive(false);
        }
    }

    public override void HitScan()
    {
        if(_usedMoveIndex == WAVE)
        {
            Debug.Log(this.name+"'s Hurtbox has position "+_hurtBox.transform.position);
            _hurtSphere.gameObject.SetActive(true);
            List<Collider> entitiesHit = Physics.OverlapSphere(_hurtBox.transform.position, _hurtSphere.radius * transform.GetChild(0).localScale.y,_hitLayers).ToList();
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
        else
            base.HitScan();
    }

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
                for(int i = 0; i < _bulletPositions.Length; i++)
                {
                    _bulletSet[i].transform.position = new Vector3(transform.position.x + Random.Range(-2f,2f),8f,
                     transform.position.z + Random.Range(-2f,2f));
                    _bulletSet[i].gameObject.SetActive(true);
                }
                part++;
            }
            else
            {
                part = 1;
                _orbFuryRest = StartCoroutine(OrbFury());
            }
        }
    }


    private IEnumerator SpawnMultipleColumns()
    {
        Projectile _projectile;
        for(int i = 0; i < _maxColumnSpawns; i++)
        {
            _projectile = _columnSet[i];
            _projectile.transform.position = _targetPos.position;
            _projectile.SetUpSpriteProjectileDelayed(_stats,_moveSet[SLAM]);
            yield return new WaitForSeconds(0.5f);
            _projectile.gameObject.SetActive(false);

        }
        _columnSpawnTimer = null;
    }

    private IEnumerator OrbFury()
    {
        for(int i = 0; i < 10; i++)
        {
            _bulletSet[i].SetUpProjectile(_enemyStats,_moveSet[ORB]);
            _bulletSet[i].FireAtAPosition(_targetPos.position,20,2f);
            yield return new WaitForSeconds(0.125f);
        }

        yield return new WaitForSeconds(2f);
        Recover();
    }

    public override void SkillSelect()
    {
        if(Vector3.Distance(transform.position,_targetPos.position) < ActualDistance(4.5f))
        {
            int randomChoice = Random.Range(0,2);
            
            if(_columnSpawnTimer == null && randomChoice == 0)
                _usedMoveIndex = SLAM;
            else
                _usedMoveIndex = WAVE;
        }
        else if (_cooldowns[ORB] == null)
        {
            _usedMoveIndex = ORB;
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

     public override void OnDeath()
    {
        if(_columnSpawnTimer != null)
            StopCoroutine(_columnSpawnTimer);
        if(_orbFuryRest != null)
            StopCoroutine(_orbFuryRest);
        
        base.OnDeath();
    }
}