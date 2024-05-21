using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Skill _activeSkill;
    protected Stats _instigator;

    protected BoxCollider _hurtBox;

    protected bool _isFriendly = false;
    
    void Start()
    {
        _hurtBox = GetComponent<BoxCollider>();
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