using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skill: ScriptableObject
{
    [SerializeField]
    protected int _damage = 10;

    [SerializeField]
    protected int _traumaDamage = 0;

    [SerializeField]
    protected EnumLib.Element _attribute;

    [SerializeField]
    protected AnimationClip[] _attackAnimations = new AnimationClip[2];

    public AnimationClip ReturnAttackAnimation(int index)
    {
        if (index < 0 || index >= _attackAnimations.Length)
            return null;
        
        return _attackAnimations[index];
    }
}