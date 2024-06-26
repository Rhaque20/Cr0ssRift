//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/EnhancedInput/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Combat"",
            ""id"": ""59a87ac8-5baa-401c-9856-19c0d00b06b7"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""d129b635-3a62-4642-8a88-9967e98b8400"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""NormalAttack"",
                    ""type"": ""Button"",
                    ""id"": ""bae5a420-f5ed-487c-8839-36db7c7d8424"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChargeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""230bef07-da27-426b-a4e4-9b4cff61e4da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwitchLeft"",
                    ""type"": ""Button"",
                    ""id"": ""608edf88-662a-41a1-aa7c-59a5f12c3abd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchRight"",
                    ""type"": ""Button"",
                    ""id"": ""0dd016d3-8b72-46a2-bbe0-9a64fa21534c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleFamiliar"",
                    ""type"": ""Button"",
                    ""id"": ""94d220f6-b279-4daa-bfbc-f618b64e4d58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""eef061fc-59c6-4f9d-b172-9df29eabe1e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Parry"",
                    ""type"": ""Button"",
                    ""id"": ""ed35e041-907d-428f-af7f-b3f9326a2221"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""ddae0c6b-a14c-4154-bf9f-368a594993f9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Switch1"",
                    ""type"": ""Button"",
                    ""id"": ""c596385c-b13b-4814-bad9-e01f2c93d536"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Switch2"",
                    ""type"": ""Button"",
                    ""id"": ""9b7107c3-25cc-464f-bd44-9b2873df6f2c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Switch3"",
                    ""type"": ""Button"",
                    ""id"": ""ffa2ef4f-b41e-4ff2-8c4f-11a43fd766f9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bd5320f1-a24f-4c25-b697-c57b7d360095"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a25bff76-76d6-41f0-b9d4-016c9a9117b9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e0e6fdf6-d839-4783-ba91-b9570340308e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3aa5f327-847e-44ee-b952-228d033efd99"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""91e62967-752f-4c34-9561-905c6366a7ef"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3769a720-6e13-4d1d-93c7-c3b1bd90959a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""NormalAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa9ca529-4305-4056-9f3e-3f187ce88200"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""559e7b6c-0ba4-4882-9c24-e1a2655980f0"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ToggleFamiliar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""113089bb-7b7c-4e05-938b-14289f4dab77"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11e332a0-603d-48a1-bffe-d509185fcbca"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9350460-ded9-42a2-b979-394d3f41781d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ab3f6f6-0ab2-479b-b45c-a1ba20478c59"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChargeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60bb7aee-bd0e-47b9-9825-89f39cebe0ba"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a150adc-4567-4d13-ae27-cd2c14e2368b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1954296-77ff-4952-95ad-92f2081f4ac7"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""953b729c-ac30-4e22-96b4-5b2015f7c3e5"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        }
    ]
}");
        // Combat
        m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
        m_Combat_Movement = m_Combat.FindAction("Movement", throwIfNotFound: true);
        m_Combat_NormalAttack = m_Combat.FindAction("NormalAttack", throwIfNotFound: true);
        m_Combat_ChargeAttack = m_Combat.FindAction("ChargeAttack", throwIfNotFound: true);
        m_Combat_SwitchLeft = m_Combat.FindAction("SwitchLeft", throwIfNotFound: true);
        m_Combat_SwitchRight = m_Combat.FindAction("SwitchRight", throwIfNotFound: true);
        m_Combat_ToggleFamiliar = m_Combat.FindAction("ToggleFamiliar", throwIfNotFound: true);
        m_Combat_PauseGame = m_Combat.FindAction("PauseGame", throwIfNotFound: true);
        m_Combat_Parry = m_Combat.FindAction("Parry", throwIfNotFound: true);
        m_Combat_Dodge = m_Combat.FindAction("Dodge", throwIfNotFound: true);
        m_Combat_Switch1 = m_Combat.FindAction("Switch1", throwIfNotFound: true);
        m_Combat_Switch2 = m_Combat.FindAction("Switch2", throwIfNotFound: true);
        m_Combat_Switch3 = m_Combat.FindAction("Switch3", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Combat
    private readonly InputActionMap m_Combat;
    private List<ICombatActions> m_CombatActionsCallbackInterfaces = new List<ICombatActions>();
    private readonly InputAction m_Combat_Movement;
    private readonly InputAction m_Combat_NormalAttack;
    private readonly InputAction m_Combat_ChargeAttack;
    private readonly InputAction m_Combat_SwitchLeft;
    private readonly InputAction m_Combat_SwitchRight;
    private readonly InputAction m_Combat_ToggleFamiliar;
    private readonly InputAction m_Combat_PauseGame;
    private readonly InputAction m_Combat_Parry;
    private readonly InputAction m_Combat_Dodge;
    private readonly InputAction m_Combat_Switch1;
    private readonly InputAction m_Combat_Switch2;
    private readonly InputAction m_Combat_Switch3;
    public struct CombatActions
    {
        private @PlayerControls m_Wrapper;
        public CombatActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Combat_Movement;
        public InputAction @NormalAttack => m_Wrapper.m_Combat_NormalAttack;
        public InputAction @ChargeAttack => m_Wrapper.m_Combat_ChargeAttack;
        public InputAction @SwitchLeft => m_Wrapper.m_Combat_SwitchLeft;
        public InputAction @SwitchRight => m_Wrapper.m_Combat_SwitchRight;
        public InputAction @ToggleFamiliar => m_Wrapper.m_Combat_ToggleFamiliar;
        public InputAction @PauseGame => m_Wrapper.m_Combat_PauseGame;
        public InputAction @Parry => m_Wrapper.m_Combat_Parry;
        public InputAction @Dodge => m_Wrapper.m_Combat_Dodge;
        public InputAction @Switch1 => m_Wrapper.m_Combat_Switch1;
        public InputAction @Switch2 => m_Wrapper.m_Combat_Switch2;
        public InputAction @Switch3 => m_Wrapper.m_Combat_Switch3;
        public InputActionMap Get() { return m_Wrapper.m_Combat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatActions set) { return set.Get(); }
        public void AddCallbacks(ICombatActions instance)
        {
            if (instance == null || m_Wrapper.m_CombatActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CombatActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @NormalAttack.started += instance.OnNormalAttack;
            @NormalAttack.performed += instance.OnNormalAttack;
            @NormalAttack.canceled += instance.OnNormalAttack;
            @ChargeAttack.started += instance.OnChargeAttack;
            @ChargeAttack.performed += instance.OnChargeAttack;
            @ChargeAttack.canceled += instance.OnChargeAttack;
            @SwitchLeft.started += instance.OnSwitchLeft;
            @SwitchLeft.performed += instance.OnSwitchLeft;
            @SwitchLeft.canceled += instance.OnSwitchLeft;
            @SwitchRight.started += instance.OnSwitchRight;
            @SwitchRight.performed += instance.OnSwitchRight;
            @SwitchRight.canceled += instance.OnSwitchRight;
            @ToggleFamiliar.started += instance.OnToggleFamiliar;
            @ToggleFamiliar.performed += instance.OnToggleFamiliar;
            @ToggleFamiliar.canceled += instance.OnToggleFamiliar;
            @PauseGame.started += instance.OnPauseGame;
            @PauseGame.performed += instance.OnPauseGame;
            @PauseGame.canceled += instance.OnPauseGame;
            @Parry.started += instance.OnParry;
            @Parry.performed += instance.OnParry;
            @Parry.canceled += instance.OnParry;
            @Dodge.started += instance.OnDodge;
            @Dodge.performed += instance.OnDodge;
            @Dodge.canceled += instance.OnDodge;
            @Switch1.started += instance.OnSwitch1;
            @Switch1.performed += instance.OnSwitch1;
            @Switch1.canceled += instance.OnSwitch1;
            @Switch2.started += instance.OnSwitch2;
            @Switch2.performed += instance.OnSwitch2;
            @Switch2.canceled += instance.OnSwitch2;
            @Switch3.started += instance.OnSwitch3;
            @Switch3.performed += instance.OnSwitch3;
            @Switch3.canceled += instance.OnSwitch3;
        }

        private void UnregisterCallbacks(ICombatActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @NormalAttack.started -= instance.OnNormalAttack;
            @NormalAttack.performed -= instance.OnNormalAttack;
            @NormalAttack.canceled -= instance.OnNormalAttack;
            @ChargeAttack.started -= instance.OnChargeAttack;
            @ChargeAttack.performed -= instance.OnChargeAttack;
            @ChargeAttack.canceled -= instance.OnChargeAttack;
            @SwitchLeft.started -= instance.OnSwitchLeft;
            @SwitchLeft.performed -= instance.OnSwitchLeft;
            @SwitchLeft.canceled -= instance.OnSwitchLeft;
            @SwitchRight.started -= instance.OnSwitchRight;
            @SwitchRight.performed -= instance.OnSwitchRight;
            @SwitchRight.canceled -= instance.OnSwitchRight;
            @ToggleFamiliar.started -= instance.OnToggleFamiliar;
            @ToggleFamiliar.performed -= instance.OnToggleFamiliar;
            @ToggleFamiliar.canceled -= instance.OnToggleFamiliar;
            @PauseGame.started -= instance.OnPauseGame;
            @PauseGame.performed -= instance.OnPauseGame;
            @PauseGame.canceled -= instance.OnPauseGame;
            @Parry.started -= instance.OnParry;
            @Parry.performed -= instance.OnParry;
            @Parry.canceled -= instance.OnParry;
            @Dodge.started -= instance.OnDodge;
            @Dodge.performed -= instance.OnDodge;
            @Dodge.canceled -= instance.OnDodge;
            @Switch1.started -= instance.OnSwitch1;
            @Switch1.performed -= instance.OnSwitch1;
            @Switch1.canceled -= instance.OnSwitch1;
            @Switch2.started -= instance.OnSwitch2;
            @Switch2.performed -= instance.OnSwitch2;
            @Switch2.canceled -= instance.OnSwitch2;
            @Switch3.started -= instance.OnSwitch3;
            @Switch3.performed -= instance.OnSwitch3;
            @Switch3.canceled -= instance.OnSwitch3;
        }

        public void RemoveCallbacks(ICombatActions instance)
        {
            if (m_Wrapper.m_CombatActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICombatActions instance)
        {
            foreach (var item in m_Wrapper.m_CombatActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CombatActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CombatActions @Combat => new CombatActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface ICombatActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnNormalAttack(InputAction.CallbackContext context);
        void OnChargeAttack(InputAction.CallbackContext context);
        void OnSwitchLeft(InputAction.CallbackContext context);
        void OnSwitchRight(InputAction.CallbackContext context);
        void OnToggleFamiliar(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnParry(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnSwitch1(InputAction.CallbackContext context);
        void OnSwitch2(InputAction.CallbackContext context);
        void OnSwitch3(InputAction.CallbackContext context);
    }
}
