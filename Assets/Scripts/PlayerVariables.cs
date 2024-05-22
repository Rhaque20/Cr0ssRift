using System;
using UnityEngine;

public class PlayerVariables: GlobalVariables
{

    [SerializeField]private AnimatorOverrideController _animOverrideController;
    private PlayerControls _playerControls;

    private PlayerStaggerSystem _playerStaggerSystem;

    public Action onSwitchOut;

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

    public void Initialize(PlayerControls _playerCont)
    {
        _playerControls = _playerCont;
        if (_animOverrideController != null)
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _animOverrideController;

        GetComponent<PlayerStats>().Start();

        _playerStaggerSystem = GetComponent<PlayerStaggerSystem>();
    }
}