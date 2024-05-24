
using System.Collections;
using UnityEngine;

public class DefenseCore : MonoBehaviour, IOnDeath
{
    [SerializeField]protected bool _isParryFocused = true, _isBlocking = false;
    [SerializeField]protected float _dodgePower = 10f;

    protected int _maxDodges = 1, _currentDodges = 0;

    protected bool _isParrying = false, _isDodging = false;

    protected Coroutine _iFrames = null;

    [SerializeField]protected BoxCollider _parryBox;
    protected Rigidbody _rigid;

    protected Animator _anim;

    protected Movement _movement;

    public bool isParrying
    {
        get { return _isParrying;}
    }

    public bool isDodging
    {
        get { return _isDodging;}
    }

    protected virtual void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
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

        _anim.SetTrigger("iFrameEnd");
        _iFrames = null;
    }

    public virtual bool CanActivate()
    {
        return false;
    }

    public virtual void OnFailedDefense()
    {
        
    }

    public virtual void Parry()
    {
        if(!_isDodging && !_isParrying && _iFrames == null)
        {
            _isParrying = true;
            _isBlocking = true;
            // Set movement disable here
            _anim.Play("Block");
            _anim.SetBool("Blocking",_isBlocking);
            _iFrames = StartCoroutine(IFrames(0.5f));
        }
        
    }

    public virtual void Dodge()
    {
        if(!_isDodging && !_isParrying && _iFrames == null)
        {
            _isDodging = true;
            if(_movement.direction != Vector3.zero)
                _rigid.AddForce(_dodgePower * _movement.direction,ForceMode.Impulse);
            else
            {
                _rigid.AddForce(-_dodgePower * transform.localScale.x * Vector3.right, ForceMode.Impulse);
            }
            _anim.Play("Dodge");
            
            _iFrames = StartCoroutine(IFrames(0.5f));

        }
    }

    public virtual void OnDeath()
    {
        
    }
}