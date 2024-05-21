
using UnityEngine;

public class PlayerStaggerSystem : StaggerSystem, ISwitchCharacter
{
    public void SwitchIn()
    {
        
    }

    public void SwitchOut()
    {
        if (_staggerTimer != null)
        {
            StopCoroutine(_staggerTimer);
            _staggerTimer = null;
        }
        _spriteRenderer.color = Color.white;
        Recovery();
    }
}