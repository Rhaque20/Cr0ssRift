using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefenseCore : DefenseCore,ISwitchCharacter
{
    protected PlayerControls _playerControls;
    PlayerVariables _playerVariables;

    protected Coroutine _dodgeRecharge = null;

    int _numDodges = 1;
    public void Start()
    {
        Debug.Log("Start on player defense core has been called");
        base.Start();
        _playerVariables = GetComponent<PlayerVariables>();
        _playerControls = _playerVariables.playerControls;

        if(!_isParryFocused)
        {
            _maxDodges = 3;
            _numDodges = _maxDodges;
        }
    }

    protected override IEnumerator IFrames(float duration)
    {
        Debug.Log("Triggering iframes");
        _playerVariables.setMove?.Invoke(false);
        _canDefend = false;

        if (_isParrying)
        {
            _parrySignal.SetActive(true);
        }
        else if (_isDodging)
        {
            _dodgeSignal.SetActive(true);
        }

        yield return new WaitForSeconds(duration);
        if (_isParrying)
            _isParrying = false;
        else if(_isDodging)
            _isDodging = false;

        _parrySignal.SetActive(false);
        _dodgeSignal.SetActive(false);

        if(!_isBlocking)
        {
            // Set movement enable here
            _playerVariables.setMove?.Invoke(true);
        }

        _playerVariables.onParryEnd?.Invoke();

        _anim.SetTrigger("iFrameEnd");
        _rigid.velocity = Vector3.zero;
        _numDodges--;

        if(_numDodges > 0)
        {
            _canDefend = true;
            _canDodge = true;
        }

        yield return new WaitForSeconds(0.5f);
        _canDefend = true;
        _iFrames = null;
        _numDodges = _maxDodges;
    }

    private void ParryAction(InputAction.CallbackContext ctx)
    {
        if(CanActivate() && _canParry)
            Parry();
    }

    public override void Dodge()
    {
        if(!_isDodging && !_isParrying && (_iFrames == null || _numDodges > 0))
        {
            _isDodging = true;
            if(_movement.direction != Vector3.zero)
                _rigid.AddForce(_dodgePower * _movement.direction,ForceMode.Impulse);
            else
            {
                _rigid.AddForce(-_dodgePower * transform.localScale.x * Vector3.right, ForceMode.Impulse);
            }
            _anim.Play("Dodge");

            if(_iFrames != null)
            {
                StopCoroutine(_iFrames);
            }
            
            _iFrames = StartCoroutine(IFrames(0.5f));

        }
    }

    private void DodgeAction(InputAction.CallbackContext ctx)
    {
        if(CanActivate() && _canDodge)
            Dodge();
    }

    public override bool CanActivate()
    {
        return !_playerVariables.playerStaggerSystem.isStaggered && !_playerVariables.playerStats.isDead
         && _canDefend;
    }

    private void ReleaseBlock(InputAction.CallbackContext ctx)
    {
        _isBlocking = false;
        _anim.SetBool("Blocking",_isBlocking);
        _playerVariables.setMove?.Invoke(true);
        Debug.Log("Released blocking");
    }

    public override void OnFailedDefense()
    {
        _isParrying = false;
        _isDodging = false;
        _isBlocking = false;

        if(_iFrames != null)
        {
            StopCoroutine(_iFrames);
            _iFrames = null;
        }
        _anim.SetBool("Blocking",_isBlocking);
        
    }
    public virtual void SwitchOut()
    {
        if(_iFrames != null)
        {
            StopCoroutine(_iFrames);
            _iFrames = null;
        }

        _numDodges = _maxDodges;

        _isBlocking = false;
        _isParrying = false;

        _canDefend = true;

        _playerVariables.onParryEnd?.Invoke();

        _playerControls.Combat.Parry.performed -= ParryAction;
        _playerControls.Combat.Parry.canceled -= ReleaseBlock;
        _playerControls.Combat.Dodge.performed -= DodgeAction;
        _rigid.velocity = Vector3.zero;
    }

    public virtual void SwitchIn()
    {
        if (_playerControls == null)
            Start();
        _playerControls.Combat.Parry.performed += ParryAction;
        _playerControls.Combat.Parry.canceled += ReleaseBlock;
        _playerControls.Combat.Dodge.performed += DodgeAction;
    }

    public override void OnDeath()
    {
        SwitchOut();
    }
}