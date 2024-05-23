using System;
using UnityEngine;

public class PlayerVariables: GlobalVariables
{

    [SerializeField]private AnimatorOverrideController _animOverrideController;
    private PlayerControls _playerControls;

    private PlayerStaggerSystem _playerStaggerSystem;

    private PlayerStats _playerStats;

    public Action onSwitchOut;

    public Action onAttack;

    public Action onParryEnd;

    public Action<bool> onSummonFamiliar;

    public Action onForcedUnSummon;

    public PlayerControls playerControls
    {
        get { return _playerControls;}
    }

    public AnimatorOverrideController animOverrideController
    {
        get {return _animOverrideController;}
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
    }
}