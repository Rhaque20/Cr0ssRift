

using System;
using System.Collections;
using System.Collections.Generic;
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

    public virtual void CleanUp()
    {
        Delegate[] delegateArray = setMove.GetInvocationList();

        foreach (Delegate d in delegateArray)
            setMove -= (Action<bool>)d;

        delegateArray = onArmorBreak.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onArmorBreak -= (Action<bool>)d;

        delegateArray = onImmobilized.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onImmobilized -= (Action<bool>)d;



        delegateArray = onHealthUpdate.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onHealthUpdate -= (Action<float>)d;
        
        delegateArray = onCountering.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onArmorUpdate -= (Action<float>)d;



        delegateArray = onCountering.GetInvocationList();
        foreach (Delegate d in delegateArray)
            onCountering -= (Action<Stats>)d;

        delegateArray = onEvading.GetInvocationList();
        foreach (Delegate d in delegateArray)
            onEvading -= (Action<Stats>)d;

        
        delegateArray = onBeingCountered.GetInvocationList();
        foreach (Delegate d in delegateArray)
            onBeingCountered -= (Action)d;

        delegateArray = endIframes.GetInvocationList();
        foreach (Delegate d in delegateArray)
            endIframes -= (Action)d;
        
        delegateArray = statusTick.GetInvocationList();
        foreach (Delegate d in delegateArray)
            statusTick -= (Action)d;

    }
}