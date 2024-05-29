using System.Linq;
using UnityEngine;
public class Skill: ScriptableObject
{
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected int _armorDamage = 10;

    [SerializeField] protected int _statusDamage = 0;

    [SerializeField] protected EnumLib.Element _attribute;

    [SerializeField] protected AnimationClip[] _attackAnimations = new AnimationClip[2];
    [SerializeField] protected bool _isChargeable = false;
    [SerializeField] protected EnumLib.HitPower _hitPower = EnumLib.HitPower.None;

    [SerializeField] protected EnumLib.SkillCategory[] _skillTags = new EnumLib.SkillCategory[0];

    public bool ContainsTag(EnumLib.SkillCategory tag)
    {
        return _skillTags.Contains(tag);
    }

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
        set {_attribute = value; }
    }

    public bool isChargeable
    {
        get { return _isChargeable;}
    }

    public float ReturnKnockbackForce()
    {
        switch(_hitPower)
        {
            case EnumLib.HitPower.Weak:
                return 50f;
            case EnumLib.HitPower.Strong:
                return 80f;
            case EnumLib.HitPower.SuperStrong:
                return 150f;
            default:
                return 0f;
        }
    }
}