using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forces : MonoBehaviour, IOnDeath
{
    protected Rigidbody _rigid;

    protected Coroutine _forceTimer = null;

    protected CapsuleCollider _capsuleCollider;

    protected Animator _anim;

    protected List<Collider> _collidedTargets = new List<Collider>();

    protected bool _dealsContactDamage = false;

    protected void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
    }

    protected virtual IEnumerator ForceTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        _forceTimer = null;
        _anim.SetBool("Charging",false);

        _rigid.drag = 10f;

        if(_collidedTargets.Count > 0)
        {
            foreach(Collider col in _collidedTargets)
            {
                Physics.IgnoreCollision(_capsuleCollider,col,false);
            }
        }
        _dealsContactDamage = false;
        _rigid.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.5f);
        _rigid.drag = 0f;

    }


    public void Knockback(float power, Vector3 position,float duration)
    {
        _rigid.AddForce((transform.position - position).normalized * power,ForceMode.Force);
        
        if(_forceTimer == null)
        {
            _forceTimer = StartCoroutine(ForceTimer(duration));
        }
    }

    public virtual void DealContactDamage(GameObject target)
    {

    }

    public virtual void Charge(float power)
    {
        
    }

    public virtual void Charge(float power, Vector3 direction)
    {
        _rigid.AddForce(power * direction,ForceMode.Impulse);
        if(_forceTimer == null)
        {
            _forceTimer = StartCoroutine(ForceTimer(2f));
        }
    }
    public virtual void OnDeath()
    {
        if (_forceTimer != null)
        {
            StopCoroutine(_forceTimer);
        }

        _dealsContactDamage = false;

        if(_collidedTargets.Count > 0)
        {
            foreach(Collider col in _collidedTargets)
            {
                Physics.IgnoreCollision(_capsuleCollider,col,false);
            }
        }

        _rigid.drag = 0f;
    }
}