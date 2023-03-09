//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.3
//     from Assets/Controls/ControlsMap.inputactions
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

public partial class @ControlsMap : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlsMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlsMap"",
    ""maps"": [
        {
            ""name"": ""OnLand"",
            ""id"": ""794a2225-828d-426c-9523-618f195595f2"",
            ""actions"": [
                {
                    ""name"": ""SelectBuildingMinus"",
                    ""type"": ""Button"",
                    ""id"": ""0652ca88-af3d-4f3d-abd6-9869f433e06a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectBuildingPlus"",
                    ""type"": ""Button"",
                    ""id"": ""9866fbcb-5761-41d6-9e03-21f3763098ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenPanel"",
                    ""type"": ""Button"",
                    ""id"": ""03a3aa29-689b-4b48-9a65-c429fdee7d11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e75afa14-49e2-4e9f-adf9-c039fd1c4032"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectBuildingMinus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f08b2c2e-a814-4625-a5f0-f6aebeb434d6"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectBuildingPlus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a818a04-5e0e-41ba-838e-efcdc328788a"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenPanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Anytime"",
            ""id"": ""b9cb5589-d558-4bf5-90c5-8068892a98a1"",
            ""actions"": [
                {
                    ""name"": ""OpenInventory"",
                    ""type"": ""Button"",
                    ""id"": ""73edda37-e00b-4a40-96b5-eee07723b932"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0cadeb3f-b2b7-4878-957c-f7ee32856dba"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Powerups"",
            ""id"": ""7794f4c8-748a-4283-95be-462df955e3e2"",
            ""actions"": [
                {
                    ""name"": ""Powerup1"",
                    ""type"": ""Button"",
                    ""id"": ""01f97849-f29e-4237-93fc-632d0bdb777f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Powerup2"",
                    ""type"": ""Button"",
                    ""id"": ""cd71c68c-8d9c-47fe-aa0d-0ff4ce240809"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Powerup3"",
                    ""type"": ""Button"",
                    ""id"": ""78d10208-5a0f-46e8-8548-358d53f04813"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Powerup4"",
                    ""type"": ""Button"",
                    ""id"": ""cc0b6d28-646d-4246-810c-08dbd9d2912c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e14d50ea-a697-4c1c-a498-a3464a0e7636"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c11ca76-54c3-4a18-85c8-517e8406d91c"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8a72f13-c3a3-48ee-96c8-8556062e4f41"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e731b35c-6b09-4aa9-bdab-a7da9f1442c3"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82f408e5-3d47-45f4-b15e-24315facf14d"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7de4cb0d-345a-478f-a5eb-799a13b1280e"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f97e9167-e88a-4546-9610-4466c4d33b74"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6db271e3-4116-4250-b6c1-8ff97788a597"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d1de801-7f02-40a9-b680-0f2d17d3dff2"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""942a88f2-b536-48d4-9225-f8cbdd2e8a40"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb5a2af9-e90a-4bde-9e98-5ad4ccaa8d0a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36aaf45b-6b58-4423-ab74-855404c4ddde"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Powerup4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // OnLand
        m_OnLand = asset.FindActionMap("OnLand", throwIfNotFound: true);
        m_OnLand_SelectBuildingMinus = m_OnLand.FindAction("SelectBuildingMinus", throwIfNotFound: true);
        m_OnLand_SelectBuildingPlus = m_OnLand.FindAction("SelectBuildingPlus", throwIfNotFound: true);
        m_OnLand_OpenPanel = m_OnLand.FindAction("OpenPanel", throwIfNotFound: true);
        // Anytime
        m_Anytime = asset.FindActionMap("Anytime", throwIfNotFound: true);
        m_Anytime_OpenInventory = m_Anytime.FindAction("OpenInventory", throwIfNotFound: true);
        // Powerups
        m_Powerups = asset.FindActionMap("Powerups", throwIfNotFound: true);
        m_Powerups_Powerup1 = m_Powerups.FindAction("Powerup1", throwIfNotFound: true);
        m_Powerups_Powerup2 = m_Powerups.FindAction("Powerup2", throwIfNotFound: true);
        m_Powerups_Powerup3 = m_Powerups.FindAction("Powerup3", throwIfNotFound: true);
        m_Powerups_Powerup4 = m_Powerups.FindAction("Powerup4", throwIfNotFound: true);
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

    // OnLand
    private readonly InputActionMap m_OnLand;
    private IOnLandActions m_OnLandActionsCallbackInterface;
    private readonly InputAction m_OnLand_SelectBuildingMinus;
    private readonly InputAction m_OnLand_SelectBuildingPlus;
    private readonly InputAction m_OnLand_OpenPanel;
    public struct OnLandActions
    {
        private @ControlsMap m_Wrapper;
        public OnLandActions(@ControlsMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectBuildingMinus => m_Wrapper.m_OnLand_SelectBuildingMinus;
        public InputAction @SelectBuildingPlus => m_Wrapper.m_OnLand_SelectBuildingPlus;
        public InputAction @OpenPanel => m_Wrapper.m_OnLand_OpenPanel;
        public InputActionMap Get() { return m_Wrapper.m_OnLand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OnLandActions set) { return set.Get(); }
        public void SetCallbacks(IOnLandActions instance)
        {
            if (m_Wrapper.m_OnLandActionsCallbackInterface != null)
            {
                @SelectBuildingMinus.started -= m_Wrapper.m_OnLandActionsCallbackInterface.OnSelectBuildingMinus;
                @SelectBuildingMinus.performed -= m_Wrapper.m_OnLandActionsCallbackInterface.OnSelectBuildingMinus;
                @SelectBuildingMinus.canceled -= m_Wrapper.m_OnLandActionsCallbackInterface.OnSelectBuildingMinus;
                @SelectBuildingPlus.started -= m_Wrapper.m_OnLandActionsCallbackInterface.OnSelectBuildingPlus;
                @SelectBuildingPlus.performed -= m_Wrapper.m_OnLandActionsCallbackInterface.OnSelectBuildingPlus;
                @SelectBuildingPlus.canceled -= m_Wrapper.m_OnLandActionsCallbackInterface.OnSelectBuildingPlus;
                @OpenPanel.started -= m_Wrapper.m_OnLandActionsCallbackInterface.OnOpenPanel;
                @OpenPanel.performed -= m_Wrapper.m_OnLandActionsCallbackInterface.OnOpenPanel;
                @OpenPanel.canceled -= m_Wrapper.m_OnLandActionsCallbackInterface.OnOpenPanel;
            }
            m_Wrapper.m_OnLandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SelectBuildingMinus.started += instance.OnSelectBuildingMinus;
                @SelectBuildingMinus.performed += instance.OnSelectBuildingMinus;
                @SelectBuildingMinus.canceled += instance.OnSelectBuildingMinus;
                @SelectBuildingPlus.started += instance.OnSelectBuildingPlus;
                @SelectBuildingPlus.performed += instance.OnSelectBuildingPlus;
                @SelectBuildingPlus.canceled += instance.OnSelectBuildingPlus;
                @OpenPanel.started += instance.OnOpenPanel;
                @OpenPanel.performed += instance.OnOpenPanel;
                @OpenPanel.canceled += instance.OnOpenPanel;
            }
        }
    }
    public OnLandActions @OnLand => new OnLandActions(this);

    // Anytime
    private readonly InputActionMap m_Anytime;
    private IAnytimeActions m_AnytimeActionsCallbackInterface;
    private readonly InputAction m_Anytime_OpenInventory;
    public struct AnytimeActions
    {
        private @ControlsMap m_Wrapper;
        public AnytimeActions(@ControlsMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenInventory => m_Wrapper.m_Anytime_OpenInventory;
        public InputActionMap Get() { return m_Wrapper.m_Anytime; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AnytimeActions set) { return set.Get(); }
        public void SetCallbacks(IAnytimeActions instance)
        {
            if (m_Wrapper.m_AnytimeActionsCallbackInterface != null)
            {
                @OpenInventory.started -= m_Wrapper.m_AnytimeActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.performed -= m_Wrapper.m_AnytimeActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.canceled -= m_Wrapper.m_AnytimeActionsCallbackInterface.OnOpenInventory;
            }
            m_Wrapper.m_AnytimeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OpenInventory.started += instance.OnOpenInventory;
                @OpenInventory.performed += instance.OnOpenInventory;
                @OpenInventory.canceled += instance.OnOpenInventory;
            }
        }
    }
    public AnytimeActions @Anytime => new AnytimeActions(this);

    // Powerups
    private readonly InputActionMap m_Powerups;
    private IPowerupsActions m_PowerupsActionsCallbackInterface;
    private readonly InputAction m_Powerups_Powerup1;
    private readonly InputAction m_Powerups_Powerup2;
    private readonly InputAction m_Powerups_Powerup3;
    private readonly InputAction m_Powerups_Powerup4;
    public struct PowerupsActions
    {
        private @ControlsMap m_Wrapper;
        public PowerupsActions(@ControlsMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Powerup1 => m_Wrapper.m_Powerups_Powerup1;
        public InputAction @Powerup2 => m_Wrapper.m_Powerups_Powerup2;
        public InputAction @Powerup3 => m_Wrapper.m_Powerups_Powerup3;
        public InputAction @Powerup4 => m_Wrapper.m_Powerups_Powerup4;
        public InputActionMap Get() { return m_Wrapper.m_Powerups; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PowerupsActions set) { return set.Get(); }
        public void SetCallbacks(IPowerupsActions instance)
        {
            if (m_Wrapper.m_PowerupsActionsCallbackInterface != null)
            {
                @Powerup1.started -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup1;
                @Powerup1.performed -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup1;
                @Powerup1.canceled -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup1;
                @Powerup2.started -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup2;
                @Powerup2.performed -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup2;
                @Powerup2.canceled -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup2;
                @Powerup3.started -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup3;
                @Powerup3.performed -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup3;
                @Powerup3.canceled -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup3;
                @Powerup4.started -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup4;
                @Powerup4.performed -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup4;
                @Powerup4.canceled -= m_Wrapper.m_PowerupsActionsCallbackInterface.OnPowerup4;
            }
            m_Wrapper.m_PowerupsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Powerup1.started += instance.OnPowerup1;
                @Powerup1.performed += instance.OnPowerup1;
                @Powerup1.canceled += instance.OnPowerup1;
                @Powerup2.started += instance.OnPowerup2;
                @Powerup2.performed += instance.OnPowerup2;
                @Powerup2.canceled += instance.OnPowerup2;
                @Powerup3.started += instance.OnPowerup3;
                @Powerup3.performed += instance.OnPowerup3;
                @Powerup3.canceled += instance.OnPowerup3;
                @Powerup4.started += instance.OnPowerup4;
                @Powerup4.performed += instance.OnPowerup4;
                @Powerup4.canceled += instance.OnPowerup4;
            }
        }
    }
    public PowerupsActions @Powerups => new PowerupsActions(this);
    public interface IOnLandActions
    {
        void OnSelectBuildingMinus(InputAction.CallbackContext context);
        void OnSelectBuildingPlus(InputAction.CallbackContext context);
        void OnOpenPanel(InputAction.CallbackContext context);
    }
    public interface IAnytimeActions
    {
        void OnOpenInventory(InputAction.CallbackContext context);
    }
    public interface IPowerupsActions
    {
        void OnPowerup1(InputAction.CallbackContext context);
        void OnPowerup2(InputAction.CallbackContext context);
        void OnPowerup3(InputAction.CallbackContext context);
        void OnPowerup4(InputAction.CallbackContext context);
    }
}
