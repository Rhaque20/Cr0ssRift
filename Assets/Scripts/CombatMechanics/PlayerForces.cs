using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForces : Forces,ISwitchCharacter
{
    public void SwitchOut()
    {
        if (_forceTimer != null)
        {
            StopCoroutine(_forceTimer);
        }

        if(_collidedTargets.Count > 0)
        {
            foreach(Collider col in _collidedTargets)
            {
                Physics.IgnoreCollision(_capsuleCollider,col,false);
            }
        }

        _rigid.drag = 0f;
    }

    public void KnockbackWave()
    {
        
    }

    public void SwitchIn()
    {
        
    }

    public override void OnDeath()
    {
        SwitchOut();
    }
}