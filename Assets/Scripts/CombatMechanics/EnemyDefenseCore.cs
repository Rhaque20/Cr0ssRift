using System.Collections;
using UnityEngine;

public class EnemyDefenseCore : DefenseCore
{
    protected override IEnumerator IFrames(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isParrying = false;
        _isDodging = false;

        _globalVariables.endIframes?.Invoke();

        if(!_isBlocking)
        {
            // Set movement enable here
        }
        else
        {
            _isBlocking = false;
            _anim.SetBool("Blocking",_isBlocking);
        }

        _anim.SetTrigger("iFrameEnd");

        _iFrames = null;
    }
}