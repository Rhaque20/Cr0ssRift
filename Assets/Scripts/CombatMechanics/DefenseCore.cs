
using System.Collections;
using UnityEngine;

public class DefenseCore : MonoBehaviour, IOnDeath
{
    [SerializeField]protected bool _isParryFocused = true;
    [SerializeField]protected float _dodgePower = 10f;

    protected bool _canDefend = true;

    protected int _maxDodges = 1, _currentDodges = 0;

    protected bool _isParrying = false, _isDodging = false,_isBlocking = false;
    protected bool _canParry = true, _canDodge = true;
    protected bool _effectTriggered = false;
    protected bool _extendIframes = false;

    protected Coroutine _iFrames = null;

    [SerializeField]protected GameObject _parrySignal, _dodgeSignal;
    protected Rigidbody _rigid;

    protected Animator _anim;

    protected Movement _movement;
    
    [SerializeField]GameObject _defenseEffect;

    protected GlobalVariables _globalVariables;

    public bool isParrying
    {
        get { return _isParrying;}
    }

    public bool isDodging
    {
        get { return _isDodging;}
    }

    public bool isBlocking
    {
        get { return _isBlocking;}
    }

    protected virtual void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _movement = GetComponent<Movement>();
        _globalVariables = GetComponent<GlobalVariables>();
        _globalVariables.onImmobilized += DisableDefend;
    }

    protected virtual IEnumerator IFrames(float duration)
    {

        if (_isParrying)
        {
            _parrySignal.SetActive(true);
        }
        else if (_isDodging)
        {
            _dodgeSignal.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        _isParrying = false;
        _isDodging = false;

        _parrySignal.SetActive(false);
        _dodgeSignal.SetActive(false);

        if(!_isBlocking)
        {
            // Set movement enable here
        }

        _anim.SetTrigger("iFrameEnd");
        _iFrames = null;
        _extendIframes = false;
    }

    public virtual bool CanActivate()
    {
        return false;
    }

    public virtual void OnFailedDefense()
    {
        
    }

    public virtual void DisableDefend(bool value)
    {
        if(!value)
        {
            if (_iFrames != null)
            {
                StopCoroutine(_iFrames);
            }
        }
 
        _canDefend = value;
    }

    protected IEnumerator DefenseEffectTimer()
    {
        _defenseEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _defenseEffect.SetActive(false);
        _effectTriggered = false;
    }

    public virtual void DefenseEffect()
    {
        _effectTriggered = true;
        StartCoroutine(DefenseEffectTimer());
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

    public virtual void Counter(Stats attackerStats)
    {
        attackerStats.CounterArmorDamage(10);
        DefenseEffect();
        _globalVariables.onCountering?.Invoke(attackerStats);
        if(!_extendIframes)
        {
            _extendIframes = true;
            if(_iFrames != null)
                StopCoroutine(_iFrames);
            _iFrames = StartCoroutine(IFrames(0.5f));
        }
    }

    public virtual void Evaded(Stats attackerStats)
    {
        DefenseEffect();
        _globalVariables.onEvading?.Invoke(attackerStats);
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