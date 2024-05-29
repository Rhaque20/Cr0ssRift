using UnityEngine.UI;
using UnityEngine;

public class CombatCore : MonoBehaviour, IOnDeath
{
    [SerializeField]protected AnimatorOverrideController _animOverrideController;
    protected Animator _anim;

    [SerializeField]protected BoxCollider _hurtBox;
    [SerializeField]protected LayerMask _hitLayers;

    protected Skill _activeSkill;

    protected Stats _stats;

    protected CapsuleCollider _capsuleCollider;

    protected DefenseCore _defenseCore;

    [SerializeField]protected Image _chargeBar;

    protected Image _isReadyIcon;

    protected bool _charging = false;

    protected bool _isAttacking = false, _canAttack = true, _isCanceled = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animOverrideController = GetComponent<GlobalVariables>().animOverrideController;
        _stats = GetComponent<Stats>();
        _defenseCore = GetComponent<DefenseCore>();
        GetComponent<GlobalVariables>().onBeingCountered += CancelAction;
        GetComponent<GlobalVariables>().onImmobilized += SetCanAttack;
    }

    public virtual void Attack()
    {

    }

    public virtual void SpecialAction()
    {

    }

    public virtual void CancelAction()
    {
        Debug.Log("Parry canceled");
        _isCanceled = true;
    }

    public virtual void ChargeAttack()
    {
        
    }

    public virtual void Recover()
    {

    }

    public virtual void EvasionEffect(Stats attackerStats)
    {

    }

    public virtual void ParryEffect(Stats attackerStats)
    {

    }

    public bool IsFacingEachOther(Transform target)
    {
        float dot = Vector3.Dot(transform.right * target.transform.localScale.x, (target.position - transform.position).normalized);
        Debug.Log("Dot product is "+dot);
        return dot <= -0.7;
    }

    public virtual void DealDamage(Collider entity)
    {
        DefenseCore _defenseCore = entity.GetComponent<DefenseCore>();

        Debug.Log("Dealing damage to "+entity.name);
        if(_defenseCore.isParrying && IsFacingEachOther(entity.transform) && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnParryable))
        {
            Debug.Log("Parry!");
            _defenseCore.Counter(_stats);
            GetComponent<GlobalVariables>().onBeingCountered?.Invoke();
            return;
        }
        else if(_defenseCore.isDodging && !_activeSkill.ContainsTag(EnumLib.SkillCategory.UnDodgeable))
        {
            Debug.Log("Evaded");
            _defenseCore.Evaded(_stats);
            return;
        }

        Stats stat = entity.GetComponent<Stats>();
        entity.GetComponent<Stats>().DamageProcess(_activeSkill,_stats);

        if(!stat.isDead)
            entity.GetComponent<StaggerSystem>().KnockBack(transform.position, _activeSkill.ReturnKnockbackForce());

    }

    public virtual void HitScan()
    {
        Debug.Log(this.name+"'s Hurtbox has size "+_hurtBox.size);
        _hurtBox.gameObject.SetActive(true);
        Collider[] entitiesHit = Physics.OverlapBox(_hurtBox.transform.position, _hurtBox.size,_hurtBox.transform.localRotation,_hitLayers);
        _hurtBox.gameObject.SetActive(false);

        if (entitiesHit.Length > 0)
        {
            foreach(Collider entity in entitiesHit)
            {
                DealDamage(entity);
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
