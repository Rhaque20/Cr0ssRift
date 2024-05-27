using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Skill _activeSkill;
    protected Stats _instigator;

    protected BoxCollider _hurtBox;

    protected bool _isFriendly = false;

    protected Animator _anim;

    protected GameObject _originalPrefab;

    protected Rigidbody _rigid;
    
    void Start()
    {
        _hurtBox = GetComponent<BoxCollider>();
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }
    
    public void SetUpProjectile(Stats instigator, Skill activeSkill)
    {
        _activeSkill = activeSkill;
        _instigator = instigator;

        if (instigator as PlayerStats)
        {
            _isFriendly = true;
            Debug.Log("Fired from friendly");
        }
        else
        {
            _isFriendly = false;
        }

        _hurtBox.enabled = true;
    }

    public void SetUpPrefab(GameObject prefab)
    {
        _originalPrefab = prefab;
    }

    public void ActivateCollider()
    {
        _hurtBox.enabled = true;
    }
    public void DestroyProjectile()
    {
        // Will replace with disable when object pooling method established
        
        Destroy(this);
            
    }

    public void ReturnProjectile()
    {
        if (ProjectileManager.instance != null)
            ProjectileManager.instance.ReturnProjectile(gameObject,_originalPrefab);
    }

    public void FireAtAPosition(Vector3 position, float force)
    {
        if(_rigid != null)
        {
            _rigid.AddForce((position - transform.position).normalized * force,ForceMode.Impulse);
        }
        else
            Debug.Log("Rigidbody is null");
    }

    public void SetUpSpriteProjectileDelayed(Stats instigator, Skill activeSkill)
    {
        Start();
        if(_anim != null)
        {
            _activeSkill = activeSkill;
            _instigator = instigator;

            if (instigator as PlayerStats)
            {
                _isFriendly = true;
            }
            else
            {
                _isFriendly = false;
                Debug.Log("Burst from enemy");
            }
            _anim.Play("Burst");
            _hurtBox.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && _isFriendly)
        {
            other.GetComponent<EnemyStats>().DamageProcess(_activeSkill, _instigator);
        }
        else if (other.gameObject.CompareTag("Player") && !_isFriendly)
        {
            other.GetComponent<PlayerStats>().DamageProcess(_activeSkill,_instigator);
        }
    }
    
}