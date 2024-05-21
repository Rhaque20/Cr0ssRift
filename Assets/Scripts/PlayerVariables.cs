using System;
using UnityEngine;

public class PlayerVariables: GlobalVariables
{

    [SerializeField]private AnimatorOverrideController _animOverrideController;
    private PlayerControls _playerControls;

    public Action onSwitchOut;

    public PlayerControls playerControls
    {
        get { return _playerControls;}
    }

    public AnimatorOverrideController animOverrideController
    {
        get {return _animOverrideController;}
    }

    public void Initialize(PlayerControls _playerCont)
    {
        _playerControls = _playerCont;
        if (_animOverrideController != null)
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _animOverrideController;

        GetComponent<PlayerStats>().Start();
    }
}