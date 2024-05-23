using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefenseCore : DefenseCore,ISwitchCharacter
{

    protected bool _canDefend = true;
    protected PlayerControls _playerControls;
    PlayerVariables _playerVariables;
    public void Start()
    {
        base.Start();
        _playerVariables = GetComponent<PlayerVariables>();
        _playerControls = _playerVariables.playerControls;
    }

    protected override IEnumerator IFrames(float duration)
    {
        _playerVariables.setMove?.Invoke(false);
        _canDefend = false;
        yield return new WaitForSeconds(duration);
        _isParrying = false;
        _isDodging = false;

        if(!_isBlocking)
        {
            // Set movement enable here
            _playerVariables.setMove?.Invoke(true);
        }

        _playerVariables.onParryEnd?.Invoke();

        _anim.SetTrigger("iFrameEnd");

        yield return new WaitForSeconds(0.5f);
        _canDefend = true;
        _iFrames = null;
    }

    private void ParryAction(InputAction.CallbackContext ctx)
    {
        if(CanActivate())
            Parry();
    }

    public override bool CanActivate()
    {
        return (!_playerVariables.playerStaggerSystem.isStaggered && !_playerVariables.playerStats.isDead && _canDefend);
    }

    private void ReleaseBlock(InputAction.CallbackContext ctx)
    {
        _isBlocking = false;
        _anim.SetBool("Blocking",_isBlocking);
        _playerVariables.setMove?.Invoke(true);
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

        _isBlocking = false;
        _isParrying = false;

        _playerControls.Combat.Parry.performed -= ParryAction;
        _playerControls.Combat.Parry.canceled -= ReleaseBlock;
    }

    public virtual void SwitchIn()
    {
        if (_playerControls == null)
            Start();
        _playerControls.Combat.Parry.performed += ParryAction;
        _playerControls.Combat.Parry.canceled += ReleaseBlock;
    }
}