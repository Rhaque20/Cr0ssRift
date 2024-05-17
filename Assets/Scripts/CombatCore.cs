using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCore : MonoBehaviour
{
    [SerializeField]protected AnimatorOverrideController _animOverrideController;
    protected Animator _anim;

    protected bool _isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void Attack()
    {

    }

    public virtual void Recover()
    {

    }

    public virtual void HitScan()
    {
        
    }
}
