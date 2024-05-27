

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalVariables : MonoBehaviour
{
    public Action<bool> setMove;
    public Action<bool> onArmorBreak;
    public Action<float> onHealthUpdate;
    public Action<float> onArmorUpdate;

    public Action onBeingCountered;

    public Action<Stats> onCountering;
    public Action<Stats> onEvading;

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