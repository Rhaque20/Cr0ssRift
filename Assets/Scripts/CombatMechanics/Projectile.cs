using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Skill _activeSkill;
    protected CombatCore _instigator;

    protected BoxCollider _hurtBox;

    protected bool _isFriendly = false;
    
    void Start()
    {
        _hurtBox = GetComponent<BoxCollider>();
    }
    
    public void SetUpProjectile(CombatCore instigator, Skill activeSkill)
    {
        _activeSkill = activeSkill;
        _instigator = instigator;

        if (instigator as PlayerCore)
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
            other.GetComponent<EnemyStats>().DamageProcess(_activeSkill);
        }
        else if (other.gameObject.CompareTag("Player") && !_isFriendly)
        {
            other.GetComponent<PlayerStats>().DamageProcess(_activeSkill);
        }
    }
    
}