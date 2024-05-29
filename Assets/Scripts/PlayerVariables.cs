using System;
using UnityEngine;

public class PlayerVariables: GlobalVariables
{
    private PlayerControls _playerControls;

    private PlayerStaggerSystem _playerStaggerSystem;

    private PlayerStats _playerStats;

    public Action<bool> onSummonFamiliar;

    public Action onSwitchOut;

    public Action onParryEnd;
    public Action onForcedUnSummon;

    public PlayerControls playerControls
    {
        get { return _playerControls;}
    }

    public PlayerStaggerSystem playerStaggerSystem
    {
        get {return _playerStaggerSystem;}
    }

    public PlayerStats playerStats
    {
        get {return _playerStats;}
    }

    public void Initialize(PlayerControls _playerCont)
    {
        _playerControls = _playerCont;
        if (_animOverrideController != null)
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _animOverrideController;

        _playerStats = GetComponent<PlayerStats>();
        _playerStats.Start();

        _playerStaggerSystem = GetComponent<PlayerStaggerSystem>();
        _defenseCore = GetComponent<PlayerDefenseCore>();
    }

    public override void CleanUp()
    {
        base.CleanUp();

        Delegate[] delegateArray = onSummonFamiliar.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onSummonFamiliar -= (Action<bool>)d;

        delegateArray = onSwitchOut.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onSwitchOut -= (Action)d;   

        delegateArray = onParryEnd.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onParryEnd -= (Action)d;   

        delegateArray = onForcedUnSummon.GetInvocationList();

        foreach (Delegate d in delegateArray)
            onForcedUnSummon -= (Action)d;         
    }
}