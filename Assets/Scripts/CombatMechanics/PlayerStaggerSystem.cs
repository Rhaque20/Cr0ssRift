
using System.Collections;
using UnityEngine;

public class PlayerStaggerSystem : StaggerSystem, ISwitchCharacter
{

    void Start()
    {
        base.Start();
    }
    // protected override void Recovery()
    // {
    //     if(!_isArmored)
    //     {
    //         _anim.SetBool("Staggered",false);
    //         GetComponent<GlobalVariables>().setMove?.Invoke(true);
    //         GetComponent<PlayerCore>().Recover();
    //     }
        
    //     _staggerTimer = null;
        

    //     _spriteRenderer.color = Color.white;

        
    // }
    public void SwitchIn()
    {
        
    }

    protected override IEnumerator StaggerTimer()
    {
        if (!_isArmored)
        {
            GetComponent<GlobalVariables>().setMove?.Invoke(false);
            _anim.Play("Stagger");
            _anim.SetBool("Staggered",true);
            GetComponent<PlayerDefenseCore>().OnFailedDefense();
        }
        
        _spriteRenderer.color = Color.red;
        

        yield return new WaitForSeconds(_staggerDuration);

        Recovery();
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