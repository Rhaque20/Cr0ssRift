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

    protected Coroutine _bulletLife = null;
    
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

        if(_hurtBox != null)
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

    private IEnumerator ProjectileLife(float duration)
    {
        yield return new WaitForSeconds(duration);
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
        ReturnProjectile();
    }

    public void FireAtAPosition(Vector3 position, float force, float duration)
    {
        if(_rigid != null)
        {
            _rigid.AddForce((position - transform.position).normalized * force,ForceMode.Impulse);
            _bulletLife = StartCoroutine(ProjectileLife(duration));
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

    public bool IsFacingEachOther(Transform target)
    {
        float dot = Vector3.Dot(transform.right * target.transform.localScale.x, (target.position - transform.position).normalized);
        Debug.Log("Dot product is "+dot);
        return dot <= -0.7;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && _isFriendly)
        {
            EnemyDefenseCore _enemyDefense = other.GetComponent<EnemyDefenseCore>();
            if(_enemyDefense.isParrying && IsFacingEachOther(other.transform) && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnParryable))
            {
                
            }
            else
            {
                other.GetComponent<EnemyStats>().DamageProcess(_activeSkill, _instigator);
            }
        }
        else if (other.gameObject.CompareTag("Player") && !_isFriendly)
        {
            PlayerDefenseCore _playerDefense = other.GetComponent<PlayerDefenseCore>();
            if(_playerDefense.isDodging && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnDodgeable))
            {
                Debug.Log("Evaded projectile");
            }
            else
            {
                other.GetComponent<PlayerStats>().DamageProcess(_activeSkill,_instigator);
            }
        }
    }
    
}