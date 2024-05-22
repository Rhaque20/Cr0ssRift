
using System.Collections;
using UnityEngine;

public class DefenseCore : MonoBehaviour, IOnDeath
{
    [SerializeField]protected bool _isParryFocused = true, _isBlocking = false;

    protected bool _isParrying = false, _isDodging = false;

    protected Coroutine _iFrames = null;

    [SerializeField]protected BoxCollider _parryBox;
    protected Rigidbody _rigid;

    protected Animator _anim;

    protected Movement _movement;

    protected virtual void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _movement = GetComponent<Movement>();
    }

    protected virtual IEnumerator IFrames(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isParrying = false;
        _isDodging = false;

        if(!_isBlocking)
        {
            // Set movement enable here
        }
    }

    public virtual void Parry()
    {
        if(!_isDodging && !_isParrying && _iFrames == null)
        {
            _isParrying = true;
            _isBlocking = true;
            // Set movement disable here
            _iFrames = StartCoroutine(IFrames(1f));
        }
        
    }

    public virtual void Dodge()
    {
        if(!_isDodging && !_isParrying && _iFrames == null)
        {
            _isDodging = true;
            if(_movement.direction != Vector3.zero)
                _rigid.AddForce(50f * _movement.direction,ForceMode.Impulse);
            else
            {
                _rigid.AddForce(-50f * transform.localScale.x * Vector3.right, ForceMode.Impulse);
            }

            _iFrames = StartCoroutine(IFrames(1f));

        }
    }

    public virtual void OnDeath()
    {
        
    }
}