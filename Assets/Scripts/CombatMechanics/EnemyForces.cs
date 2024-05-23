using System.Collections;
using UnityEngine;

public class EnemyForces : Forces
{
    Transform _playerPosition;

    EnemyCore _enemyCore;

    void Start()
    {
        base.Start();
        _enemyCore = GetComponent<EnemyCore>();
    }
    public override void Charge(float power)
    {
        _playerPosition = PlayerPartyManager.instance.getActivePlayer.transform;

        Vector3 direction = _playerPosition.position - transform.position;

        _rigid.AddForce(power * direction.normalized,ForceMode.Impulse);
        if(_forceTimer == null)
        {
            _forceTimer = StartCoroutine(ForceTimer(2f));
        }
        _dealsContactDamage = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(_dealsContactDamage)
        {
            if(collision.collider.CompareTag("Player"))
            {
                Debug.Log("Bumped into "+collision.collider.name);
                _enemyCore.DealDamage(collision.collider);
                _collidedTargets.Add(collision.collider);
                Physics.IgnoreCollision(_capsuleCollider,collision.collider,true);
            }
            else if(collision.collider.CompareTag("Enemy"))
            {
                _collidedTargets.Add(collision.collider);
                Physics.IgnoreCollision(_capsuleCollider,collision.collider,true);
            }
        }
        
    }
}