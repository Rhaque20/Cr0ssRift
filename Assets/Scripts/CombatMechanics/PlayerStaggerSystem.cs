
using UnityEngine;

public class PlayerStaggerSystem : StaggerSystem, ISwitchCharacter
{

    protected override void Recovery()
    {
        _anim.SetBool("Staggered",false);
        GetComponent<GlobalVariables>().setMove?.Invoke(true);
        _staggerTimer = null;
        

        _spriteRenderer.color = Color.white;

        GetComponent<PlayerCore>().Recover();
    }
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