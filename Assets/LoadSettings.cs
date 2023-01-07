using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadSettings : MonoBehaviour
{
    [SerializeField] private InputAction Action;
    public CurrentSettings currentSettings;
    [SerializeField] private PlayerController playerController;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        currentSettings = GameObject.FindGameObjectWithTag("CurrentSettings").GetComponent<CurrentSettings>();
        Action = playerController.playerActionsScript.Player.Jump;
        Action.Disable();
        Action.ApplyBindingOverride(currentSettings.Jump);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Look;
        Action.Disable();
        Action.ApplyBindingOverride(4, currentSettings.RotateCameraRight);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Look;
        Action.Disable();
        Action.ApplyBindingOverride(3, currentSettings.RotateCameraLeft);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Move;
        Action.Disable();
        Action.ApplyBindingOverride(7, currentSettings.MoveRight);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Move;
        Action.Disable();
        Action.ApplyBindingOverride(6, currentSettings.MoveLeft);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Fire;
        Action.Disable();
        Action.ApplyBindingOverride(5, currentSettings.ThrowObject);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Interact;
        Action.Disable();
        Action.ApplyBindingOverride(0, currentSettings.PickupObject);
        Action.Enable();
    }

}
