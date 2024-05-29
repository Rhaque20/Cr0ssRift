

using System;
using UnityEngine;


public class GlobalVariables : MonoBehaviour
{
    public Action<bool> setMove;
    public Action<bool> onArmorBreak;
    public Action<bool> onImmobilized;

    public Action<float> onHealthUpdate;
    public Action<float> onArmorUpdate;

    public Action<Stats> onCountering;
    public Action<Stats> onEvading;

    public Action onBeingCountered;
    public Action endIframes;
    public Action statusTick;
    public Action onDeath;
    [SerializeField]protected AnimatorOverrideController _animOverrideController;

    protected DefenseCore _defenseCore;

    public AnimatorOverrideController animOverrideController
    {
        get {return _animOverrideController;}
    }

    public DefenseCore defenseCore
    {
        get {return _defenseCore;}
    }
}