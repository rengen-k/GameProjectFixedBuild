using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingDisplay : MonoBehaviour
{

    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private GameObject bindingDisplayNameText = null;
    [SerializeField] private GameObject startRebindObject = null;
    [SerializeField] private GameObject waitingForInputObject = null;
    [SerializeField] private InputAction Action;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void RebindRotateCameraRight()
    {

        Action = playerController.playerActionsScript.Player.Look;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(4)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        })
        .Start();

        Action.Enable();    
    }

    public void RebindRotateCameraLeft()
    {

        Action = playerController.playerActionsScript.Player.Look;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(3)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        })
        .Start();

        Action.Enable();
    }

    public void RebindMoveRight()
    {

        Action = playerController.playerActionsScript.Player.Move;
        var vector = Action.ChangeBinding("2D Vector");
        var right = vector.NextPartBinding("Right");
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(right.bindingIndex)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        })
        .Start();

        Action.Enable();
    }

    public void RebindMoveLeft()
    {

        Action = playerController.playerActionsScript.Player.Move;
        var vector = Action.ChangeBinding("2D Vector");
        var left = vector.NextPartBinding("Left");
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(left.bindingIndex)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        })
        .Start();

        Action.Enable();
    }

    public void RebindJump()
    {
        Action = playerController.playerActionsScript.Player.Jump;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding()
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        })
        .Start();

        Action.Enable();
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
    }
}
