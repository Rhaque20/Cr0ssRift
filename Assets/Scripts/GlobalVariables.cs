

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

    public Action statusTick;
    public Action onDeath;
    [SerializeField]protected AnimatorOverrideController _animOverrideController;

    public AnimatorOverrideController animOverrideController
    {
        get {return _animOverrideController;}
    }
}