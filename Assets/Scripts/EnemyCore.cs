using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : CombatCore
{
    void Start()
    {
        if (_animOverrideController != null)
        {
            _animOverrideController = Instantiate(_animOverrideController);
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _animOverrideController;
        }
            
    }
}