using System;
using UnityEngine;

public class CombatCore : MonoBehaviour, IOnDeath
{
    [SerializeField]protected AnimatorOverrideController _animOverrideController;
    protected Animator _anim;

    [SerializeField]protected BoxCollider _hurtBox;
    [SerializeField]protected LayerMask _hitLayers;

    protected bool _isAttacking = false, _canAttack = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public virtual void Attack()
    {

    }

    public virtual void ChargeAttack()
    {
        
    }

    public virtual void Recover()
    {

    }

    public virtual void HitScan()
    {
        Debug.Log(this.name+"'s Hurtbox has size "+_hurtBox.transform.localScale);
        _hurtBox.gameObject.SetActive(true);
        Collider[] entitiesHit = Physics.OverlapBox(_hurtBox.transform.position, _hurtBox.transform.localScale,_hurtBox.transform.localRotation,_hitLayers);
        _hurtBox.gameObject.SetActive(false);

        if (entitiesHit.Length > 0)
        {
            foreach(Collider entity in entitiesHit)
            {
                Stats stat = entity.GetComponent<Stats>();
                Debug.Log("Hit "+entity.name);
                entity.GetComponent<StaggerSystem>().KnockBack(transform.position);
                stat.DealArmorDamage(10,EnumLib.Element.Physical);
                stat.DealDamage(10,EnumLib.Element.Physical);
                stat.DealStatusDamage(EnumLib.Status.Paralyze,10);
            }
        }
    }

    public virtual void SkillSelect()
    {

    }

    public void SetCanAttack(bool value)
    {
        _canAttack = value;
    }

    public virtual void SkillSelect(int i)
    {
        
    }

    public virtual void OnDeath()
    {
        
    }
}
