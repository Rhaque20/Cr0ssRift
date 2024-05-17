using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVariables: MonoBehaviour
{

    [SerializeField]private AnimatorOverrideController _animOverrideController;
    private PlayerInput _playerInput;
    private PlayerControls _playerInputActions;

    public PlayerInput playerInput
    {
        get { return _playerInput; }
    }

    public PlayerControls playerInputActions
    {
        get { return _playerInputActions;}
    }

    public AnimatorOverrideController animatorOverrideController
    {
        get {return _animOverrideController;}
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerControls();
        if (_animOverrideController != null)
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _animOverrideController;
    }
}