using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skill: ScriptableObject
{
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected int _armorDamage = 10;

    [SerializeField] protected int _statusDamage = 0;

    [SerializeField] protected EnumLib.Element _attribute;

    [SerializeField] protected AnimationClip[] _attackAnimations = new AnimationClip[2];

    public AnimationClip ReturnAttackAnimation(int index)
    {
        if (index < 0 || index >= _attackAnimations.Length)
            return null;
        
        return _attackAnimations[index];
    }

    public int damage
    {
        get { return _damage; }
    }

    public int armorDamage
    {
        get { return _armorDamage; }
    }

    public int statusDamage
    {
        get { return _statusDamage; }
    }

    public EnumLib.Element attribute
    {
        get { return _attribute; }
    }
}